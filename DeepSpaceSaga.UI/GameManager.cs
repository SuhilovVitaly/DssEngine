namespace DeepSpaceSaga.UI;

public class GameManager
{
    public event Action<GameSessionDTO>? OnUpdateGameData;

    public IScreensService Screens { get; set; }
    public IGameContextService GameContext { get; }
    private readonly IGameServer _gameServer;

    public GameManager(IGameServer gameServer, IScreensService screenManager, IGameContextService gameContext) 
    {
        _gameServer = gameServer;

        _gameServer.OnTurnExecute += UpdateGameData;

        Screens = screenManager;
        GameContext = gameContext;
    }

    public void SessionStart()
    {
        _gameServer.SessionStart();
        Screens.ShowTacticalMapScreen();
    }

    public void SessionPause() => _gameServer.SessionPause();

    public void SessionResume() => _gameServer.SessionResume();

    public void SessionStop() => _gameServer.SessionStop();

    private void UpdateGameData(GameSessionDTO session)
    {
        OnUpdateGameData?.Invoke(session);
    }
}
