namespace DeepSpaceSaga.UI;

public class GameManager
{
    public event Action<GameSessionDTO>? OnUpdateGameData;
    private readonly IGameServer _gameServer;

    public GameManager(IGameServer gameServer) 
    {
        _gameServer = gameServer;

        _gameServer.OnTurnExecute += UpdateGameData;
    }

    public void SessionStart() => _gameServer.SessionStart();

    public void SessionPause() => _gameServer.SessionPause();

    public void SessionResume() => _gameServer.SessionResume();

    public void SessionStop() => _gameServer.SessionStop();

    private void UpdateGameData(GameSessionDTO session)
    {
        OnUpdateGameData?.Invoke(session);
    }
}
