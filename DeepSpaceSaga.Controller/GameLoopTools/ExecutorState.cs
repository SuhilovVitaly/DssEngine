namespace DeepSpaceSaga.Controller.GameLoopTools;

public class ExecutorState
{
    private volatile int _tickTotal = 0;
    private volatile int _tickCounter = 0;
    private volatile int _turnCounter = 0;
    private volatile int _cycleCounter = 1;

    public int TickTotal => _tickTotal;
    public int TickCounter => _tickCounter;
    public int TurnCounter => _turnCounter;
    public int CycleCounter => _cycleCounter;

    public bool IsPaused { get; set; } = true;

    public int IncrementTickTotal() => Interlocked.Increment(ref _tickTotal);
    public int IncrementTickCounter() => Interlocked.Increment(ref _tickCounter);
    public void ResetTickCounter() => Interlocked.Exchange(ref _tickCounter, 0);
    public int IncrementTurnCounter() => Interlocked.Increment(ref _turnCounter);
    public void ResetTurnCounter() => Interlocked.Exchange(ref _turnCounter, 0);
    public int IncrementCycleCounter() => Interlocked.Increment(ref _cycleCounter);

    public override string ToString() => $"[{CycleCounter:D3}-{TurnCounter:D3}-{TickCounter:D3}-{TickTotal:D3}]";
}
