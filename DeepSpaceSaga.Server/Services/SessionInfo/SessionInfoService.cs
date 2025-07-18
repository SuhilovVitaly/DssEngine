namespace DeepSpaceSaga.Server.Services.SessionInfo;

public class SessionInfoService : ISessionInfoService
{
    public SessionInfoService()
    {
        Id = Guid.NewGuid();
    }

    public SessionInfoService(int cycleCounter, int turnCounter, int tickCounter) : this()
    {
        _cycleCounter = cycleCounter;
        _turnCounter = turnCounter;
        _tickCounter = tickCounter;
    }

    public SessionInfoService(int cycleCounter, int turnCounter, int tickCounter, int tickTotal) : this(cycleCounter, turnCounter, tickCounter)
    {
        _tickTotal = tickTotal;
    }

    public void Reset()
    {
        Interlocked.Exchange(ref _cycleCounter, 0);
        Interlocked.Exchange(ref _turnCounter, 0);
        Interlocked.Exchange(ref _tickCounter, 0);
    }

    public Guid Id { get; set; }
    public int Speed { get; set; } = 1;

    public void SetSpeed(int speed)
    {
        Speed = speed;
        IsPaused = false;
        IsCalculationInProgress = false;
    }

    private volatile int _turn;
    public int Turn
    {
        get => _turn;
        set => Interlocked.Exchange(ref _turn, value);
    }

    public int IncrementTurn() => Interlocked.Increment(ref _turn);

    private volatile int _tickTotal = 0;
    private volatile int _tickCounter = 0;
    private volatile int _turnCounter = 0;
    private volatile int _cycleCounter = 1;

    public int TickTotal => _tickTotal;
    public int TickCounter => _tickCounter;
    public int TurnCounter => _turnCounter;
    public int CycleCounter => _cycleCounter;

    public bool IsCalculationInProgress { get; set; } = true;
    public bool IsPaused { get; set; }
    public int IncrementTickTotal() => Interlocked.Increment(ref _tickTotal);
    public int IncrementTickCounter() => Interlocked.Increment(ref _tickCounter);
    public void ResetTickCounter() => Interlocked.Exchange(ref _tickCounter, 0);
    public int IncrementTurnCounter() => Interlocked.Increment(ref _turnCounter);
    public void ResetTurnCounter() => Interlocked.Exchange(ref _turnCounter, 0);
    public int IncrementCycleCounter() => Interlocked.Increment(ref _cycleCounter);

    public override string ToString() => $"[{CycleCounter:D3}-{TurnCounter:D3}-{TickCounter:D3}-{TickTotal:D3}]";
}