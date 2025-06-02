namespace DeepSpaceSaga.Common.Abstractions.Services;

public interface IGameServer
{
    event Action<GameSessionDto>? OnTurnExecute;
    
    GameSessionDto GetSessionContextDto();
    void SessionStart(GameSession session);
    void SessionPause();
    void SessionResume();
    void SessionStop();
    void SetGameSpeed(int speed);
    void AddCommand(ICommand command);
    void RemoveCommand(Guid commandId);
}
