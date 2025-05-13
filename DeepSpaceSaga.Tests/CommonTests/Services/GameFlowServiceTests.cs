namespace DeepSpaceSaga.Tests.CommonTests.Services;

public class GameFlowServiceTests
{
    private readonly Mock<ISessionInfoService> _sessionInfoMock;
    private readonly Mock<IExecutor> _executorMock;
    private readonly GameFlowService _sut;

    public GameFlowServiceTests()
    {
        _sessionInfoMock = new Mock<ISessionInfoService>();
        _executorMock = new Mock<IExecutor>();
        _sut = new GameFlowService(_sessionInfoMock.Object, _executorMock.Object);
    }

    [Fact]
    public void Constructor_InitializesCorrectly()
    {
        // Assert
        Assert.Equal(_sessionInfoMock.Object, _sut.SessionInfo);
    }

    [Fact]
    public void SessionStart_CallsExecutorStart()
    {
        // Act
        _sut.SessionStart();

        // Assert
        _executorMock.Verify(x => x.Start(It.IsAny<Action<ISessionInfoService, CalculationType>>()), Times.Once);
    }

    [Fact]
    public void SessionPause_CallsExecutorStop()
    {
        // Act
        _sut.SessionPause();

        // Assert
        _executorMock.Verify(x => x.Stop(), Times.Once);
    }

    [Fact]
    public void SessionResume_CallsExecutorResume()
    {
        // Act
        _sut.SessionResume();

        // Assert
        _executorMock.Verify(x => x.Resume(), Times.Once);
    }

    [Fact]
    public void SessionStop_CallsExecutorStop()
    {
        // Act
        _sut.SessionStop();

        // Assert
        _executorMock.Verify(x => x.Stop(), Times.Once);
    }

    [Fact]
    public void TurnExecution_CanBeSetAndExecuted()
    {
        // Arrange
        var wasExecuted = false;
        _sut.TurnExecution = (_, _) => wasExecuted = true;

        // Act
        _sut.TurnExecution(_sessionInfoMock.Object, CalculationType.Turn);

        // Assert
        Assert.True(wasExecuted);
    }

    [Fact]
    public void SessionStart_WithNullTurnExecution_StartsExecutorWithoutException()
    {
        // Arrange
        _sut.TurnExecution = null;

        // Act & Assert
        var exception = Record.Exception(() => _sut.SessionStart());
        Assert.Null(exception);
    }

    [Fact]
    public void SessionStart_PassesCorrectDelegateToExecutor()
    {
        // Arrange
        Action<ISessionInfoService, CalculationType> capturedDelegate = null;
        _executorMock.Setup(x => x.Start(It.IsAny<Action<ISessionInfoService, CalculationType>>()))
            .Callback<Action<ISessionInfoService, CalculationType>>(d => capturedDelegate = d);

        var wasExecuted = false;
        _sut.TurnExecution = (_, _) => wasExecuted = true;

        // Act
        _sut.SessionStart();
        capturedDelegate?.Invoke(_sessionInfoMock.Object, CalculationType.Turn);

        // Assert
        Assert.True(wasExecuted);
    }
} 