using DeepSpaceSaga.Common.Abstractions.Entities.Commands;

namespace DeepSpaceSaga.Server;

public class LocalGameServer(ISchedulerService schedulerService, ISessionContextService sessionContext, IProcessingService processingService) : IGameServer
{
    public event Action<GameSessionDto>? OnTurnExecute;
    
    private GameSessionDto _gameSessionDto = new GameSessionDto();

    private static readonly ILog Logger = LogManager.GetLogger(Settings.LoggerRepository, typeof(LocalGameServer));
    
    private readonly ISchedulerService _flowManager = schedulerService ?? throw new ArgumentNullException(nameof(schedulerService));
    private readonly ISessionContextService _sessionContext = sessionContext ?? throw new ArgumentNullException(nameof(sessionContext));
    private readonly IProcessingService _processingService = processingService ?? throw new ArgumentNullException(nameof(processingService));

    public void TurnExecution(ISessionInfoService info, CalculationType type)
    {
        Logger?.Debug($"GameSessionMap {info.ToString()}");

        _gameSessionDto = SessionTurnFinalization(info, _processingService.Process(_sessionContext));
        
        OnTurnExecute?.Invoke(_gameSessionDto);
    }

    public async Task AddCommand(ICommand command)
    {
        await _sessionContext.GameSession.AddCommand(command);
    }

    public void RemoveCommand(Guid commandId)
    {
        _sessionContext.GameSession.RemoveCommand(commandId);
    }
    
    public void SetGameSpeed(int speed)
    {
        _sessionContext.SessionInfo.SetSpeed(speed);
        SessionResume();
        RefreshGameSessionDto();
    }

    public GameSessionDto GetSessionContextDto()
    {
        return _gameSessionDto;
    }

    private GameSessionDto SessionTurnFinalization(ISessionInfoService sessionInfo, GameSessionDto sessionDto)
    {
        var turn = sessionInfo.IncrementTurn(); 
        Logger?.Debug($"GameSessionMap {sessionInfo.Turn}");
        Console.WriteLine($"[SessionTurnFinalization] Finish turn processing for session {sessionDto.Id} Turn: {sessionDto.State.Turn}");

        return GameSessionMapper.ToDto(_sessionContext);
    }
    
    public void SessionStart(GameSession session)
    {
        _sessionContext.GameSession = session ?? throw new ArgumentNullException(nameof(session));
        _sessionContext.GameSession.Changed += GameSession_Changed;
        _sessionContext.SessionInfo.Reset();
        _sessionContext.Metrics.Reset();
        _flowManager.SessionStart(TurnExecution);
    }

    private void GameSession_Changed(object? sender, EventArgs e)
    {
        RefreshGameSessionDto();
    }

    private void RefreshGameSessionDto()
    {
        _gameSessionDto = GameSessionMapper.ToDto(_sessionContext);
        OnTurnExecute?.Invoke(_gameSessionDto);
    }

    public void SessionPause()
    {
        _flowManager.SessionPause();
        RefreshGameSessionDto();
    }

    public void SessionResume() 
    { 
        _flowManager.SessionResume();
        RefreshGameSessionDto();
    }

    public void SessionStop()
    {
        _flowManager.SessionStop();
        RefreshGameSessionDto();
    }

}
