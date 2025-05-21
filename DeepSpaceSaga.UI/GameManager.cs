namespace DeepSpaceSaga.UI;

public class GameManager
{
    public event Action<GameSessionDto>? OnUpdateGameData;

    public IScreensService Screens { get; set; }
    public IGameContextService GameContext { get; }
    private readonly IGameServer _gameServer;

    public GameManager(IGameServer gameServer, IScreensService screenManager, IGameContextService gameContext) 
    {
        _gameServer = gameServer;

        _gameServer.OnTurnExecute += OnUpdateGameData;

        Screens = screenManager;
        GameContext = gameContext;
    }

    public void SessionStart()
    {
        _gameServer.SessionStart(new GameSession());
        Screens.ShowTacticalMapScreen();
    }

    public void SessionPause() => _gameServer.SessionPause();

    public void SessionResume() => _gameServer.SessionResume();

    public void SessionStop() => _gameServer.SessionStop();

    private void UpdateGameData(GameSessionDto session)
    {
        OnUpdateGameData?.Invoke(session);
    }
}
