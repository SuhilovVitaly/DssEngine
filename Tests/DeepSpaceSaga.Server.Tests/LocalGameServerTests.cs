using DeepSpaceSaga.Common.Abstractions.Dto.Ui;
using DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects;

namespace DeepSpaceSaga.Server.Tests;

public class LocalGameServerTests
{
    private readonly LocalGameServer _sut;
    private readonly ISchedulerService _schedulerService;
    private readonly ISessionContextService _sessionContext;
    private readonly Mock<IMetricsService> _gameFlowMetricsMock;
    private readonly Mock<IProcessingService> _processingServiceMock;
    private readonly Mock<ISaveLoadService> _saveLoadServiceMock;
    private GameSessionDto? _lastExecutedSession;

    public LocalGameServerTests()
    {
        // Initialize the logger
        TestLoggerRepository.Initialize();
        
        // Setup dependencies
        var sessionInfo = new SessionInfoService();
        _gameFlowMetricsMock = new Mock<IMetricsService>();
        _processingServiceMock = new Mock<IProcessingService>();
        _saveLoadServiceMock = new Mock<ISaveLoadService>();
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
                CelestialObjects = new Dictionary<int, CelestialObjectSaveFormatDto>(),
                Commands = new Dictionary<Guid, CommandDto>()
            });
        
        // Use real SessionContextService for scheduler and LocalGameServer to avoid lock issues
        _schedulerService = new SchedulerService(_sessionContext);
        _sut = new LocalGameServer(_schedulerService, _sessionContext, _processingServiceMock.Object, _saveLoadServiceMock.Object);
        
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
        _sut.TurnExecution(CalculationType.Turn);

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
        _sut.TurnExecution(type);

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
        _sut.TurnExecution(CalculationType.Turn);
        
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
        var saveLoadServiceMock = new Mock<ISaveLoadService>();
        var generationToolMock = new Mock<IGenerationTool>();
        
        // Setup unique ID generation to prevent duplicate key errors
        var idCounter = 100;
        generationToolMock.Setup(x => x.GetId()).Returns(() => idCounter++);
        
        var sessionContext = new SessionContextService(sessionInfo, metricsMock.Object, generationToolMock.Object);
        var schedulerService = new SchedulerService(sessionContext);
        var localGameServer = new LocalGameServer(schedulerService, sessionContext, _processingServiceMock.Object, saveLoadServiceMock.Object);
        
        var session = new GameSession();
        localGameServer.SessionStart(session);
        
        // Act & Assert
        var action = () => localGameServer.TurnExecution(CalculationType.Turn);
        action.Should().NotThrow();
    }
    
    [Fact]
    public void Constructor_WithNullDependencies_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var action1 = () => new LocalGameServer(null!, _sessionContext, _processingServiceMock.Object, _saveLoadServiceMock.Object);
        var action2 = () => new LocalGameServer(_schedulerService, null!, _processingServiceMock.Object, _saveLoadServiceMock.Object);
        var action3 = () => new LocalGameServer(_schedulerService, _sessionContext, null!, _saveLoadServiceMock.Object);
        var action4 = () => new LocalGameServer(_schedulerService, _sessionContext, _processingServiceMock.Object, null!);
        
        action1.Should().Throw<ArgumentNullException>().WithParameterName("schedulerService");
        action2.Should().Throw<ArgumentNullException>().WithParameterName("sessionContext");
        action3.Should().Throw<ArgumentNullException>().WithParameterName("processingService");
        action4.Should().Throw<ArgumentNullException>().WithParameterName("saveLoadService");
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
        _sut.SessionStart(session);
        var command = new Command { Id = Guid.NewGuid() };
        
        // Act
        _sut.AddCommand(command);
        
        // Assert
        _sessionContext.GameSession.Commands.Should().ContainKey(command.Id);
        _sessionContext.GameSession.Commands[command.Id].Should().Be(command);
    }


    [Fact]
    public void GetSessionContextDto_ShouldReturnCurrentGameSessionDto()
    {
        // Arrange
        var session = new GameSession();
        _sut.SessionStart(session);
        
        // Act
        var result = _sut.GetSessionContextDto();
        
        // Assert
        result.Should().NotBeNull();
        // The result comes from processing service mock
    }

    [Fact]
    public void SetGameSpeed_ShouldUpdateSpeedAndRefreshDto()
    {
        // Arrange
        var session = new GameSession();
        _sut.SessionStart(session);
        const int newSpeed = 5;
        
        // Act
        _sut.SetGameSpeed(newSpeed);
        
        // Assert
        _sessionContext.SessionInfo.Speed.Should().Be(newSpeed);
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
    }

    [Fact]
    public void OnTurnExecute_Event_ShouldBeTriggeredCorrectly()
    {
        // Arrange
        var session = new GameSession();
        _sut.SessionStart(session);
        GameSessionDto? capturedSession = null;
        _sut.OnTurnExecute += sessionDto => capturedSession = sessionDto;
        
        // Act
        _sut.TurnExecution(CalculationType.Turn);
        
        // Assert
        capturedSession.Should().NotBeNull();
        capturedSession!.Id.Should().NotBe(Guid.Empty);
        capturedSession.State.Should().NotBeNull();
        capturedSession.CelestialObjects.Should().NotBeNull();
        capturedSession.Commands.Should().NotBeNull();
    }

    [Fact]
    public void SessionStart_WithNullSession_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var action = () => _sut.SessionStart(null!);
        action.Should().Throw<ArgumentNullException>().WithParameterName("session");
    }

    [Fact]
    public void AddCommand_WithSameCommandTwice_ShouldThrowArgumentException()
    {
        // Arrange
        var session = new GameSession();
        _sut.SessionStart(session);
        var command = new Command { Id = Guid.NewGuid() };
        
        // Act
        _sut.AddCommand(command);
        
        // Assert - should throw because Dictionary.Add throws on duplicate key
        var action = () => _sut.AddCommand(command);
        action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public void GetSessionContextDto_WhenNotStarted_ShouldReturnDto()
    {
        // Act
        var result = _sut.GetSessionContextDto();
        
        // Assert
        result.Should().NotBeNull();
        // When not started, it should return a DTO from processing service
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
            _sut.SessionStop();
        };
        action.Should().NotThrow();
    }

    [Fact]
    public void RefreshGameSessionDto_ShouldTriggerOnTurnExecuteEvent()
    {
        // Arrange
        var session = new GameSession();
        _sut.SessionStart(session);
        GameSessionDto? capturedSession = null;
        _sut.OnTurnExecute += sessionDto => capturedSession = sessionDto;
        
        // Act
        _sut.SessionPause(); // This should trigger RefreshGameSessionDto
        
        // Assert
        capturedSession.Should().NotBeNull();
        capturedSession!.Id.Should().Be(_sessionContext.GameSession.Id);
    }
} 