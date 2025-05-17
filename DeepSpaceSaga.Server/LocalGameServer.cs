namespace DeepSpaceSaga.Server;

public class LocalGameServer(ISchedulerService schedulerService, ISessionContextService sessionContext): IGameServer
{
    public event Action<GameSessionDTO>? OnTurnExecute;

    private static readonly ILog Logger = LogManager.GetLogger(Settings.LoggerRepository, typeof(LocalGameServer));
    private readonly ISchedulerService _flowManager = schedulerService ?? throw new ArgumentNullException(nameof(schedulerService));
    private readonly ISessionContextService _sessionContext = sessionContext ?? throw new ArgumentNullException(nameof(sessionContext));

    public void TurnExecution(ISessionInfoService info, CalculationType type)
    {
        Logger?.Debug($"GameSessionMap {info.ToString()}");
        OnTurnExecute?.Invoke(GameSessionMap(info));
    }

    private GameSessionDTO GameSessionMap(ISessionInfoService sessionInfo)
    {
        var turn = sessionInfo.IncrementTurn(); 
        Logger?.Debug($"GameSessionMap {sessionInfo.Turn}");
        
        return new GameSessionDTO 
        { 
            Id = Guid.NewGuid(), 
            Turn = turn,
            FlowState = sessionInfo.ToString(),
            SpaceMap = []
        };
    }

    public void SessionStart()
    {
        _flowManager.SessionStart(TurnExecution);
    }

    public void SessionPause() => _flowManager.SessionPause();

    public void SessionResume() => _flowManager.SessionResume();

    public void SessionStop() => _flowManager.SessionStop();

}
