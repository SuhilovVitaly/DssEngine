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
        var calculationCount = 0;
        void OnCalculation(ExecutorState state, CalculationType type) =>
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
        ExecutorState? lastState = null;
        void OnCalculation(ExecutorState state, CalculationType type) => lastState = state;

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
        ExecutorState? lastState = null;
        void OnCalculation(ExecutorState state, CalculationType type)
        {
            lastState = state;
            _calculations.Enqueue((state, type));
        }

        // Act
        _sut.Start(OnCalculation);
        await Task.Delay(5000); // Ждем достаточное время для выполнения нескольких ходов
        _sut.Stop();

        // Assert
        lastState.Should().NotBeNull();
        lastState!.TurnCounter.Should().BeGreaterThan(1, "должно быть выполнено несколько ходов");
        _calculations.Count(c => c.Type == CalculationType.Turn).Should().BeGreaterThan(1, "должно быть несколько событий типа Turn");
    }

    [Fact]
    public async Task TickCalculation_SetsPausedStateCorrectly()
    {
        // Arrange
        var statesDuringCalculation = new List<bool>();
        void OnCalculation(ExecutorState state, CalculationType type)
        {
            statesDuringCalculation.Add(state.IsPaused);
            Thread.Sleep(50); // Имитируем длительные вычисления
        }

        // Act
        _sut.Start(OnCalculation);
        await Task.Delay(200); // Ждем пару тиков
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
        
        void OnCalculation(ExecutorState state, CalculationType type)
        {
            try
            {
                // Имитируем сложные вычисления с доступом к общим ресурсам
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
        await Task.Delay(500); // Даем время на выполнение нескольких тиков
        _sut.Stop();

        // Assert
        errors.Should().BeEmpty();
        calculationCount.Should().BeGreaterThan(0);
    }

    [Fact]
    public void Resume_AfterDispose_ThrowsObjectDisposedException()
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
    public async Task Stop_SetsPausedStateCorrectly()
    {
        // Arrange
        ExecutorState? lastState = null;
        void OnCalculation(ExecutorState state, CalculationType type) => lastState = state;

        // Act
        _sut.Start(OnCalculation);
        await Task.Delay(200); // Ждем пару тиков
        _sut.Stop();
        await Task.Delay(100); // Даем время на обработку остановки

        // Assert
        lastState.Should().NotBeNull();
        lastState!.IsPaused.Should().BeTrue("состояние должно быть приостановлено после остановки");
    }

    public void Dispose()
    {
        _sut.Dispose();
    }
}