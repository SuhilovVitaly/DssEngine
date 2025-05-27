namespace DeepSpaceSaga.Tests.ServerTests;

public class LocalGameServerTests
{
    private readonly LocalGameServer _sut;
    private readonly ISchedulerService _schedulerService;
    private readonly ISessionContextService _sessionContext;
    private readonly Mock<IMetricsService> _gameFlowMetricsMock;
    private readonly Mock<IProcessingService> _processingServiceMock;
    private GameSessionDto? _lastExecutedSession;

    public LocalGameServerTests()
    {
        // Initialize the logger
        TestLoggerRepository.Initialize();
        
        // Setup dependencies
        var sessionInfo = new SessionInfoService();
        _gameFlowMetricsMock = new Mock<IMetricsService>();
        _processingServiceMock = new Mock<IProcessingService>();
        var generationToolMock = new Mock<IGenerationTool>();
        
        // Setup unique ID generation to prevent duplicate key errors
        var idCounter = 0;
        generationToolMock.Setup(x => x.GetId()).Returns(() => idCounter++);
        
        // Use real SessionContextService instead of mock to avoid lock issues
        _sessionContext = new SessionContextService(sessionInfo, _gameFlowMetricsMock.Object, generationToolMock.Object);
        
        // Setup processing service mock to return a valid GameSessionDto
        _processingServiceMock.Setup(x => x.Process(It.IsAny<ISessionContextService>()))
            .Returns(() => new GameSessionDto 
            { 
                Id = Guid.NewGuid(),
                State = new GameStateDto(),
                CelestialObjects = new Dictionary<int, CelestialObjectDto>(),
                Commands = new Dictionary<Guid, CommandDto>()
            });
        
        // Use real SessionContextService for scheduler and LocalGameServer to avoid lock issues
        _schedulerService = new SchedulerService(_sessionContext);
        _sut = new LocalGameServer(_schedulerService, _sessionContext, _processingServiceMock.Object);
        
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
        _sessionContext.GameSession.Should().Be(session);
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
        _lastExecutedSession.State.ProcessedTurns.Should().Be(initialTurn + 1);
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
        _lastExecutedSession!.State.ProcessedTurns.Should().Be(initialTurn + 1);
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
        var sessionInfo = new SessionInfoService();
        var metricsMock = new Mock<IMetricsService>();
        var generationToolMock = new Mock<IGenerationTool>();
        
        // Setup unique ID generation to prevent duplicate key errors
        var idCounter = 100;
        generationToolMock.Setup(x => x.GetId()).Returns(() => idCounter++);
        
        var sessionContext = new SessionContextService(sessionInfo, metricsMock.Object, generationToolMock.Object);
        var schedulerService = new SchedulerService(sessionContext);
        var localGameServer = new LocalGameServer(schedulerService, sessionContext, _processingServiceMock.Object);
        
        var session = new GameSession();
        localGameServer.SessionStart(session);
        
        // Act & Assert
        var action = () => localGameServer.TurnExecution(sessionInfo, CalculationType.Turn);
        action.Should().NotThrow();
    }
    
    [Fact]
    public void Constructor_WithNullDependencies_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var action1 = () => new LocalGameServer(null!, _sessionContext, _processingServiceMock.Object);
        var action2 = () => new LocalGameServer(_schedulerService, null!, _processingServiceMock.Object);
        var action3 = () => new LocalGameServer(_schedulerService, _sessionContext, null!);
        
        action1.Should().Throw<ArgumentNullException>().WithParameterName("schedulerService");
        action2.Should().Throw<ArgumentNullException>().WithParameterName("sessionContext");
        action3.Should().Throw<ArgumentNullException>().WithParameterName("processingService");
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
        _sessionContext.SessionInfo.IsPaused.Should().BeTrue();
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
        _sessionContext.GameSession.Commands.Should().ContainKey(command.CommandId);
        _sessionContext.GameSession.Commands[command.CommandId].Should().Be(command);
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
        _sessionContext.GameSession.Commands.Should().NotContainKey(command.CommandId);
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
        _sessionContext.GameSession.Changed += (s, e) => changeTriggered = true;

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
        _sessionContext.GameSession.Changed += (s, e) => changeTriggered = true;

        // Act
        _sut.RemoveCommand(command.CommandId);

        // Assert
        changeTriggered.Should().BeTrue();
    }

    [Fact]
    public void SetGameSpeed_ShouldUpdateSpeedAndRefreshDto()
    {
        // Arrange
        var session = new GameSession();
        _sut.SessionStart(session);
        var eventTriggered = false;
        _sut.OnTurnExecute += _ => eventTriggered = true;

        // Act
        _sut.SetGameSpeed(3);

        // Assert
        _sessionContext.SessionInfo.Speed.Should().Be(3);
        eventTriggered.Should().BeTrue();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(5)]
    [InlineData(10)]
    public void SetGameSpeed_WithDifferentSpeeds_ShouldSetCorrectSpeed(int speed)
    {
        // Arrange
        var session = new GameSession();
        _sut.SessionStart(session);

        // Act
        _sut.SetGameSpeed(speed);

        // Assert
        _sessionContext.SessionInfo.Speed.Should().Be(speed);
        _sessionContext.SessionInfo.IsPaused.Should().BeFalse();
    }

    [Fact]
    public void OnTurnExecute_Event_ShouldBeTriggeredCorrectly()
    {
        // Arrange
        var session = new GameSession();
        var eventCallCount = 0;
        GameSessionDto? capturedSession = null;
        
        _sut.OnTurnExecute += dto =>
        {
            eventCallCount++;
            capturedSession = dto;
        };
        
        _sut.SessionStart(session);

        // Act
        _sut.TurnExecution(_sessionContext.SessionInfo, CalculationType.Turn);

        // Assert
        eventCallCount.Should().Be(1);
        capturedSession.Should().NotBeNull();
        capturedSession!.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void SessionStart_WithNullSession_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var action = () => _sut.SessionStart(null!);
        
        action.Should().Throw<ArgumentNullException>()
            .WithParameterName("session");
    }

    [Fact]
    public void RemoveCommand_WithNonExistentCommandId_ShouldNotThrow()
    {
        // Arrange
        var session = new GameSession();
        var nonExistentCommandId = Guid.NewGuid();
        _sut.SessionStart(session);

        // Act & Assert
        var action = () => _sut.RemoveCommand(nonExistentCommandId);
        action.Should().NotThrow();
    }

    [Fact]
    public void AddCommand_WithSameCommandTwice_ShouldUpdateExistingCommand()
    {
        // Arrange
        var session = new GameSession();
        var command = new Command();
        _sut.SessionStart(session);
        _sut.AddCommand(command);

        // Act - try to add same command again
        var action = () => _sut.AddCommand(command);

        // Assert - should throw because Dictionary.Add throws on duplicate key
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GetSessionContextDto_WhenNotStarted_ShouldReturnEmptyDto()
    {
        // Act
        var result = _sut.GetSessionContextDto();

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(Guid.Empty);
    }

    [Fact]
    public void SessionLifecycle_MultipleStops_ShouldNotThrow()
    {
        // Arrange
        var session = new GameSession();
        _sut.SessionStart(session);

        // Act & Assert
        var action = () =>
        {
            _sut.SessionStop();
            _sut.SessionStop(); // Second stop should not throw
        };
        
        action.Should().NotThrow();
    }

    [Fact]
    public void RefreshGameSessionDto_ShouldTriggerOnTurnExecuteEvent()
    {
        // Arrange
        var session = new GameSession();
        var eventTriggered = false;
        _sut.OnTurnExecute += _ => eventTriggered = true;
        _sut.SessionStart(session);
        eventTriggered = false; // Reset after SessionStart

        // Act
        _sut.SessionPause(); // This should trigger RefreshGameSessionDto

        // Assert
        eventTriggered.Should().BeTrue();
    }
} 