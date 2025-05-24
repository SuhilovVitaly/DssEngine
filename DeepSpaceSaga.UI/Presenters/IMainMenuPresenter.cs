using DeepSpaceSaga.UI.Controller.Models;

namespace DeepSpaceSaga.UI.Presenters;

/// <summary>
/// Interface for Main Menu Presenter handling View coordination
/// </summary>
public interface IMainMenuPresenter
{
    /// <summary>
    /// Event fired when game info is updated
    /// </summary>
    event EventHandler<GameInfoModel>? GameInfoUpdated;

    /// <summary>
    /// Event fired when an error occurs
    /// </summary>
    event EventHandler<string>? ErrorOccurred;

    /// <summary>
    /// Initializes the presenter and loads initial data
    /// </summary>
    Task InitializeAsync();

    /// <summary>
    /// Handles new game command
    /// </summary>
    Task HandleNewGameAsync();

    /// <summary>
    /// Handles load game command
    /// </summary>
    Task HandleLoadGameAsync();

    /// <summary>
    /// Handles exit application command
    /// </summary>
    Task HandleExitAsync();

    /// <summary>
    /// Refreshes game information
    /// </summary>
    Task RefreshGameInfoAsync();

    /// <summary>
    /// Checks if the presenter is busy with an operation
    /// </summary>
    bool IsBusy { get; }
} 