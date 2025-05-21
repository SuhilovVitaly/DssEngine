namespace DeepSpaceSaga.Server;

public class LocalGameServer(ISchedulerService schedulerService, ISessionContextService sessionContext): IGameServer
{
    public event Action<GameSessionDto>? OnTurnExecute;
    public GameSession GameSession { get; private set; }
    private GameSessionDto _gameSessionDto;

    private static readonly ILog Logger = LogManager.GetLogger(Settings.LoggerRepository, typeof(LocalGameServer));
    
    private readonly ISchedulerService _flowManager = schedulerService ?? throw new ArgumentNullException(nameof(schedulerService));
    private readonly ISessionContextService _sessionContext = sessionContext ?? throw new ArgumentNullException(nameof(sessionContext));

    public void TurnExecution(ISessionInfoService info, CalculationType type)
    {
        Logger?.Debug($"GameSessionMap {info.ToString()}");

        _gameSessionDto = SessionTurnFinalization(info, BaseProcessing.Process(GameSession));
        
        OnTurnExecute?.Invoke(_gameSessionDto);
    }

    public void AddCommand(Command command)
    {
        GameSession.AddCommand(command);
    }

    public void RemoveCommand(Guid commandId)
    {
        GameSession.RemoveCommand(commandId);
    }
    
    public GameSessionDto GetSessionContextDto()
    {
        return _gameSessionDto;
    }

    private GameSessionDto SessionTurnFinalization(ISessionInfoService sessionInfo, GameSessionDto sessionDto)
    {
        var turn = sessionInfo.IncrementTurn(); 
        Logger?.Debug($"GameSessionMap {sessionInfo.Turn}");
        Console.WriteLine($"[SessionTurnFinalization] Finish turn processing for session {sessionDto.Id} Turn: {sessionDto.Turn}");

        sessionDto.Turn = turn;

        return sessionDto;
    }
    
    public void SessionStart(GameSession session)
    {
        GameSession = session ?? throw new ArgumentNullException(nameof(session));
        GameSession.Changed += GameSession_Changed;
        _flowManager.SessionStart(TurnExecution);
    }

    private void GameSession_Changed(object? sender, EventArgs e)
    {
        _gameSessionDto = GameSessionMapper.ToDto(GameSession);
    }

    public void SessionPause() => _flowManager.SessionPause();

    public void SessionResume() => _flowManager.SessionResume();

    public void SessionStop() => _flowManager.SessionStop();

}
