using DeepSpaceSaga.Common.Abstractions.UI.Screens;
using log4net;

namespace DeepSpaceSaga.UI.Controller.Screens.Presenters;

/// <summary>
/// Presenter for Game Menu screen handling View coordination and UI logic
/// </summary>
public class GameMenuPresenter : IGameMenuPresenter
{
    private readonly IGameMenuController _controller;
    private static readonly ILog Logger = LogManager.GetLogger("ControllerAppRepository", typeof(GameMenuPresenter));
    
    private bool _isBusy = false;

    public GameMenuPresenter(IGameMenuController controller)
    {
        _controller = controller ?? throw new ArgumentNullException(nameof(controller));
    }

    #region Events

    /// <summary>
    /// Event fired when an error occurs
    /// </summary>
    public event EventHandler<string>? ErrorOccurred;

    #endregion

    #region Properties

    /// <summary>
    /// Checks if the presenter is busy with an operation
    /// </summary>
    public bool IsBusy => _isBusy;

    #endregion

    #region Public Methods

    /// <summary>
    /// Initializes the presenter
    /// </summary>
    public async Task InitializeAsync()
    {
        try
        {
            Logger.Info("Initializing Game Menu presenter");
            await Task.CompletedTask;
            Logger.Debug("Game Menu presenter initialized successfully");
        }
        catch (Exception ex)
        {
            Logger.Error($"Failed to initialize Game Menu presenter: {ex.Message}", ex);
            OnErrorOccurred($"Failed to initialize menu: {ex.Message}");
        }
    }

    /// <summary>
    /// Handles resume game command
    /// </summary>
    public async Task HandleResumeGameAsync()
    {
        if (_isBusy) return;
        
        try
        {
            _isBusy = true;
            Logger.Info("Resume game requested");
            await _controller.ResumeGameAsync();
            Logger.Debug("Resume game completed successfully");
        }
        catch (Exception ex)
        {
            Logger.Error($"Failed to resume game: {ex.Message}", ex);
            OnErrorOccurred($"Failed to resume game: {ex.Message}");
        }
        finally
        {
            _isBusy = false;
        }
    }

    /// <summary>
    /// Handles save game command
    /// </summary>
    public async Task HandleSaveGameAsync()
    {
        if (_isBusy) return;
        
        try
        {
            _isBusy = true;
            Logger.Info("Save game requested");
            await _controller.SaveGameAsync();
            Logger.Debug("Save game completed successfully");
        }
        catch (Exception ex)
        {
            Logger.Error($"Failed to save game: {ex.Message}", ex);
            OnErrorOccurred($"Failed to save game: {ex.Message}");
        }
        finally
        {
            _isBusy = false;
        }
    }

    /// <summary>
    /// Handles load game command
    /// </summary>
    public async Task HandleLoadGameAsync()
    {
        if (_isBusy) return;
        
        try
        {
            _isBusy = true;
            Logger.Info("Load game requested");
            await _controller.LoadGameAsync();
            Logger.Debug("Load game completed successfully");
        }
        catch (Exception ex)
        {
            Logger.Error($"Failed to load game: {ex.Message}", ex);
            OnErrorOccurred($"Failed to load game: {ex.Message}");
        }
        finally
        {
            _isBusy = false;
        }
    }

    /// <summary>
    /// Handles go to main menu command
    /// </summary>
    public async Task HandleGoToMainMenuAsync()
    {
        if (_isBusy) return;
        
        try
        {
            _isBusy = true;
            Logger.Info("Go to main menu requested");
            await _controller.GoToMainMenuAsync();
            Logger.Debug("Go to main menu completed successfully");
        }
        catch (Exception ex)
        {
            Logger.Error($"Failed to go to main menu: {ex.Message}", ex);
            OnErrorOccurred($"Failed to go to main menu: {ex.Message}");
        }
        finally
        {
            _isBusy = false;
        }
    }

    /// <summary>
    /// Handles menu closing (resume game)
    /// </summary>
    public async Task HandleMenuClosingAsync()
    {
        try
        {
            Logger.Info("Game menu closing - resuming game");
            await _controller.ResumeGameAsync();
            Logger.Debug("Game resumed successfully on menu close");
        }
        catch (Exception ex)
        {
            Logger.Error($"Failed to resume game on menu close: {ex.Message}", ex);
            OnErrorOccurred($"Failed to resume game: {ex.Message}");
        }
    }

    #endregion

    #region Private Methods

    private void OnErrorOccurred(string error)
    {
        ErrorOccurred?.Invoke(this, error);
    }

    #endregion
}