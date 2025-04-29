namespace DeepSpaceSaga.Tests.ControllerTests;

public class WorkerServiceTests
{
    private readonly Mock<Executor> _mockExecutor;
    private readonly WorkerService _workerService;

    public WorkerServiceTests()
    {
        _mockExecutor = new Mock<Executor>(32); // Pass tickInterval to constructor
        _workerService = new WorkerService(_mockExecutor.Object);
    }

    [Fact]
    public void StartProcessing_WhenNotRunning_StartsExecutor()
    {
        // Arrange
        _mockExecutor.Setup(x => x.Start(It.IsAny<Action<ExecutorState, CalculationType>>()));

        // Act
        _workerService.StartProcessing();

        // Assert
        _mockExecutor.Verify(x => x.Start(It.IsAny<Action<ExecutorState, CalculationType>>()),
            Times.Once());
    }

    [Fact]
    public void StartProcessing_WhenAlreadyRunning_DoesNotStartExecutorAgain()
    {
        // Arrange
        _mockExecutor.Setup(x => x.Start(It.IsAny<Action<ExecutorState, CalculationType>>()));
        _workerService.StartProcessing(); // First start

        // Act
        _workerService.StartProcessing(); // Second start

        // Assert
        _mockExecutor.Verify(x => x.Start(It.IsAny<Action<ExecutorState, CalculationType>>()),
            Times.Once());
    }

    [Fact]
    public async Task StopProcessing_WhenRunning_StopsProcessing()
    {
        // Arrange
        _mockExecutor.Setup(x => x.Start(It.IsAny<Action<ExecutorState, CalculationType>>()));
        _workerService.StartProcessing();

        // Act
        await _workerService.StopProcessing();

        // Assert
        // Start new processing to verify internal state was reset
        _workerService.StartProcessing();
        _mockExecutor.Verify(x => x.Start(It.IsAny<Action<ExecutorState, CalculationType>>()),
            Times.Exactly(2));
    }

    [Fact]
    public async Task StopProcessing_WhenNotRunning_DoesNothing()
    {
        // Act
        await _workerService.StopProcessing();

        // Assert - should not throw any exceptions
        Assert.True(true);
    }

    [Fact]
    public void Calculation_RaisesEventWithCorrectData()
    {
        // Arrange
        GameSessionDTO? receivedSession = null;
        string? receivedState = null;
        _workerService.OnGetDataFromServer += (state, session) =>
        {
            receivedState = state;
            receivedSession = session;
        };

        Action<ExecutorState, CalculationType> calculationAction = null!;
        _mockExecutor.Setup(x => x.Start(It.IsAny<Action<ExecutorState, CalculationType>>()))
            .Callback<Action<ExecutorState, CalculationType>>(action => calculationAction = action);

        // Act
        _workerService.StartProcessing();
        var state = new ExecutorState();
        calculationAction(state, CalculationType.Tick);

        // Assert
        Assert.NotNull(receivedSession);
        Assert.Equal(state.ToString(), receivedState);
        Assert.Equal(1, receivedSession.Turn);
    }

    [Fact]
    public void Dispose_CallsStopProcessing()
    {
        // Arrange
        _mockExecutor.Setup(x => x.Start(It.IsAny<Action<ExecutorState, CalculationType>>()));
        _workerService.StartProcessing();

        // Act
        _workerService.Dispose();

        // Assert
        // Try to start again to verify internal state was reset
        _workerService.StartProcessing();
        _mockExecutor.Verify(x => x.Start(It.IsAny<Action<ExecutorState, CalculationType>>()),
            Times.Exactly(2));
    }
} 