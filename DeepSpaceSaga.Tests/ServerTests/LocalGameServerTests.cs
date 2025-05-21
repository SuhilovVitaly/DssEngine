using DeepSpaceSaga.Common.Abstractions.Dto;
using DeepSpaceSaga.Common.Abstractions.Entities;
using DeepSpaceSaga.Server.Services.Scheduler;
using DeepSpaceSaga.Server.Services.SessionInfo;

namespace DeepSpaceSaga.Tests.ServerTests;

public class LocalGameServerTests
{
    private readonly LocalGameServer _sut;
    private readonly ISchedulerService _schedulerService;
    private readonly SessionInfoService _sessionInfo;
    private readonly Mock<ISessionContextService> _sessionContextMock;
    private readonly Mock<ISessionContextService> _serverContextMock;
    private readonly Mock<IMetricsService> _gameFlowMetricsMock;
    private readonly Mock<IMetricsService> _serverMetricsMock;
    private readonly TurnSchedulerService _turnSchedulerService;
    private GameSessionDto? _lastExecutedSession;

    public LocalGameServerTests()
    {
        // Initialize the logger
        TestLoggerRepository.Initialize();
        
        // Setup dependencies
        _sessionInfo = new SessionInfoService();
        _turnSchedulerService = new TurnSchedulerService(_sessionInfo);
        _sessionContextMock = new Mock<ISessionContextService>();
        _serverContextMock = new Mock<ISessionContextService>();
        _gameFlowMetricsMock = new Mock<IMetricsService>();
        _serverMetricsMock = new Mock<IMetricsService>();
        _sessionContextMock.Setup(x => x.Metrics).Returns(_gameFlowMetricsMock.Object);
        _serverContextMock.Setup(x => x.Metrics).Returns(_serverMetricsMock.Object);
        _sessionContextMock.Setup(x => x.SessionInfo).Returns(_sessionInfo);
        _schedulerService = new SchedulerService(_sessionContextMock.Object);
        _sut = new LocalGameServer(_schedulerService, _serverContextMock.Object);
        
        // Subscribe to OnTurnExecute event
        _sut.OnTurnExecute += session => _lastExecutedSession = session;
    }

    [Fact]
    public void SessionStart_ShouldStartGameFlow()
    {
        // Act
        _sut.SessionStart(new GameSession());
        
        // Assert
        _gameFlowMetricsMock.Verify(x => x.Add(It.Is<string>(s => s == MetricsServer.SessionStart), 1), Times.Once);
    }

    [Fact]
    public void SessionPause_ShouldPauseGameFlow()
    {
        // Arrange
        _sut.SessionStart(new GameSession());

        // Act
        _sut.SessionPause();

        // Assert
        _gameFlowMetricsMock.Verify(x => x.Add(It.Is<string>(s => s == MetricsServer.SessionPause), 1), Times.Once);
        _sessionInfo.IsPaused.Should().BeTrue();
    }

    [Fact]
    public void SessionResume_ShouldResumeGameFlow()
    {
        // Arrange
        _sut.SessionStart(new GameSession());
        _sut.SessionPause();

        // Act
        _sut.SessionResume();

        // Assert
        _gameFlowMetricsMock.Verify(x => x.Add(It.Is<string>(s => s == MetricsServer.SessionResume), 1), Times.Once);
        _sessionInfo.IsPaused.Should().BeFalse();
    }

    [Fact]
    public void SessionStop_ShouldStopGameFlow()
    {
        // Arrange
        _sut.SessionStart(new GameSession());

        // Act
        _sut.SessionStop();

        // Assert
        _gameFlowMetricsMock.Verify(x => x.Add(It.Is<string>(s => s == MetricsServer.SessionStop), 1), Times.Once);
        _sessionInfo.IsPaused.Should().BeTrue();
    }

    [Fact]
    public void TurnExecution_ShouldIncrementTurnAndNotifySubscribers()
    {
        // Arrange
        var initialTurn = _sessionInfo.Turn;

        // Act
        _sut.TurnExecution(_sessionInfo, CalculationType.Turn);

        // Assert
        _lastExecutedSession.Should().NotBeNull();
        _lastExecutedSession!.Id.Should().NotBe(Guid.Empty);
        _lastExecutedSession.Turn.Should().Be(initialTurn + 1);
    }

    [Theory]
    [InlineData(CalculationType.Tick)]
    [InlineData(CalculationType.Turn)]
    [InlineData(CalculationType.Cycle)]
    public void TurnExecution_DifferentTypes_ShouldIncrementTurn(CalculationType type)
    {
        // Arrange
        var initialTurn = _sessionInfo.Turn;

        // Act
        _sut.TurnExecution(_sessionInfo, type);

        // Assert
        _lastExecutedSession.Should().NotBeNull();
        _lastExecutedSession!.Turn.Should().Be(initialTurn + 1);
    }
    
    [Fact]
    public void GameSessionMap_ShouldCreateCorrectDTO()
    {
        // Arrange & Act
        _sut.TurnExecution(_sessionInfo, CalculationType.Turn);
        
        // Assert
        _lastExecutedSession.Should().NotBeNull();
        _lastExecutedSession!.Id.Should().NotBe(Guid.Empty);
    }
    
    [Fact]
    public void TurnExecution_WithoutSubscribers_ShouldNotThrowException()
    {
        // Arrange
        var localGameServer = new LocalGameServer(_schedulerService, _serverContextMock.Object);
        
        // Act & Assert
        var action = () => localGameServer.TurnExecution(_sessionInfo, CalculationType.Turn);
        action.Should().NotThrow();
    }
    
    [Fact]
    public void Constructor_WithNullDependencies_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var action1 = () => new LocalGameServer(null!, _serverContextMock.Object);
        var action2 = () => new LocalGameServer(_schedulerService, null!);
        
        action1.Should().Throw<ArgumentNullException>().WithParameterName("schedulerService");
        action2.Should().Throw<ArgumentNullException>().WithParameterName("sessionContext");
    }
    
    [Fact]
    public void SessionLifecycle_FullWorkflow_ShouldExecuteCorrectly()
    {
        // Arrange & Act
        _sut.SessionStart(new GameSession());
        _sut.SessionPause();
        _sut.SessionResume();
        _sut.SessionStop();
        
        // Assert
        _gameFlowMetricsMock.Verify(x => x.Add(It.Is<string>(s => s == MetricsServer.SessionStart), 1), Times.Once);
        _gameFlowMetricsMock.Verify(x => x.Add(It.Is<string>(s => s == MetricsServer.SessionPause), 1), Times.Once);
        _gameFlowMetricsMock.Verify(x => x.Add(It.Is<string>(s => s == MetricsServer.SessionResume), 1), Times.Once);
        _gameFlowMetricsMock.Verify(x => x.Add(It.Is<string>(s => s == MetricsServer.SessionStop), 1), Times.Once);
    }
} 