namespace DeepSpaceSaga.Tests.CommonTests.Services;

public class GameFlowServiceTests
{
    private readonly Mock<ISessionInfoService> _sessionInfoMock;
    private readonly Mock<ITurnSchedulerService> _executorMock;
    private readonly Mock<ISessionContext> _sessionContextMock;
    private readonly Mock<IMetricsService> _metricsMock;
    private readonly GameFlowService _sut;

    public GameFlowServiceTests()
    {
        _sessionInfoMock = new Mock<ISessionInfoService>();
        _executorMock = new Mock<ITurnSchedulerService>();
        _sessionContextMock = new Mock<ISessionContext>();
        _metricsMock = new Mock<IMetricsService>();
        _sessionContextMock.Setup(x => x.Metrics).Returns(_metricsMock.Object);
        _sut = new GameFlowService(_sessionInfoMock.Object, _executorMock.Object, _sessionContextMock.Object);
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
        // Arrange
        _sut.TurnExecution = (_, _) => { };

        // Act
        _sut.SessionStart();

        // Assert
        _executorMock.Verify(x => x.Start(It.IsAny<Action<ISessionInfoService, CalculationType>>()), Times.Once);
        _metricsMock.Verify(x => x.Add(It.Is<string>(s => s == DeepSpaceSaga.Server.MetricsServer.SessionStart), 1), Times.Once);
    }

    [Fact]
    public void SessionPause_CallsExecutorStop()
    {
        // Arrange
        _sut.TurnExecution = (_, _) => { };
        _sut.SessionStart();

        // Act
        _sut.SessionPause();

        // Assert
        _executorMock.Verify(x => x.Stop(), Times.Once);
        _metricsMock.Verify(x => x.Add(It.Is<string>(s => s == DeepSpaceSaga.Server.MetricsServer.SessionPause), 1), Times.Once);
    }

    [Fact]
    public void SessionResume_CallsExecutorResume()
    {
        // Arrange
        _sut.TurnExecution = (_, _) => { };
        _sut.SessionStart();
        _sut.SessionPause();

        // Act
        _sut.SessionResume();

        // Assert
        _executorMock.Verify(x => x.Resume(), Times.Once);
        _metricsMock.Verify(x => x.Add(It.Is<string>(s => s == DeepSpaceSaga.Server.MetricsServer.SessionResume), 1), Times.Once);
    }

    [Fact]
    public void SessionStop_CallsExecutorStop()
    {
        // Arrange
        _sut.TurnExecution = (_, _) => { };
        _sut.SessionStart();

        // Act
        _sut.SessionStop();

        // Assert
        _executorMock.Verify(x => x.Stop(), Times.Once);
        _metricsMock.Verify(x => x.Add(It.Is<string>(s => s == DeepSpaceSaga.Server.MetricsServer.SessionStop), 1), Times.Once);
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
    public void SessionStart_WithNullTurnExecution_ThrowsInvalidOperationException()
    {
        // Arrange
        _sut.TurnExecution = null;

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => _sut.SessionStart());
        Assert.Equal("TurnExecution delegate must be set before starting the game flow", exception.Message);
    }

    [Fact]
    public void SessionStart_PassesCorrectTurnExecutionToExecutor()
    {
        // Arrange
        Action<ISessionInfoService, CalculationType> capturedDelegate = null;
        _executorMock.Setup(x => x.Start(It.IsAny<Action<ISessionInfoService, CalculationType>>()))
            .Callback<Action<ISessionInfoService, CalculationType>>(d => capturedDelegate = d);

        var turnExecutionCalled = false;
        _sut.TurnExecution = (_, _) => turnExecutionCalled = true;

        // Act
        _sut.SessionStart();

        // Assert
        Assert.NotNull(capturedDelegate);
        capturedDelegate(_sessionInfoMock.Object, CalculationType.Turn);
        Assert.True(turnExecutionCalled);
    }
} 