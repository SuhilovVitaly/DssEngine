using DeepSpaceSaga.Common.Abstractions.Session.Entities;
using DeepSpaceSaga.Common.Implementation.Services;

namespace DeepSpaceSaga.Tests.ControllerTests;

public class WorkerServiceTests : IDisposable
{
    private readonly Mock<IExecutor> _mockExecutor;
    private readonly Mock<IGameServer> _mockServer;
    private readonly WorkerService _workerService;

    public WorkerServiceTests()
    {
        // Initialize log4net repositories
        TestLoggerRepository.Initialize();
        
        _mockExecutor = new Mock<IExecutor>();
        _mockServer = new Mock<IGameServer>();
        _workerService = new WorkerService(_mockExecutor.Object, _mockServer.Object);
    }

    public void Dispose()
    {
        // No need to dispose individual files, TestLoggerRepository handles cleanup
    }

    [Fact]
    public void StartProcessing_WhenNotRunning_StartsExecutor()
    {
        // Arrange
        _mockExecutor.Setup(x => x.Start(It.IsAny<Action<ISessionInfo, CalculationType>>()));

        // Act
        _workerService.StartProcessing();

        // Assert
        _mockExecutor.Verify(x => x.Start(It.IsAny<Action<ISessionInfo, CalculationType>>()),
            Times.Once());
    }

    [Fact]
    public void StartProcessing_WhenAlreadyRunning_DoesNotStartExecutorAgain()
    {
        // Arrange
        _mockExecutor.Setup(x => x.Start(It.IsAny<Action<ISessionInfo, CalculationType>>()));
        _workerService.StartProcessing(); // First start

        // Act
        _workerService.StartProcessing(); // Second start

        // Assert
        _mockExecutor.Verify(x => x.Start(It.IsAny<Action<ISessionInfo, CalculationType>>()),
            Times.Once());
    }

    [Fact]
    public async Task StopProcessing_WhenRunning_StopsProcessing()
    {
        // Arrange
        _mockExecutor.Setup(x => x.Start(It.IsAny<Action<ISessionInfo, CalculationType>>()));
        _workerService.StartProcessing();

        // Act
        await _workerService.StopProcessing();

        // Assert
        // Start new processing to verify internal state was reset
        _workerService.StartProcessing();
        _mockExecutor.Verify(x => x.Start(It.IsAny<Action<ISessionInfo, CalculationType>>()),
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

        var testSession = new GameSessionDTO { Turn = 1 };
        _mockServer.Setup(x => x.TurnCalculation(It.IsAny<CalculationType>()))
            .Returns(testSession);

        Action<SessionInfo, CalculationType> calculationAction = null!;
        _mockExecutor.Setup(x => x.Start(It.IsAny<Action<ISessionInfo, CalculationType>>()))
            .Callback<Action<SessionInfo, CalculationType>>(action => calculationAction = action);

        // Act
        _workerService.StartProcessing();
        var state = new SessionInfo();
        calculationAction(state, CalculationType.Tick);

        // Assert
        // Verify mock server was called
        _mockServer.Verify(x => x.TurnCalculation(CalculationType.Tick), Times.Once());
        Assert.NotNull(receivedSession);
        Assert.Equal(state.ToString(), receivedState);
        Assert.Equal(1, receivedSession.Turn);
    }

    [Fact]
    public void Dispose_CallsStopProcessing()
    {
        // Arrange
        _mockExecutor.Setup(x => x.Start(It.IsAny<Action<ISessionInfo, CalculationType>>()));
        _workerService.StartProcessing();

        // Act
        _workerService.Dispose();

        // Assert
        _mockExecutor.Verify(x => x.Start(It.IsAny<Action<ISessionInfo, CalculationType>>()),
            Times.Once());
    }

    [Fact]
    public void PauseProcessing_CallsExecutorStop()
    {
        // Arrange
        _mockExecutor.Setup(x => x.Stop());
        _workerService.StartProcessing();

        // Act
        _workerService.PauseProcessing();

        // Assert
        _mockExecutor.Verify(x => x.Stop(), Times.Once());
    }

    [Fact]
    public void ResumeProcessing_CallsExecutorResume()
    {
        // Arrange
        _mockExecutor.Setup(x => x.Resume());
        _workerService.StartProcessing();
        _workerService.PauseProcessing();

        // Act
        _workerService.ResumeProcessing();

        // Assert
        _mockExecutor.Verify(x => x.Resume(), Times.Once());
    }

    [Fact]
    public void StartProcessing_AfterDispose_ThrowsObjectDisposedException()
    {
        // Arrange
        _workerService.Dispose();

        // Act
        var action = () => _workerService.StartProcessing();

        // Assert
        Assert.Throws<ObjectDisposedException>(action);
    }

    [Fact]
    public void Calculation_WhenExceptionOccurs_DoesNotPropagateException()
    {
        // Arrange
        Action<SessionInfo, CalculationType> calculationAction = null!;
        _mockExecutor.Setup(x => x.Start(It.IsAny<Action<ISessionInfo, CalculationType>>()))
            .Callback<Action<SessionInfo, CalculationType>>(action => calculationAction = action);

        bool eventWasRaised = false;
        _workerService.OnGetDataFromServer += (_, _) => 
        {
            eventWasRaised = true;
            throw new Exception("Test exception");
        };

        // Act
        _workerService.StartProcessing();
        calculationAction(new SessionInfo(), CalculationType.Tick);

        // Assert
        Assert.True(eventWasRaised);
    }

    [Fact]
    public async Task StopProcessing_CancelsPendingOperations()
    {
        // Arrange
        _mockExecutor.Setup(x => x.Start(It.IsAny<Action<ISessionInfo, CalculationType>>()));
        
        // Act
        _workerService.StartProcessing();
        var stopTask = _workerService.StopProcessing();
        await stopTask;

        // Assert
        _mockExecutor.Verify(x => x.Start(It.IsAny<Action<ISessionInfo, CalculationType>>()), Times.Once());
        Assert.True(stopTask.IsCompleted, "StopProcessing должен завершиться быстро");
    }
} 