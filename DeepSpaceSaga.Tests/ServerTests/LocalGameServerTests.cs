using DeepSpaceSaga.Common.Abstractions.Entities;
using DeepSpaceSaga.Common.Abstractions.Services;
using DeepSpaceSaga.Server.Services;
using DeepSpaceSaga.Server.Services.SessionContext;
using FluentAssertions;
using Moq;

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
    private GameSession _gameSession;

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
        _gameSession = new GameSession();
        
        _sessionContextMock.Setup(x => x.Metrics).Returns(_gameFlowMetricsMock.Object);
        _sessionContextMock.Setup(x => x.SessionInfo).Returns(_sessionInfo);
        _sessionContextMock.SetupSet(x => x.GameSession = It.IsAny<GameSession>());
        _sessionContextMock.Setup(x => x.GameSession).Returns(_gameSession);
        
        _serverContextMock.Setup(x => x.Metrics).Returns(_serverMetricsMock.Object);
        _serverContextMock.SetupSet(x => x.GameSession = It.IsAny<GameSession>());
        _serverContextMock.Setup(x => x.GameSession).Returns(() => _gameSession);
        
        _schedulerService = new SchedulerService(_sessionContextMock.Object);
        _sut = new LocalGameServer(_schedulerService, _serverContextMock.Object);
        
        // Subscribe to OnTurnExecute event
        _sut.OnTurnExecute += session => _lastExecutedSession = session;
    }

    [Fact]
    public void SessionStart_ShouldStartGameFlow()
    {
        // Arrange
        var session = new GameSession();
        
        // Act
        _sut.SessionStart(session);
        
        // Assert
        _gameFlowMetricsMock.Verify(x => x.Add(It.Is<string>(s => s == MetricsServer.SessionStart), 1), Times.Once);
        _serverContextMock.VerifySet(x => x.GameSession = session);
    }

    [Fact]
    public void SessionPause_ShouldPauseGameFlow()
    {
        // Arrange
        var session = new GameSession();
        _sut.SessionStart(session);

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
        var session = new GameSession();
        _sut.SessionStart(session);
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
        var session = new GameSession();
        _sut.SessionStart(session);

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
        var session = new GameSession();
        _sut.SessionStart(session);

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
        var session = new GameSession();
        _sut.SessionStart(session);

        // Act
        _sut.TurnExecution(_sessionInfo, type);

        // Assert
        _lastExecutedSession.Should().NotBeNull();
        _lastExecutedSession!.Turn.Should().Be(initialTurn + 1);
    }
    
    [Fact]
    public void GameSessionMap_ShouldCreateCorrectDTO()
    {
        // Arrange
        var session = new GameSession();
        _sut.SessionStart(session);

        // Act
        _sut.TurnExecution(_sessionInfo, CalculationType.Turn);
        
        // Assert
        _lastExecutedSession.Should().NotBeNull();
        _lastExecutedSession!.Id.Should().NotBe(Guid.Empty);
    }
    
    [Fact]
    public void TurnExecution_WithoutSubscribers_ShouldNotThrowException()
    {
        // Arrange
        var serverContextMock = new Mock<ISessionContextService>();
        var session = new GameSession();
        serverContextMock.Setup(x => x.Metrics).Returns(_serverMetricsMock.Object);
        serverContextMock.SetupSet(x => x.GameSession = It.IsAny<GameSession>());
        serverContextMock.Setup(x => x.GameSession).Returns(session);
        var localGameServer = new LocalGameServer(_schedulerService, serverContextMock.Object);
        localGameServer.SessionStart(session);
        
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
        var session = new GameSession();
        _sut.SessionStart(session);
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