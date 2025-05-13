namespace DeepSpaceSaga.Common.Abstractions.Services;

public interface IGameServer
{
    event Action<GameSessionDTO>? OnTurnExecute;

    void SessionStart();
    void SessionPause();
    void SessionResume();
    void SessionStop();
}
