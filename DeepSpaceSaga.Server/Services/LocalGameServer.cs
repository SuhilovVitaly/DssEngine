using DeepSpaceSaga.Common.Abstractions.Entities.Characters;
using DeepSpaceSaga.Common.Extensions;

namespace DeepSpaceSaga.Server.Services;

public class LocalGameServer(
    ISchedulerService schedulerService, 
    ISessionContextService sessionContext, 
    IProcessingService processingService,
    ISaveLoadService saveLoadService) : IGameServer
{
    public event Action<GameSessionDto>? OnTurnExecute;
    
    private GameSessionDto _gameSessionDto = new();

    private static readonly ILog? Logger = GetLoggerSafe();
    
    private readonly ISchedulerService _flowManager = schedulerService ?? throw new ArgumentNullException(nameof(schedulerService));
    private ISessionContextService _sessionContext = sessionContext ?? throw new ArgumentNullException(nameof(sessionContext));
    private readonly IProcessingService _processingService = processingService ?? throw new ArgumentNullException(nameof(processingService));
    private readonly ISaveLoadService _saveLoadService = saveLoadService ?? throw new ArgumentNullException(nameof(saveLoadService));

    public void TurnExecution(CalculationType type)
    {
        Logger?.Debug($"GameSessionMap {_sessionContext.SessionInfo.ToString()}");

        _processingService.Process(_sessionContext);
        _gameSessionDto = SessionTurnFinalization(_sessionContext.SessionInfo);
        
        OnTurnExecute?.Invoke(_gameSessionDto);
    }

    public async Task AddCommand(ICommand command)
    {     
        _sessionContext.Metrics.Add(MetricsServer.ServerCommandReceived);
            
        await _sessionContext.GameSession.AddCommand(command);

        if (command.IsPauseProcessed)
        {
            _processingService.PauseProcess(_sessionContext);
            _gameSessionDto = SessionTurnFinalization(_sessionContext.SessionInfo);
        }
    }

    public async Task<GameActionEventDto> ProcessDialogChoice(ICommand command)
    {
        _sessionContext.Metrics.Add(MetricsServer.ServerCommandReceived);

        if (command.IsPauseProcessed)
        {
            return new ProcessingDialogHandler().Execute(sessionContext, command);
        }

        return null;
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

    private GameSessionDto SessionTurnFinalization(ISessionInfoService sessionInfo)
    {
        sessionInfo.IncrementTurn(); 
        Logger?.Debug($"GameSessionMap {sessionInfo.Turn}");
        Console.WriteLine($"[SessionTurnFinalization] Finish turn processing for session {sessionInfo.Id} Turn: {sessionInfo.Turn}");

        return GameSessionMapper.ToDto(_sessionContext);
    }
    
    public void SessionStart(GameSession session)
    {
        _sessionContext.GameSession = session ?? throw new ArgumentNullException(nameof(session));
        _sessionContext.GameSession.Changed += GameSession_Changed;
        _sessionContext.SessionInfo.Reset();
        _sessionContext.Metrics.Reset();

        _processingService.ScenarioStartProcess(_sessionContext);
        _gameSessionDto = SessionTurnFinalization(_sessionContext.SessionInfo);

        _flowManager.SessionStart(TurnExecution);

        _gameSessionDto = GameSessionMapper.ToDto(_sessionContext);
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
    
    private static ILog? GetLoggerSafe()
    {
        try
        {
            return LogManager.GetLogger(Settings.LoggerRepository, typeof(LocalGameServer));
        }
        catch
        {
            return null; // No logging in tests
        }
    }

    public Task SaveGame(string saveName)
    {
        _saveLoadService.Save(sessionContext, saveName);

        return Task.CompletedTask;
    }

    public Task LoadGame(string saveName)
    {
        _sessionContext = _saveLoadService.Load(saveName);

        _gameSessionDto = SessionTurnFinalization(_sessionContext.SessionInfo);

        OnTurnExecute?.Invoke(_gameSessionDto);

        return Task.CompletedTask;
    }

    public GameActionEventDto GetGameActionEvent(string key)
    {
        var dialog = sessionContext.GameSession.Dialogs.GetDialog(key);

            if (dialog != null)
            {
                var gameActionEvent = new GameActionEvent
                {
                    Key = dialog.Key,
                    Dialog = dialog,
                    Type = dialog.Type,
                    ConnectedDialogs = sessionContext.GameSession.Dialogs.GetConnectedDialogs(dialog),
                };

                return GameActionEventMapper.ToDto(gameActionEvent);
            }

        return null;
    }

    public ICharacter GetMainCharacter()
    {
        var mainCharacter = _sessionContext.GameSession.GetPlayerSpaceShip().GetCrewMember(1);

        return mainCharacter;
    }

    public ICharacter GetCharacter(string id)
    {
        return null;
    }
}
