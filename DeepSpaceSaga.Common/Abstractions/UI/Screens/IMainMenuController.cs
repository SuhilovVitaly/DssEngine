namespace DeepSpaceSaga.Common.Abstractions.UI.Screens;

/// <summary>
/// Interface for Main Menu Controller handling business logic and coordination
/// </summary>
public interface IMainMenuController
{
    /// <summary>
    /// Starts a new game session
    /// </summary>
    /// <returns>Task representing the asynchronous operation</returns>
    Task StartNewGameAsync();

    /// <summary>
    /// Loads an existing game
    /// </summary>
    /// <returns>Task representing the asynchronous operation</returns>
    Task LoadGameAsync();

    /// <summary>
    /// Exits the application
    /// </summary>
    /// <returns>Task representing the asynchronous operation</returns>
    Task ExitApplicationAsync();

    /// <summary>
    /// Checks if loading games is available
    /// </summary>
    /// <returns>True if load game functionality is available</returns>
    Task<bool> IsLoadGameAvailableAsync();

    /// <summary>
    /// Gets game information for display
    /// </summary>
    /// <returns>Game information model</returns>
    Task<GameInfoModel> GetGameInfoAsync();
} 