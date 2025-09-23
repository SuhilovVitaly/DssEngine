using DeepSpaceSaga.Common.Abstractions.Dto.Ui;

namespace DeepSpaceSaga.Common.Abstractions.Services;

/// <summary>
/// Interface for Game Manager handling core game operations
/// </summary>
public interface IGameManager
{
    GameSessionDto GameSession();
    /// <summary>
    /// Event fired when game data is updated
    /// </summary>
    event Action<GameSessionDto>? OnUpdateGameData;

    /// <summary>
    /// Generation tool for creating game content
    /// </summary>
    IGenerationTool GenerationTool { get; set; }

    /// <summary>
    /// Screens service for managing UI screens
    /// </summary>
    IScreensService Screens { get; set; }

    /// <summary>
    /// Screen information and parameters
    /// </summary>
    IScreenInfo ScreenInfo { get; set; }

    /// <summary>
    /// Outer space service for space-related operations
    /// </summary>
    IOuterSpaceService OuterSpace { get; set; }

    ILocalizationService Localization { get; }

    /// <summary>
    /// Sets the game speed
    /// </summary>
    /// <param name="speed">Game speed value</param>
    void SetGameSpeed(int speed);

    /// <summary>
    /// Starts a new game session
    /// </summary>
    void SessionStart();

    /// <summary>
    /// Pauses the current game session
    /// </summary>
    void SessionPause();

    /// <summary>
    /// Resumes the current game session
    /// </summary>
    void SessionResume();

    /// <summary>
    /// Stops the current game session
    /// </summary>
    void SessionStop();

    /// <summary>
    /// Shows the tactical map screen
    /// </summary>
    void ShowTacticalMapScreen();

    /// <summary>
    /// Shows the main menu screen
    /// </summary>
    void ShowMainMenuScreen();

    void CommandExecute(ICommand command);

    void DialogCommandExecute(ICommand command);

    Task<int> SaveGame(string savename);
    Task<int> LoadGame(string savename);
    void GameEventInvoke(GameActionEventDto gameEvent);

    void Navigation(CommandTypes command);

    GameActionEventDto GetGameActionEvent(string key);

    ICharacter GetMainCharacter();

    ICharacter GetCharacter(string id);
} 