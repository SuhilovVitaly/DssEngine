namespace DeepSpaceSaga.UI;

public class GameManager : IGameManager
{
    public event Action<GameSessionDto>? OnUpdateGameData;

    public IScreensService Screens { get; set; }

    private readonly IGameServer _gameServer;

    public GameManager(IGameServer gameServer, IScreensService screenManager) 
    {
        _gameServer = gameServer;

        _gameServer.OnTurnExecute += UpdateGameData;

        Screens = screenManager;
    }

    public void SetGameSpeed(int speed)
    {
        _gameServer.SetGameSpeed(speed);
    }

    public void SessionStart()
    {
        _gameServer.SessionStart(new GameSession());
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
}
