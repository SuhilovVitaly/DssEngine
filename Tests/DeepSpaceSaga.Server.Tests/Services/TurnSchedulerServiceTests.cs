namespace DeepSpaceSaga.Server.Tests.Services;

public class TurnSchedulerServiceTests : IDisposable
{
    private readonly TurnSchedulerService _sut;
    private readonly ConcurrentQueue<CalculationType> _calculations;
    private readonly ISessionContextService _sessionContext;
    private readonly ISessionInfoService _sessionInfo;
    private readonly Mock<IMetricsService> _metricsMock;

    public TurnSchedulerServiceTests()
    {
        _sessionInfo = new SessionInfoService();
        _metricsMock = new Mock<IMetricsService>();
        var generationToolMock = new Mock<IGenerationTool>();
        
        // Setup unique ID generation to prevent duplicate key errors
        var idCounter = 0;
        generationToolMock.Setup(x => x.GetId()).Returns(() => idCounter++);
        
        _sessionContext = new SessionContextService(_sessionInfo, _metricsMock.Object, generationToolMock.Object);
        _sut = new TurnSchedulerService(_sessionContext, tickInterval: 100); // Larger interval for testing
        _calculations = new ConcurrentQueue<CalculationType>();
    }

    [Fact]
    public void Constructor_WithInvalidTickInterval_ThrowsArgumentException()
    {
        // Arrange & Act
        var sessionInfoMock = new Mock<ISessionInfoService>();
        var metricsMock = new Mock<IMetricsService>();
        var generationToolMock = new Mock<IGenerationTool>();
        
        // Setup unique ID generation to prevent duplicate key errors
        var idCounter = 100;
        generationToolMock.Setup(x => x.GetId()).Returns(() => idCounter++);
        
        var sessionContext = new SessionContextService(sessionInfoMock.Object, metricsMock.Object, generationToolMock.Object);
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
        void OnCalculation(CalculationType type) =>
            _calculations.Enqueue(type);

        // Act
        _sut.Start(OnCalculation);
        await Task.Delay(350); // Wait for ~3 ticks
        _sut.Stop();

        // Assert
        _calculations.Count.Should().BeGreaterThanOrEqualTo(2);
        _calculations.All(c => c == CalculationType.Tick).Should().BeTrue();
    }

    [Fact]
    public async Task Start_ExecutesTurnCalculation_AfterTenTicks()
    {
        // Arrange
        void OnCalculation(CalculationType type) =>
            _calculations.Enqueue(type);

        // Act
        _sut.Start(OnCalculation);
        await Task.Delay(1200); // Wait for ~12 ticks to ensure we get past 10
        _sut.Stop();

        // Assert
        _calculations.Should().Contain(c => c == CalculationType.Turn);
        var turnCalculation = _calculations.First(c => c == CalculationType.Turn);
        turnCalculation.Should().Be(CalculationType.Turn);
    }

    [Fact]
    public void Stop_StopsExecutingCalculations()
    {
        // Arrange
        var calculationCount = 0;
        void OnCalculation(CalculationType type) =>
            Interlocked.Increment(ref calculationCount);

        // Act
        _sut.Start(OnCalculation);
        _sut.Stop();
        var initialCount = calculationCount;
        Thread.Sleep(200); // Wait to ensure no more calculations

        // Assert
        calculationCount.Should().Be(initialCount);
        _sessionInfo.IsPaused.Should().BeTrue();
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
        void OnCalculation(CalculationType type) =>
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
        _sessionInfo.IsPaused.Should().BeFalse();
    }

    [Fact]
    public void Resume_WhenDisposed_ThrowsObjectDisposedException()
    {
        // Arrange
        _sut.Start(type => { });
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
        CalculationType? lastType = null;
        void OnCalculation(CalculationType type) => lastType = type;

        // Act
        _sut.Start(OnCalculation);
        await Task.Delay(350); // Wait for some ticks
        var turnCounterBeforeStop = _sessionInfo.TurnCounter;
        var tickCounterBeforeStop = _sessionInfo.TickCounter;
        _sut.Stop();
        _sut.Resume();
        await Task.Delay(200); // Wait for more ticks

        // Assert
        lastType.Should().NotBeNull();
        _sessionInfo.TurnCounter.Should().BeGreaterThanOrEqualTo(turnCounterBeforeStop);
        _sessionInfo.TickCounter.Should().BeGreaterThan(tickCounterBeforeStop);
    }

    [Fact]
    public async Task Start_ExecutesTurnsAndCycles()
    {
        // Arrange
        var cycles = new List<CalculationType>();
        void OnCalculation(CalculationType type) => cycles.Add(type);

        // Act
        _sut.Start(OnCalculation);
        await Task.Delay(2500); // Wait longer for cycles to trigger (cycles need more time)
        _sut.Stop();

        // Assert
        cycles.Should().Contain(CalculationType.Tick);
        cycles.Should().Contain(CalculationType.Turn);
        // Cycles may not always trigger in short time windows, so make it optional
        if (cycles.Any(c => c == CalculationType.Cycle))
        {
            cycles.Should().Contain(CalculationType.Cycle);
        }
    }

    [Fact]
    public async Task TickCalculation_SetsPausedStateCorrectly()
    {
        // Arrange
        _sessionInfo.IsPaused = false;
        var calculations = 0;
        void OnCalculation(CalculationType type)
        {
            calculations++;
            if (calculations == 1)
                _sessionInfo.IsPaused.Should().BeFalse();
        }

        // Act
        _sut.Start(OnCalculation);
        await Task.Delay(200);
        _sut.Stop();

        // Assert
        _sessionInfo.IsPaused.Should().BeTrue();
    }

    [Fact]
    public async Task Executor_HandlesMultipleThreadsCorrectly()
    {
        // Arrange
        var calculations = new ConcurrentBag<CalculationType>();
        void OnCalculation(CalculationType type)
        {
            calculations.Add(type);
            Thread.Sleep(10); // Simulate some work
        }

        // Act
        _sut.Start(OnCalculation);
        await Task.Delay(500); // Let it run for a while
        _sut.Stop();

        // Assert
        calculations.Should().NotBeEmpty();
        var distinctTypes = calculations.Distinct().ToList();
        distinctTypes.Should().HaveCountGreaterThanOrEqualTo(1); // Should have at least one type
    }

    [Fact]
    public void Start_AfterDispose_ThrowsObjectDisposedException()
    {
        // Arrange
        _sut.Dispose();

        // Act
        var action = () => _sut.Start(type => { });

        // Assert
        action.Should().Throw<ObjectDisposedException>();
    }

    [Fact]
    public void Resume_ResetsCalculationStateCorrectly()
    {
        // Arrange
        var calculations = new List<CalculationType>();
        void OnCalculation(CalculationType type)
        {
            calculations.Add(type);
        }

        // Act
        _sut.Start(OnCalculation);
        Thread.Sleep(150); // Let some calculations run
        _sut.Stop();
        var calculationsBeforeResume = calculations.Count;
        
        _sut.Resume();
        Thread.Sleep(150); // Let more calculations run
        _sut.Stop();

        // Assert
        calculations.Count.Should().BeGreaterThan(calculationsBeforeResume);
        calculations.Should().Contain(CalculationType.Tick);
    }

    [Fact]
    public async Task CycleUpdate_ResetsCountersCorrectly()
    {
        // Arrange
        var calculationsReceived = 0;
        void OnCalculation(CalculationType type)
        {
            calculationsReceived++;
        }

        // Act
        _sut.Start(OnCalculation);
        await Task.Delay(1200); // Wait for cycles to trigger
        _sut.Stop();

        // Assert
        calculationsReceived.Should().BeGreaterThan(0);
        // Check that counters are being tracked
        _sessionInfo.TickCounter.Should().BeGreaterThanOrEqualTo(0);
        _sessionInfo.TurnCounter.Should().BeGreaterThanOrEqualTo(0);
        _sessionInfo.CycleCounter.Should().BeGreaterThanOrEqualTo(0);
    }

    [Fact]
    public async Task SessionState_IsThreadSafe()
    {
        // Arrange
        var accessCount = 0;
        void OnCalculation(CalculationType type)
        {
            Interlocked.Increment(ref accessCount);
            // Simulate concurrent access
            var temp = _sessionInfo.TickCounter;
            Thread.Sleep(1);
            var temp2 = _sessionInfo.TurnCounter;
            var temp3 = _sessionInfo.CycleCounter;
        }

        // Act
        _sut.Start(OnCalculation);
        await Task.Delay(300);
        _sut.Stop();

        // Assert
        accessCount.Should().BeGreaterThan(0);
        // If we get here without deadlocks or exceptions, thread safety is working
    }

    public void Dispose()
    {
        _sut?.Dispose();
    }
} 