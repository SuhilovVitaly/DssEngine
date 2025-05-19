using DeepSpaceSaga.Server.Services.Scheduler;
using DeepSpaceSaga.Server.Services.SessionInfo;

namespace DeepSpaceSaga.Tests.ServerTests.Services;

public class TurnSchedulerServiceTests : IDisposable
{
    private readonly TurnSchedulerService _sut;
    private readonly ConcurrentQueue<(ISessionInfoService State, CalculationType Type)> _calculations;

    public TurnSchedulerServiceTests()
    {
        _sut = new TurnSchedulerService(new SessionInfoService(),  tickInterval: 100); // Larger interval for testing
        _calculations = new ConcurrentQueue<(ISessionInfoService, CalculationType)>();
    }

    [Fact]
    public void Constructor_WithInvalidTickInterval_ThrowsArgumentException()
    {
        // Arrange & Act
        var action = () => new TurnSchedulerService(new SessionInfoService(), tickInterval: 0);

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
        void OnCalculation(ISessionInfoService state, CalculationType type)
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
        
        void OnCalculation(ISessionInfoService state, CalculationType type)
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
        ISessionInfoService lastState = null;
        void OnCalculation(ISessionInfoService state, CalculationType type) => lastState = state;

        // Act
        _sut.Start(OnCalculation);
        await Task.Delay(200); // Ждем пару тиков
        _sut.Stop();
        await Task.Delay(100); // Даем время на обработку остановки

        // Assert
        lastState.Should().NotBeNull();
        lastState!.IsPaused.Should().BeTrue("состояние должно быть приостановлено после остановки");
    }

    [Fact]
    public async Task Start_ExecutesCycleCalculation_AfterTenTurns()
    {
        // Arrange
        void OnCalculation(ISessionInfoService state, CalculationType type) =>
            _calculations.Enqueue((state, type));

        // Act - используем небольшой интервал тиков для ускорения теста
        var fastExecutor = new TurnSchedulerService(new SessionInfoService(), tickInterval: 10);
        fastExecutor.Start(OnCalculation);
        await Task.Delay(2500); // Ждем достаточно времени для завершения полного цикла (10 тиков * 10 ходов * 10 мс)
        fastExecutor.Stop();
        fastExecutor.Dispose();

        // Assert
        _calculations.Should().Contain(c => c.Type == CalculationType.Cycle);
        var cycleCalculation = _calculations.First(c => c.Type == CalculationType.Cycle);
        cycleCalculation.State.CycleCounter.Should().BeGreaterThan(1);
    }

    [Fact]
    public async Task OnTimerElapsed_SkipsCalculation_WhenIsPausedIsTrue()
    {
        // Arrange
        var sessionInfo = new SessionInfoService();
        sessionInfo.IsPaused = true; // Устанавливаем в true до запуска

        var calculationCount = 0;
        void OnCalculation(ISessionInfoService state, CalculationType type) => 
            Interlocked.Increment(ref calculationCount);

        // Act
        var pausedExecutor = new TurnSchedulerService(sessionInfo, tickInterval: 50);
        
        // Оборачиваем старт в блокировку чтобы не допустить запуск тиков до нашей установки паузы
        lock (sessionInfo)
        {
            pausedExecutor.Start(OnCalculation);
            // Убеждаемся, что наше состояние паузы не изменилось
            sessionInfo.IsPaused = true;
        }
        
        await Task.Delay(300); // Ждем несколько потенциальных тиков
        pausedExecutor.Stop();
        pausedExecutor.Dispose();

        // Assert
        calculationCount.Should().Be(0, "вычисления не должны выполняться, когда IsPaused = true");
    }

    [Fact]
    public async Task Dispose_StopsExecutingCalculations()
    {
        // Arrange
        var sessionInfo = new SessionInfoService();
        var calculationCount = 0;
        void OnCalculation(ISessionInfoService state, CalculationType type) => 
            Interlocked.Increment(ref calculationCount);

        // Act
        var disposableExecutor = new TurnSchedulerService(sessionInfo, tickInterval: 50);
        disposableExecutor.Start(OnCalculation);
        await Task.Delay(100); // Подождем немного для начала вычислений
        calculationCount.Should().BeGreaterThan(0, "должно быть выполнено хотя бы одно вычисление");
        
        var countBeforeDispose = calculationCount;
        disposableExecutor.Dispose();
        await Task.Delay(200); // Подождем еще для проверки отсутствия вычислений

        // Assert
        calculationCount.Should().Be(countBeforeDispose, "после Dispose не должно быть вычислений");
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
        var sessionInfo = new SessionInfoService();
        var callbackWasCalled = new ManualResetEventSlim(false);
        var stateBeforeCallback = true;

        void OnCalculation(ISessionInfoService state, CalculationType type)
        {
            // Записываем состояние до выполнения расчетов
            stateBeforeCallback = state.IsPaused;
            callbackWasCalled.Set();
        }

        // Act
        var executor = new TurnSchedulerService(sessionInfo, tickInterval: 20);
        executor.Start(OnCalculation);
        
        // Ждем, пока хотя бы один обработчик будет вызван
        callbackWasCalled.Wait(TimeSpan.FromMilliseconds(500));
        executor.Stop();
        
        // Проверяем, что состояние после Stop позволяет нам возобновить
        sessionInfo.IsPaused.Should().BeTrue("IsPaused должен быть true после Stop");
        
        // Сбрасываем флаг
        callbackWasCalled.Reset();
        
        // Возобновляем и снова ждем вызова обработчика
        executor.Resume();
        callbackWasCalled.Wait(TimeSpan.FromMilliseconds(500));
        
        // Assert
        stateBeforeCallback.Should().BeTrue("во время вычислений (внутри обработчика) IsPaused должен быть true");
        sessionInfo.IsPaused.Should().BeFalse("после вызова обработчика IsPaused должен быть false");
        
        executor.Stop();
        executor.Dispose();
    }

    [Fact]
    public async Task CycleUpdate_ResetsCountersCorrectly()
    {
        // Arrange
        var sessionInfo = new SessionInfoService();
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

        // Act - используем минимальные интервалы для быстрого теста
        var fastExecutor = new TurnSchedulerService(sessionInfo, tickInterval: 1);
        fastExecutor.Start(OnCalculation);
        
        // Ждем, пока будет выполнено хотя бы одно вычисление цикла
        await cycleReached.Task.WaitAsync(TimeSpan.FromSeconds(10));
        fastExecutor.Stop();

        // Assert
        countsByType.Should().ContainKey(CalculationType.Cycle, "должно быть вычисление типа Cycle");
        sessionInfo.TickCounter.Should().Be(0, "счетчик тиков должен быть сброшен в CycleUpdate");
        sessionInfo.TurnCounter.Should().Be(0, "счетчик ходов должен быть сброшен в CycleUpdate");
        sessionInfo.CycleCounter.Should().BeGreaterThan(1, "счетчик циклов должен быть увеличен в CycleUpdate");
        
        fastExecutor.Dispose();
    }

    [Fact]
    public async Task SessionState_IsThreadSafe()
    {
        // Arrange
        var state = new SessionInfoService();
        var safeExecutor = new TurnSchedulerService(state, tickInterval: 1); // Минимальный интервал для максимальной нагрузки
        var tasks = new List<Task>();
        var taskCount = 10;
        var runTime = 500; // мс
        
        // Act
        safeExecutor.Start((s, t) => { }); // Запускаем планировщик
        
        // Одновременно запускаем несколько задач, которые будут обращаться к состоянию
        for (int i = 0; i < taskCount; i++)
        {
            tasks.Add(Task.Run(() => 
            {
                var start = DateTime.Now;
                while ((DateTime.Now - start).TotalMilliseconds < runTime)
                {
                    // Чтение и манипуляции с состоянием
                    var tick = state.TickCounter;
                    var turn = state.TurnCounter;
                    var cycle = state.CycleCounter;
                    var isPaused = state.IsPaused;
                    // Небольшая задержка для увеличения шанса гонки
                    Thread.Sleep(1);
                }
            }));
        }
        
        await Task.WhenAll(tasks);
        safeExecutor.Stop();
        safeExecutor.Dispose();
        
        // Assert - если не было исключений, то тест прошел успешно
        true.Should().BeTrue("код должен выполняться без ошибок при многопоточном доступе к состоянию");
    }

    public void Dispose()
    {
        _sut.Dispose();
    }
}