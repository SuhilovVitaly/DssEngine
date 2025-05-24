namespace DeepSpaceSaga.Server.Services.Scheduler;

/// <summary>
/// Manages game loop execution with configurable ticks, turns and cycles
/// </summary>
public sealed class TurnSchedulerService
{
    private const int DefaultTicksPerTurn = 10;
    private const int DefaultTurnsPerCycle = 10;
    private const int MinimumTickInterval = 1;

    private readonly int _ticksPerTurn;
    private readonly int _turnsPerCycle;
    private readonly object _stateLock = new();
    private readonly ISessionContextService _sessionContext;
    private readonly Timer _timer;

    private Action<ISessionInfoService, CalculationType>? _calculationEvent;
    private bool _isDisposed;

    /// <summary>
    /// Initializes a new instance of the Executor
    /// </summary>
    /// <param name="state"></param>
    /// <param name="tickInterval">The interval between ticks in milliseconds</param>
    /// <exception cref="ArgumentException">Thrown when tickInterval is less than 1</exception>
    public TurnSchedulerService(ISessionContextService sessionContext, int tickInterval = 32)
    {
        if (tickInterval < MinimumTickInterval)
            throw new ArgumentException($"Tick interval must be at least {MinimumTickInterval}ms", nameof(tickInterval));
        _sessionContext = sessionContext;
        _ticksPerTurn = DefaultTicksPerTurn;
        _turnsPerCycle = DefaultTurnsPerCycle;
        _timer = new Timer(tickInterval);
        _timer.Elapsed += OnTimerElapsed;
        _timer.AutoReset = true;
        _timer.Enabled = false;
    }

    /// <summary>
    /// Starts the execution loop
    /// </summary>
    /// <param name="onTickCalculation">Callback to be executed on each calculation step</param>
    /// <exception cref="ArgumentNullException">Thrown when onTickCalculation is null</exception>
    public void Start(Action<ISessionInfoService, CalculationType> onTickCalculation)
    {
        _calculationEvent = onTickCalculation ?? throw new ArgumentNullException(nameof(onTickCalculation));
        _timer.Enabled = true;
        _sessionContext.SessionInfo.IsPaused = false;
        _sessionContext.SessionInfo.IsCalculationInProgress = false;
    }

    private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        if (_sessionContext.SessionInfo.IsPaused == true || _sessionContext.SessionInfo.IsCalculationInProgress) return;

        lock (_stateLock)
        {
            TickUpdate();
        }
    }

    private void TickUpdate()
    {
        var currentTickCount = _sessionContext.SessionInfo.IncrementTickCounter();

        _sessionContext.SessionInfo.IncrementTickTotal();

        if (currentTickCount >= _ticksPerTurn)
        {
            TurnUpdate();
            return;
        }

        TickCalculation(CalculationType.Tick);
    }

    private void TurnUpdate()
    {
        _sessionContext.SessionInfo.ResetTickCounter();

        if (_sessionContext.SessionInfo.TurnCounter >= _turnsPerCycle)
        {
            CycleUpdate();
        }
        else
        {
            TickCalculation(CalculationType.Turn);
            _sessionContext.SessionInfo.IncrementTurnCounter();
        }
    }

    private void CycleUpdate()
    {
        _sessionContext.SessionInfo.ResetTurnCounter();
        _sessionContext.SessionInfo.IncrementCycleCounter();
        TickCalculation(CalculationType.Cycle);
    }

    private void TickCalculation(CalculationType type)
    {
        _sessionContext.SessionInfo.IsCalculationInProgress = true;
        _calculationEvent?.Invoke(_sessionContext.SessionInfo, type);
        _sessionContext.SessionInfo.IsCalculationInProgress = false;
    }

    /// <summary>
    /// Stops the execution loop
    /// </summary>
    public void Stop()
    {
        if (_isDisposed) return;
        _timer.Enabled = false;
        _sessionContext.SessionInfo.IsPaused = true;
        _sessionContext.SessionInfo.IsCalculationInProgress = true;
    }

    public void Resume()
    {
        if (_isDisposed)
            throw new ObjectDisposedException(nameof(TurnSchedulerService));

        if (_calculationEvent == null)
            throw new InvalidOperationException("Cannot resume execution without prior Start call");

        _timer.Enabled = true;
        _sessionContext.SessionInfo.IsPaused = false;
        _sessionContext.SessionInfo.IsCalculationInProgress = false;
    }

    public void Dispose()
    {
        if (_isDisposed) return;

        _timer.Dispose();
        _isDisposed = true;
        GC.SuppressFinalize(this);
    }
}
