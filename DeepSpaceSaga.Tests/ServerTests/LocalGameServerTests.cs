using DeepSpaceSaga.Common.Implementation.Services;
using DeepSpaceSaga.Common.Implementation.GameLoop;
using DeepSpaceSaga.Server;
using DeepSpaceSaga.Common.Abstractions.Session.Entities;
using Xunit;

namespace DeepSpaceSaga.Tests.ServerTests;

public class LocalGameServerTests
{
    private readonly LocalGameServer _sut;
    private readonly IGameFlowService _gameFlowService;
    private readonly SessionInfo _sessionInfo;
    private readonly Executor _executor;
    private GameSessionDTO? _lastExecutedSession;

    public LocalGameServerTests()
    {
        // Initialize the logger
        TestLoggerRepository.Initialize();
        
        // Setup dependencies
        _sessionInfo = new SessionInfo();
        _executor = new Executor(_sessionInfo);
        _gameFlowService = new GameFlowService(_sessionInfo, _executor);
        _sut = new LocalGameServer(_gameFlowService);
        
        // Subscribe to OnTurnExecute event
        _sut.OnTurnExecute += session => _lastExecutedSession = session;
    }

    [Fact]
    public void SessionStart_ShouldStartGameFlow()
    {
        // Act
        _sut.SessionStart();

        // Assert
        Assert.Equal(SessionState.NotStarted, _sessionInfo.State);
        Assert.False(_sessionInfo.IsPaused);
    }

    [Fact]
    public void SessionPause_ShouldPauseGameFlow()
    {
        // Arrange
        _sut.SessionStart();

        // Act
        _sut.SessionPause();

        // Assert
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