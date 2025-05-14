using DeepSpaceSaga.Server.Services;
using FluentAssertions;

namespace DeepSpaceSaga.Tests.ServerTests;

public class LocalGameServerTests
{
    private readonly LocalGameServer _sut;
    private readonly ISchedulerService _schedulerService;
    private readonly SessionInfoService _sessionInfo;
    private readonly Mock<ISessionContext> _sessionContextMock;
    private readonly Mock<ISessionContext> _serverContextMock;
    private readonly Mock<IMetricsService> _gameFlowMetricsMock;
    private readonly Mock<IMetricsService> _serverMetricsMock;
    private readonly TurnSchedulerService _turnSchedulerService;
    private GameSessionDTO? _lastExecutedSession;

    public LocalGameServerTests()
    {
        // Initialize the logger
        TestLoggerRepository.Initialize();
        
        // Setup dependencies
        _sessionInfo = new SessionInfoService();
        _turnSchedulerService = new TurnSchedulerService(_sessionInfo);
        _sessionContextMock = new Mock<ISessionContext>();
        _serverContextMock = new Mock<ISessionContext>();
        _gameFlowMetricsMock = new Mock<IMetricsService>();
        _serverMetricsMock = new Mock<IMetricsService>();
        _sessionContextMock.Setup(x => x.Metrics).Returns(_gameFlowMetricsMock.Object);
        _serverContextMock.Setup(x => x.Metrics).Returns(_serverMetricsMock.Object);
        _schedulerService = new SchedulerService(_sessionContextMock.Object);
        _sut = new LocalGameServer(_schedulerService, _serverContextMock.Object);
        
        // Subscribe to OnTurnExecute event
        _sut.OnTurnExecute += session => _lastExecutedSession = session;
    }

    [Fact]
    public void SessionPause_ShouldPauseGameFlow()
    {
        // Arrange
        _sut.SessionStart();

        // Act
        _sut.SessionPause();

        // Assert
        _gameFlowMetricsMock.Verify(x => x.Add(It.Is<string>(s => s == MetricsServer.SessionPause), 1), Times.Once);
        Assert.True(_sessionInfo.IsPaused);
    }

    [Fact]
    public void SessionResume_ShouldResumeGameFlow()
    {
        // Arrange
        _sut.SessionStart();
        _sut.SessionPause();

        // Act
        _sut.SessionResume();

        // Assert
        _gameFlowMetricsMock.Verify(x => x.Add(It.Is<string>(s => s == MetricsServer.SessionResume), 1), Times.Once);
        Assert.False(_sessionInfo.IsPaused);
    }

    [Fact]
    public void SessionStop_ShouldStopGameFlow()
    {
        // Arrange
        _sut.SessionStart();

        // Act
        _sut.SessionStop();

        // Assert
        _gameFlowMetricsMock.Verify(x => x.Add(It.Is<string>(s => s == MetricsServer.SessionStop), 1), Times.Once);
        Assert.True(_sessionInfo.IsPaused);
    }

    [Fact]
    public void TurnExecution_ShouldIncrementTurnAndNotifySubscribers()
    {
        // Arrange
        var initialTurn = _sessionInfo.Turn;

        // Act
        _sut.TurnExecution(_sessionInfo, CalculationType.Turn);

        // Assert
        Assert.NotNull(_lastExecutedSession);
        Assert.NotEqual(Guid.Empty, _lastExecutedSession.Id);
        Assert.NotEqual(string.Empty, _lastExecutedSession.FlowState);
        Assert.Equal(initialTurn + 1, _lastExecutedSession.Turn);
        Assert.NotNull(_lastExecutedSession.SpaceMap);
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
        Assert.NotNull(_lastExecutedSession);
        Assert.Equal(initialTurn + 1, _lastExecutedSession.Turn);
    }
} 