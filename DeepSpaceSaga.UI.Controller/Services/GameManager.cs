namespace DeepSpaceSaga.UI.Controller.Services;

public class GameManager : IGameManager
{
    public event Action<GameSessionDto>? OnUpdateGameData;
    public IGenerationTool GenerationTool { get; set; }
    public IScreensService Screens { get; set; }
    public IScreenInfo ScreenInfo { get; set; }
    public IOuterSpaceService OuterSpace { get; set; }
    public ILocalizationService Localization { get; private set; }

    private GameSessionDto _gameSessionDto;
    private readonly IScenarioService _scenarioService;
    private readonly IGameServer _gameServer;

    public GameManager(IGameServer gameServer, IScreensService screenManager, IGenerationTool generationTool, 
        IOuterSpaceService outerSpace, IScreenResolution screenResolution, IScenarioService scenarioService,
        ILocalizationService localizationService) 
    {
        _gameServer = gameServer;
        _scenarioService = scenarioService;
        _gameServer.OnTurnExecute += UpdateGameData;

        Localization = localizationService;

        Screens = screenManager;
        ScreenInfo = new ScreenParameters(screenResolution);
        GenerationTool = generationTool;
        OuterSpace = outerSpace;        
    }

    public GameSessionDto GameSession()
    {
        return _gameSessionDto;
    }

    public async Task<int> SaveGame(string savename)
    {
        await _gameServer.SaveGame(savename);

        return 0;
    }

    public async Task<int> LoadGame(string savename)
    {
        await _gameServer.LoadGame(savename);

        return 0;
    }

    public void SetGameSpeed(int speed)
    {
        _gameServer.SetGameSpeed(speed);
    }

    public async void CommandExecute(ICommand command)
    {
        await _gameServer.AddCommand(command);

        //_gameSessionDto = _gameServer.GetSessionContextDto();
    }

    public async void DialogCommandExecute(ICommand command)
    {
        await _gameServer.AddCommand(command);

        //_gameSessionDto = _gameServer.GetSessionContextDto();
    }

    public void SessionStart()
    {
        _gameServer.SessionStart(_scenarioService.GetScenario());
        OuterSpace = new OuterSpaceService();
        Screens.ShowTacticalMapScreen();
        _gameSessionDto = _gameServer.GetSessionContextDto();
        _gameSessionDto.State = new GameStateDto();
        OnUpdateGameData?.Invoke(_gameSessionDto);
    }

    public void SessionPause() => _gameServer.SessionPause();

    public void SessionResume() => _gameServer.SessionResume();

    public void SessionStop()
    {
        _gameServer.SessionStop();
        Screens.CloseTacticalMapScreen();
    }

    public void ShowTacticalMapScreen()
    {
        Screens.ShowTacticalMapScreen();
    }

    public void ShowMainMenuScreen()
    {
        Screens.ShowGameMenuScreen();
    }

    private void UpdateGameData(GameSessionDto session)
    {
        _gameSessionDto = session;
        OnUpdateGameData?.Invoke(_gameSessionDto);
    }

    public void GameEventInvoke(GameActionEventDto gameEvent)
    {
        SessionPause();

        //Screens.ShowDialogScreen(gameEvent);
        Screens.TacticalMap.StartDialog(gameEvent);
    }


    public void Navigation(CommandTypes command)
    {
        if (_gameSessionDto.State.IsPaused) return;

        // TurnLeft  TurnRight  IncreaseShipSpeed  DecreaseShipSpeed

        CommandExecute(new Command
        {
            Category = Common.Abstractions.Entities.Commands.CommandCategory.Navigation,
            Type = command,
            IsOneTimeCommand = false,
        });
    }

    public GameActionEventDto GetGameActionEvent(string key)
    {
        return _gameServer.GetGameActionEvent(key);
    }
}
