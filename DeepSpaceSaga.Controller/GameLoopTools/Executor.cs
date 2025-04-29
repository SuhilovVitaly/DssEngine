using DeepSpaceSaga.Common.Implementation.GameLoop;
using System.Timers;
using Timer = System.Timers.Timer;

namespace DeepSpaceSaga.Controller.GameLoopTools;

public class Executor : IDisposable
{
    private const int DefaultTicksPerTurn = 10;
    private const int DefaultTurnsPerCycle = 10;

    private readonly int _ticksPerTurn;
    private readonly int _turnsPerCycle;

    private Timer _timer;
    private readonly ExecutorState _state = new();

    private Action<ExecutorState, CalculationType> _calculationEvent;

    private readonly object _stateLock = new();

    public Executor(int tickInterval = 32)
    {
        _ticksPerTurn = DefaultTicksPerTurn;
        _turnsPerCycle = DefaultTurnsPerCycle;
        _timer = new Timer(tickInterval);
        _timer.Elapsed += OnTimerElapsed;
        _timer.AutoReset = true;
        _timer.Enabled = false;
    }

    public void Start(Action<ExecutorState, CalculationType> onTickCalculation)
    {
        _calculationEvent = onTickCalculation;
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
        var currentTurnCount = _state.IncrementTurnCounter();

        if (currentTurnCount >= _turnsPerCycle)
        {
            CycleUpdate();
        }
        else
        {
            TickCalculation(CalculationType.Turn);
        }
    }

    private void CycleUpdate()
    {
        _state.ResetTurnCounter();
        _state.IncrementCycleCounter();
        TickCalculation(CalculationType.Cycle);
    }

    private void TickCalculation(CalculationType type)
    {
        _state.IsPaused = true;
        _calculationEvent(_state, type);
        _state.IsPaused = false;
    }

    public void Stop()
    {
        _timer.Enabled = false;
    }

    public void Dispose()
    {
        _timer?.Dispose();
        _timer = null;
    }
}
