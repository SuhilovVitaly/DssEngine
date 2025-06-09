using DeepSpaceSaga.Common.Abstractions.Dto.Ui;

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
    Task AddCommand(ICommand command);
    Task SaveGame(string saveName);
    Task LoadGame(string saveName);
}
