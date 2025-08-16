namespace DeepSpaceSaga.Common.Abstractions.UI.Screens;

/// <summary>
/// Interface for Game Menu Controller handling in-game menu business logic
/// </summary>
public interface IGameMenuController
{
    Task LoadAsync();
    /// <summary>
    /// Resumes the current game
    /// </summary>
    /// <returns>Task representing the asynchronous operation</returns>
    Task ResumeGameAsync();

    /// <summary>
    /// Saves the current game
    /// </summary>
    /// <returns>Task representing the asynchronous operation</returns>
    Task SaveGameAsync();

    /// <summary>
    /// Loads a saved game
    /// </summary>
    /// <returns>Task representing the asynchronous operation</returns>
    Task LoadGameAsync();

    /// <summary>
    /// Returns to the main menu
    /// </summary>
    /// <returns>Task representing the asynchronous operation</returns>
    Task GoToMainMenuAsync();
} 