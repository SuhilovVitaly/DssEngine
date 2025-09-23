namespace DeepSpaceSaga.Common.Abstractions.Services;

public interface IGameServer
{
    event Action<GameSessionDto>? OnTurnExecute;
    GameActionEventDto GetGameActionEvent(string key);
    GameSessionDto GetSessionContextDto();
    void SessionStart(GameSession session);
    void SessionPause();
    void SessionResume();
    void SessionStop();
    void SetGameSpeed(int speed);
    Task AddCommand(ICommand command);
    Task<GameActionEventDto> ProcessDialogChoice(ICommand command);
    Task SaveGame(string saveName);
    Task LoadGame(string saveName);
    ICharacter GetMainCharacter();
    ICharacter GetCharacter(int id);
}
