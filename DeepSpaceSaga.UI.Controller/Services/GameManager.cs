using DeepSpaceSaga.Common.Abstractions.Dto.Ui;

namespace DeepSpaceSaga.UI.Controller.Services;

public class GameManager : IGameManager
{
    public event Action<GameSessionDto>? OnUpdateGameData;
    public IGenerationTool GenerationTool { get; set; }
    public IScreensService Screens { get; set; }
    public IScreenInfo ScreenInfo { get; set; }
    public IOuterSpaceService OuterSpace { get; set; }
    public SpaceMapPoint TacticalMapMousePosition { get; private set; }

    private readonly IGameServer _gameServer;

    public GameManager(IGameServer gameServer, IScreensService screenManager, IGenerationTool generationTool, 
        IOuterSpaceService outerSpace, IScreenResolution screenResolution) 
    {
        _gameServer = gameServer;

        _gameServer.OnTurnExecute += UpdateGameData;

        Screens = screenManager;
        ScreenInfo = new ScreenParameters(screenResolution);
        GenerationTool = generationTool;
        OuterSpace = outerSpace;
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
    }

    public void SessionStart()
    {
        _gameServer.SessionStart(ScenarioGenerator.DefaultScenario(GenerationTool));
        OuterSpace = new OuterSpaceService();
        Screens.ShowTacticalMapScreen();
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
        OnUpdateGameData?.Invoke(session);
    }

    public void TacticalMapMouseMove(SpaceMapPoint coordinatesGame, SpaceMapPoint coordinatesScreen)
    {
        ScreenInfo.SetMousePosition(coordinatesScreen);

        TacticalMapMousePosition = coordinatesGame;
    }

    public void TacticalMapMouseClick(SpaceMapPoint coordinates)
    {

    }
    public void TacticalMapLeftMouseClick(SpaceMapPoint mouseLocation)
    {

    }
}
