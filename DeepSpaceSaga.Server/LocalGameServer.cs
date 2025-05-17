namespace DeepSpaceSaga.Server;

public class LocalGameServer : IGameServer
{
    public event Action<GameSessionDTO>? OnTurnExecute;

    private static readonly ILog Logger = LogManager.GetLogger(Settings.LoggerRepository, typeof(LocalGameServer));

    private readonly ISchedulerService _flowManager;
    private readonly ISessionContext _sessionContext;

    public LocalGameServer(ISchedulerService gameFlowService, ISessionContext sessionContext)
    {
        _sessionContext = sessionContext ?? throw new ArgumentNullException(nameof(sessionContext));
        _flowManager = gameFlowService ?? throw new ArgumentNullException(nameof(gameFlowService));

        if (TurnExecution == null)
        {
            throw new InvalidOperationException("TurnExecution method must be defined");
        }
    }    

    public void TurnExecution(ISessionInfoService info, CalculationType type)
    {
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
