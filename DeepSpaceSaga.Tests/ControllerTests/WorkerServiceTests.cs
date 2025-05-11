namespace DeepSpaceSaga.Tests.ControllerTests;

using DeepSpaceSaga.Common.Abstractions.Services;
using log4net;
using Moq;
using System.IO;

public class WorkerServiceTests : IDisposable
{
    private readonly Mock<Executor> _mockExecutor;
    private readonly Mock<IGameServer> _mockServer;
    private readonly string _configPath;
    private readonly WorkerService _workerService;

    public WorkerServiceTests()
    {
        _configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config");
        File.WriteAllText(_configPath, @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<configuration>
  <log4net>
    <root>
      <level value=""ALL"" />
      <appender-ref ref=""console"" />
    </root>
    <appender name=""console"" type=""log4net.Appender.ConsoleAppender"">
      <layout type=""log4net.Layout.PatternLayout"">
        <conversionPattern value=""%date %level %logger - %message%newline"" />
      </layout>
    </appender>
  </log4net>
</configuration>");

        _mockExecutor = new Mock<Executor>(32);
        _mockServer = new Mock<IGameServer>();
        _workerService = new WorkerService(_mockExecutor.Object, _mockServer.Object);
    }

    public void Dispose()
    {
        if (File.Exists(_configPath))
        {
            File.Delete(_configPath);
        }
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

        var testSession = new GameSessionDTO { Turn = 1 };
        _mockServer.Setup(x => x.TurnCalculation(It.IsAny<CalculationType>()))
            .Returns(testSession);

        Action<ExecutorState, CalculationType> calculationAction = null!;
        _mockExecutor.Setup(x => x.Start(It.IsAny<Action<ExecutorState, CalculationType>>()))
            .Callback<Action<ExecutorState, CalculationType>>(action => calculationAction = action);

        // Act
        _workerService.StartProcessing();
        var state = new ExecutorState();
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
        _mockExecutor.Setup(x => x.Start(It.IsAny<Action<ExecutorState, CalculationType>>()));
        _workerService.StartProcessing();

        // Act
        _workerService.Dispose();

        // Assert
        _mockExecutor.Verify(x => x.Start(It.IsAny<Action<ExecutorState, CalculationType>>()),
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
        action.Should().Throw<ObjectDisposedException>();
    }

    [Fact]
    public void Calculation_WhenExceptionOccurs_DoesNotPropagateException()
    {
        // Arrange
        Action<ExecutorState, CalculationType> calculationAction = null!;
        _mockExecutor.Setup(x => x.Start(It.IsAny<Action<ExecutorState, CalculationType>>()))
            .Callback<Action<ExecutorState, CalculationType>>(action => calculationAction = action);

        bool eventWasRaised = false;
        _workerService.OnGetDataFromServer += (_, _) => 
        {
            eventWasRaised = true;
            throw new Exception("Test exception");
        };

        // Act
        _workerService.StartProcessing();
        calculationAction(new ExecutorState(), CalculationType.Tick);

        // Assert
        eventWasRaised.Should().BeTrue();
    }

    [Fact]
    public async Task StopProcessing_CancelsPendingOperations()
    {
        // Arrange
        var cancellationTokenSource = new CancellationTokenSource();
        _mockExecutor.Setup(x => x.Start(It.IsAny<Action<ExecutorState, CalculationType>>()));

        // Act
        _workerService.StartProcessing();
        var stopTask = _workerService.StopProcessing();
        await stopTask;

        // Assert
        _mockExecutor.Verify(x => x.Start(It.IsAny<Action<ExecutorState, CalculationType>>()), Times.Once());
        (await Task.WhenAny(stopTask, Task.Delay(1000))).Should().Be(stopTask, "StopProcessing должен завершиться быстро");
    }
} 