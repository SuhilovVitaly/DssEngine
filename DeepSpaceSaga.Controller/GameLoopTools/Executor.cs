namespace DeepSpaceSaga.Controller.GameLoopTools;

/// <summary>
/// Manages game loop execution with configurable ticks, turns and cycles
/// </summary>
public class Executor : IDisposable
{
    private const int DefaultTicksPerTurn = 10;
    private const int DefaultTurnsPerCycle = 10;
    private const int MinimumTickInterval = 1;

    private readonly int _ticksPerTurn;
    private readonly int _turnsPerCycle;
    private readonly object _stateLock = new();
    private readonly ExecutorState _state = new();
    private readonly Timer _timer;

    private Action<ExecutorState, CalculationType>? _calculationEvent;
    private bool _isDisposed;

    /// <summary>
    /// Initializes a new instance of the Executor
    /// </summary>
    /// <param name="tickInterval">The interval between ticks in milliseconds</param>
    /// <exception cref="ArgumentException">Thrown when tickInterval is less than 1</exception>
    public Executor(int tickInterval = 32)
    {
        if (tickInterval < MinimumTickInterval)
            throw new ArgumentException($"Tick interval must be at least {MinimumTickInterval}ms", nameof(tickInterval));

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
    public virtual void Start(Action<ExecutorState, CalculationType> onTickCalculation)
    {
        _calculationEvent = onTickCalculation ?? throw new ArgumentNullException(nameof(onTickCalculation));
        _timer.Enabled = true;
        _state.IsPaused = false;
    }

    private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        if (_state.IsPaused) return;

        lock (_stateLock)
        {
            TickUpdate();
        }
    }

    private void TickUpdate()
    {
        var currentTickCount = _state.IncrementTickCounter();

        _state.IncrementTickTotal();

        if (currentTickCount >= _ticksPerTurn)
        {
            TurnUpdate();
            return;
        }

        TickCalculation(CalculationType.Tick);
    }

    private void TurnUpdate()
    {
        _state.ResetTickCounter();
        
        if (_state.TurnCounter >= _turnsPerCycle)
        {
            CycleUpdate();
        }
        else
        {
            TickCalculation(CalculationType.Turn);
            _state.IncrementTurnCounter();
        }
    }

    private void CycleUpdate()
    {
        _state.ResetTurnCounter();
        TickCalculation(CalculationType.Cycle);
        _state.IncrementCycleCounter();
    }

    private void TickCalculation(CalculationType type)
    {
        _state.IsPaused = true;
        _calculationEvent?.Invoke(_state, type);
        _state.IsPaused = false;
    }

    /// <summary>
    /// Stops the execution loop
    /// </summary>
    public virtual void Stop()
    {
        if (_isDisposed) return;
        _timer.Enabled = false;
    }

    public void Dispose()
    {
        if (_isDisposed) return;
        
        _timer.Dispose();
        _isDisposed = true;
        GC.SuppressFinalize(this);
    }
}
