namespace DeepSpaceSaga.Tests.ControllerTests.GameLoopTools;

public class ExecutorTests : IDisposable
{
    private readonly Executor _sut;
    private readonly ConcurrentQueue<(ExecutorState State, CalculationType Type)> _calculations;

    public ExecutorTests()
    {
        _sut = new Executor(tickInterval: 100); // Larger interval for testing
        _calculations = new ConcurrentQueue<(ExecutorState, CalculationType)>();
    }

    [Fact]
    public void Constructor_WithInvalidTickInterval_ThrowsArgumentException()
    {
        // Arrange & Act
        var action = () => new Executor(tickInterval: 0);

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
        void OnCalculation(ExecutorState state, CalculationType type) =>
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
        void OnCalculation(ExecutorState state, CalculationType type) =>
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
        void OnCalculation(ExecutorState state, CalculationType type) =>
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
        void OnCalculation(ExecutorState state, CalculationType type) =>
            _calculations.Enqueue((state, type));

        // Act
        _sut.Start(OnCalculation);
        await Task.Delay(200); // Wait for ~2 ticks
        _sut.Stop();
        var countAfterStop = _calculations.Count;
        _sut.Resume();
        await Task.Delay(200); // Wait for ~2 more ticks

        // Assert
        _calculations.Count.Should().BeGreaterThan(countAfterStop);
    }

    [Fact]
    public async Task Resume_WithoutPriorStart_ThrowsInvalidOperationException()
    {
        // Arrange & Act
        var action = () => _sut.Resume();

        // Assert
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot resume execution without prior Start call*");
    }

    [Fact]
    public async Task Resume_PreservesExecutorState()
    {
        // Arrange
        var lastState = new ExecutorState();
        void OnCalculation(ExecutorState state, CalculationType type) => lastState = state;

        // Act
        _sut.Start(OnCalculation);
        await Task.Delay(350); // Wait for ~3 ticks
        var turnCounterBeforeStop = lastState.TurnCounter;
        var tickCounterBeforeStop = lastState.TickCounter;
        _sut.Stop();
        _sut.Resume();
        await Task.Delay(100); // Wait for 1 more tick

        // Assert
        lastState.TurnCounter.Should().BeGreaterThanOrEqualTo(turnCounterBeforeStop);
        lastState.TickCounter.Should().BeGreaterThan(tickCounterBeforeStop);
    }

    public void Dispose()
    {
        _sut.Dispose();
    }
}