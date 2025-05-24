namespace DeepSpaceSaga.Tests.ServerTests;

public class LocalGameServerTests
{
    private readonly LocalGameServer _sut;
    private readonly ISchedulerService _schedulerService;
    private readonly ISessionContextService _sessionContext;
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
        var sessionInfo = new SessionInfoService();
        _gameFlowMetricsMock = new Mock<IMetricsService>();
        _serverMetricsMock = new Mock<IMetricsService>();
        _sessionContext = new SessionContextService(sessionInfo, _gameFlowMetricsMock.Object);
        _serverContextMock = new Mock<ISessionContextService>();
        _gameSession = new GameSession();
        
        _serverContextMock.Setup(x => x.Metrics).Returns(_serverMetricsMock.Object);
        _serverContextMock.SetupSet(x => x.GameSession = It.IsAny<GameSession>());
        _serverContextMock.Setup(x => x.GameSession).Returns(() => _gameSession);
        _serverContextMock.Setup(x => x.SessionInfo).Returns(sessionInfo);
        
        _schedulerService = new SchedulerService(_sessionContext);
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
        _sessionContext.SessionInfo.IsPaused.Should().BeTrue();
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
        _sessionContext.SessionInfo.IsPaused.Should().BeFalse();
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
        _sessionContext.SessionInfo.IsPaused.Should().BeTrue();
    }

    [Fact]
    public void TurnExecution_ShouldIncrementTurnAndNotifySubscribers()
    {
        // Arrange
        var initialTurn = _sessionContext.SessionInfo.Turn;
        var session = new GameSession();
        _sut.SessionStart(session);

        // Act
        _sut.TurnExecution(_sessionContext.SessionInfo, CalculationType.Turn);

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
        var initialTurn = _sessionContext.SessionInfo.Turn;
        var session = new GameSession();
        _sut.SessionStart(session);

        // Act
        _sut.TurnExecution(_sessionContext.SessionInfo, type);

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
        _sut.TurnExecution(_sessionContext.SessionInfo, CalculationType.Turn);
        
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
        var sessionInfo = new SessionInfoService();
    
        serverContextMock.Setup(x => x.Metrics).Returns(_serverMetricsMock.Object);
        serverContextMock.SetupSet(x => x.GameSession = It.IsAny<GameSession>());
        serverContextMock.Setup(x => x.GameSession).Returns(session);
        serverContextMock.Setup(x => x.SessionInfo).Returns(sessionInfo);
        
        var localGameServer = new LocalGameServer(_schedulerService, serverContextMock.Object);
        localGameServer.SessionStart(session);
        
        // Act & Assert
        var action = () => localGameServer.TurnExecution(_sessionContext.SessionInfo, CalculationType.Turn);
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

    [Fact]
    public void AddCommand_ShouldAddCommandToGameSession()
    {
        // Arrange
        var session = new GameSession();
        var command = new Command();
        _sut.SessionStart(session);

        // Act
        _sut.AddCommand(command);

        // Assert
        _gameSession.Commands.Should().ContainKey(command.CommandId);
        _gameSession.Commands[command.CommandId].Should().Be(command);
    }

    [Fact]
    public void RemoveCommand_ShouldRemoveCommandFromGameSession()
    {
        // Arrange
        var session = new GameSession();
        var command = new Command();
        _sut.SessionStart(session);
        _sut.AddCommand(command);

        // Act
        _sut.RemoveCommand(command.CommandId);

        // Assert
        _gameSession.Commands.Should().NotContainKey(command.CommandId);
    }

    [Fact]
    public void GetSessionContextDto_ShouldReturnCurrentGameSessionDto()
    {
        // Arrange
        var session = new GameSession();
        _sut.SessionStart(session);
        _sut.TurnExecution(_sessionContext.SessionInfo, CalculationType.Turn);

        // Act
        var result = _sut.GetSessionContextDto();

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(_lastExecutedSession);
    }

    [Fact]
    public void AddCommand_ShouldTriggerGameSessionChanged()
    {
        // Arrange
        var session = new GameSession();
        var command = new Command();
        _sut.SessionStart(session);
        var changeTriggered = false;
        _gameSession.Changed += (s, e) => changeTriggered = true;

        // Act
        _sut.AddCommand(command);

        // Assert
        changeTriggered.Should().BeTrue();
    }

    [Fact]
    public void RemoveCommand_ShouldTriggerGameSessionChanged()
    {
        // Arrange
        var session = new GameSession();
        var command = new Command();
        _sut.SessionStart(session);
        _sut.AddCommand(command);
        var changeTriggered = false;
        _gameSession.Changed += (s, e) => changeTriggered = true;

        // Act
        _sut.RemoveCommand(command.CommandId);

        // Assert
        changeTriggered.Should().BeTrue();
    }
} 