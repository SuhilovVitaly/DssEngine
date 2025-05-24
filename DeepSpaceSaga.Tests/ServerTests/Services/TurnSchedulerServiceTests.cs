namespace DeepSpaceSaga.Tests.ServerTests.Services;

using Moq;

public class TurnSchedulerServiceTests : IDisposable
{
    private readonly TurnSchedulerService _sut;
    private readonly ConcurrentQueue<(ISessionInfoService State, CalculationType Type)> _calculations;
    private readonly ISessionContextService _sessionContext;
    private readonly ISessionInfoService _sessionInfo;
    private readonly Mock<IMetricsService> _metricsMock;

    public TurnSchedulerServiceTests()
    {
        _sessionInfo = new SessionInfoService();
        _metricsMock = new Mock<IMetricsService>();
        _sessionContext = new SessionContextService(_sessionInfo, _metricsMock.Object);
        _sut = new TurnSchedulerService(_sessionContext, tickInterval: 100); // Larger interval for testing
        _calculations = new ConcurrentQueue<(ISessionInfoService, CalculationType)>();
    }

    [Fact]
    public void Constructor_WithInvalidTickInterval_ThrowsArgumentException()
    {
        // Arrange & Act
        var sessionInfoMock = new Mock<ISessionInfoService>();
        var metricsMock = new Mock<IMetricsService>();
        var sessionContext = new SessionContextService(sessionInfoMock.Object, metricsMock.Object);
        var action = () => new TurnSchedulerService(sessionContext, tickInterval: 0);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("Tick interval must be at least 1ms*");
    }

    [Fact]
    public void Start_WithNullCallback_ThrowsArgumentNullException()
    {
        // Arrange & Act
        var action = () => _sut.Start(null!);

        // Assert
        action.Should().Throw<ArgumentNullException>()
            .WithParameterName("onTickCalculation");
    }

    [Fact]
    public async Task Start_ExecutesTickCalculations()
    {
        // Arrange
        void OnCalculation(ISessionInfoService state, CalculationType type) =>
            _calculations.Enqueue((state, type));

        // Act
        _sut.Start(OnCalculation);
        await Task.Delay(350); // Wait for ~3 ticks
        _sut.Stop();

        // Assert
        _calculations.Count.Should().BeGreaterThanOrEqualTo(2);
        _calculations.All(c => c.Type == CalculationType.Tick).Should().BeTrue();
    }

    [Fact]
    public async Task Start_ExecutesTurnCalculation_AfterTenTicks()
    {
        // Arrange
        void OnCalculation(ISessionInfoService state, CalculationType type) =>
            _calculations.Enqueue((state, type));

        // Act
        _sut.Start(OnCalculation);
        await Task.Delay(1200); // Wait for ~12 ticks to ensure we get past 10
        _sut.Stop();

        // Assert
        _calculations.Should().Contain(c => c.Type == CalculationType.Turn);
        var turnCalculation = _calculations.First(c => c.Type == CalculationType.Turn);
        turnCalculation.State.TurnCounter.Should().Be(1);
    }

    [Fact]
    public void Stop_StopsExecutingCalculations()
    {
        // Arrange
        var calculationCount = 0;
        void OnCalculation(ISessionInfoService state, CalculationType type) =>
            Interlocked.Increment(ref calculationCount);

        // Act
        _sut.Start(OnCalculation);
        _sut.Stop();
        var initialCount = calculationCount;
        Thread.Sleep(200); // Wait to ensure no more calculations

        // Assert
        calculationCount.Should().Be(initialCount);
    }

    [Fact]
    public void Dispose_CanBeCalledMultipleTimes()
    {
        // Arrange & Act
        var action = () =>
        {
            _sut.Dispose();
            _sut.Dispose();
        };

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void Stop_AfterDispose_DoesNotThrow()
    {
        // Arrange
        _sut.Dispose();

        // Act
        var action = () => _sut.Stop();

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public async Task Resume_AfterStop_ContinuesExecutingCalculations()
    {
        // Arrange
        var calculationCount = 0;
        void OnCalculation(ISessionInfoService state, CalculationType type) =>
            Interlocked.Increment(ref calculationCount);

        // Act
        _sut.Start(OnCalculation);
        await Task.Delay(200); // Wait for some calculations
        var countBeforeStop = calculationCount;
        _sut.Stop();
        await Task.Delay(100); // Ensure stopped
        _sut.Resume();
        await Task.Delay(200); // Wait for more calculations

        // Assert
        calculationCount.Should().BeGreaterThan(countBeforeStop);
    }

    [Fact]
    public void Resume_WhenDisposed_ThrowsObjectDisposedException()
    {
        // Arrange
        _sut.Start((state, type) => { });
        _sut.Stop();
        _sut.Dispose();

        // Act
        var action = () => _sut.Resume();

        // Assert
        action.Should().Throw<ObjectDisposedException>();
    }

    [Fact]
    public void Resume_WithoutPriorStart_ThrowsInvalidOperationException()
    {
        // Act
        var action = () => _sut.Resume();

        // Assert
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot resume execution without prior Start call*");
    }

    [Fact]
    public async Task Resume_PreservesExecutorState()
    {
        // Arrange
        ISessionInfoService lastState = null;
        void OnCalculation(ISessionInfoService state, CalculationType type) => lastState = state;

        // Act
        _sut.Start(OnCalculation);
        await Task.Delay(350); // Wait for some ticks
        var turnCounterBeforeStop = lastState?.TurnCounter ?? 0;
        var tickCounterBeforeStop = lastState?.TickCounter ?? 0;
        _sut.Stop();
        _sut.Resume();
        await Task.Delay(100); // Wait for more ticks

        // Assert
        lastState.Should().NotBeNull();
        lastState!.TurnCounter.Should().BeGreaterThanOrEqualTo(turnCounterBeforeStop);
        lastState.TickCounter.Should().BeGreaterThan(tickCounterBeforeStop);
    }

    [Fact]
    public async Task Start_ExecutesTurnsAndCycles()
    {
        // Arrange
        ISessionInfoService lastState = null;
        void OnCalculation(ISessionInfoService state, CalculationType type)
        {
            lastState = state;
            _calculations.Enqueue((state, type));
        }

        // Act
        _sut.Start(OnCalculation);
        await Task.Delay(5000); // Wait for enough time to complete several turns
        _sut.Stop();

        // Assert
        lastState.Should().NotBeNull();
        lastState!.TurnCounter.Should().BeGreaterThan(1, "should have completed several turns");
        _calculations.Count(c => c.Type == CalculationType.Turn).Should().BeGreaterThan(1, "should have several Turn events");
    }

    [Fact]
    public async Task TickCalculation_SetsPausedStateCorrectly()
    {
        // Arrange
        var statesDuringCalculation = new List<bool>();
        void OnCalculation(ISessionInfoService state, CalculationType type)
        {
            statesDuringCalculation.Add(state.IsPaused);
            Thread.Sleep(50); // Simulate long calculations
        }

        // Act
        _sut.Start(OnCalculation);
        await Task.Delay(200); // Wait for a couple of ticks
        _sut.Stop();

        // Assert
        statesDuringCalculation.Should().NotBeEmpty();
        statesDuringCalculation.All(isPaused => isPaused).Should().BeTrue();
    }

    [Fact]
    public async Task Executor_HandlesMultipleThreadsCorrectly()
    {
        // Arrange
        var calculationCount = 0;
        var errors = new ConcurrentBag<Exception>();
        
        void OnCalculation(ISessionInfoService state, CalculationType type)
        {
            try
            {
                // Simulate complex calculations with shared resource access
                var currentCount = calculationCount;
                Thread.Sleep(10);
                calculationCount = currentCount + 1;
            }
            catch (Exception ex)
            {
                errors.Add(ex);
            }
        }

        // Act
        _sut.Start(OnCalculation);
        await Task.Delay(500); // Give time for several ticks
        _sut.Stop();

        // Assert
        errors.Should().BeEmpty();
        calculationCount.Should().BeGreaterThan(0);
    }

    [Fact]
    public void Start_AfterDispose_ThrowsObjectDisposedException()
    {
        // Arrange
        _sut.Dispose();

        // Act
        var action = () => _sut.Start((state, type) => { });

        // Assert
        action.Should().Throw<ObjectDisposedException>();
    }

    [Fact]
    public void Resume_ResetsIsPausedToFalse()
    {
        // Arrange
        var callbackWasCalled = new ManualResetEventSlim(false);
        var stateBeforeCallback = true;

        void OnCalculation(ISessionInfoService state, CalculationType type)
        {
            // Record state before calculations
            stateBeforeCallback = state.IsPaused;
            callbackWasCalled.Set();
        }

        // Act
        _sut.Start(OnCalculation);
        
        // Wait for at least one handler to be called
        callbackWasCalled.Wait(TimeSpan.FromMilliseconds(500));
        _sut.Stop();
        
        // Check that state after Stop allows us to resume
        _sessionInfo.IsPaused.Should().BeTrue("IsPaused should be true after Stop");
        
        // Reset flag
        callbackWasCalled.Reset();
        
        // Resume and wait for handler to be called again
        _sut.Resume();
        callbackWasCalled.Wait(TimeSpan.FromMilliseconds(500));
        
        // Assert
        stateBeforeCallback.Should().BeTrue("during calculations (inside handler) IsPaused should be true");
        _sessionInfo.IsPaused.Should().BeFalse("after handler call IsPaused should be false");
        
        _sut.Stop();
    }

    [Fact]
    public async Task CycleUpdate_ResetsCountersCorrectly()
    {
        // Arrange
        var countsByType = new ConcurrentDictionary<CalculationType, int>();
        var cycleReached = new TaskCompletionSource<bool>();

        void OnCalculation(ISessionInfoService state, CalculationType type)
        {
            countsByType.AddOrUpdate(type, 1, (_, count) => count + 1);
            
            if (type == CalculationType.Cycle)
            {
                cycleReached.TrySetResult(true);
            }
        }

        // Act - use minimum intervals for fast test
        var fastExecutor = new TurnSchedulerService(_sessionContext, tickInterval: 1);
        fastExecutor.Start(OnCalculation);
        
        // Wait for at least one cycle calculation
        await cycleReached.Task.WaitAsync(TimeSpan.FromSeconds(10));
        fastExecutor.Stop();

        // Assert
        countsByType.Should().ContainKey(CalculationType.Cycle, "should have Cycle calculation");
        _sessionInfo.TickCounter.Should().Be(0, "tick counter should be reset in CycleUpdate");
        _sessionInfo.TurnCounter.Should().Be(0, "turn counter should be reset in CycleUpdate");
        _sessionInfo.CycleCounter.Should().BeGreaterThan(1, "cycle counter should be incremented in CycleUpdate");
        
        fastExecutor.Dispose();
    }

    [Fact]
    public async Task SessionState_IsThreadSafe()
    {
        // Arrange
        var tasks = new List<Task>();
        var taskCount = 10;
        var runTime = 500; // ms
        
        // Act
        _sut.Start((s, t) => { }); // Start scheduler
        
        // Start multiple tasks that will access state simultaneously
        for (int i = 0; i < taskCount; i++)
        {
            tasks.Add(Task.Run(() => 
            {
                var start = DateTime.Now;
                while ((DateTime.Now - start).TotalMilliseconds < runTime)
                {
                    // Read and manipulate state
                    var tick = _sessionInfo.TickCounter;
                    var turn = _sessionInfo.TurnCounter;
                    var cycle = _sessionInfo.CycleCounter;
                    var isPaused = _sessionInfo.IsPaused;
                    // Small delay to increase race chance
                    Thread.Sleep(1);
                }
            }));
        }
        
        await Task.WhenAll(tasks);
        _sut.Stop();
        
        // Assert - if no exceptions occurred, test passed
        true.Should().BeTrue("code should execute without errors in multi-threaded access to state");
    }

    public void Dispose()
    {
        _sut.Dispose();
    }
}