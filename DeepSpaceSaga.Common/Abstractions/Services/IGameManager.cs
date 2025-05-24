namespace DeepSpaceSaga.Common.Abstractions.Services;

/// <summary>
/// Interface for Game Manager handling core game operations
/// </summary>
public interface IGameManager
{
    /// <summary>
    /// Event fired when game data is updated
    /// </summary>
    event Action<GameSessionDto>? OnUpdateGameData;

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
} 