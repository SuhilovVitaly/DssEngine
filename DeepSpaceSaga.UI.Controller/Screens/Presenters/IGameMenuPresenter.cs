namespace DeepSpaceSaga.UI.Controller.Screens.Presenters;

/// <summary>
/// Interface for Game Menu Presenter handling View coordination
/// </summary>
public interface IGameMenuPresenter
{
    /// <summary>
    /// Event fired when an error occurs
    /// </summary>
    event EventHandler<string>? ErrorOccurred;

    /// <summary>
    /// Initializes the presenter
    /// </summary>
    Task InitializeAsync();

    /// <summary>
    /// Handles resume game command
    /// </summary>
    Task HandleResumeGameAsync();

    /// <summary>
    /// Handles save game command
    /// </summary>
    Task HandleSaveGameAsync();

    /// <summary>
    /// Handles load game command
    /// </summary>
    Task HandleLoadGameAsync();

    /// <summary>
    /// Handles go to main menu command
    /// </summary>
    Task HandleGoToMainMenuAsync();

    /// <summary>
    /// Handles menu closing (resume game)
    /// </summary>
    Task HandleMenuClosingAsync();

    /// <summary>
    /// Checks if the presenter is busy with an operation
    /// </summary>
    bool IsBusy { get; }
} 