using DeepSpaceSaga.UI.Controller.Abstractions;
using DeepSpaceSaga.UI.Controller.Models;

namespace DeepSpaceSaga.UI.Presenters;

/// <summary>
/// Presenter for Main Menu screen handling View coordination and UI logic
/// </summary>
public class MainMenuPresenter : IMainMenuPresenter
{
    private readonly IMainMenuController _controller;
    private static readonly ILog Logger = LogManager.GetLogger(GeneralSettings.WinFormLoggerRepository, typeof(MainMenuPresenter));
    
    private bool _isBusy = false;

    public MainMenuPresenter(IMainMenuController controller)
    {
        _controller = controller ?? throw new ArgumentNullException(nameof(controller));
    }

    #region Events

    /// <summary>
    /// Event fired when game info is updated
    /// </summary>
    public event EventHandler<GameInfoModel>? GameInfoUpdated;

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
    /// Initializes the presenter and loads initial data
    /// </summary>
    public async Task InitializeAsync()
    {
        try
        {
            Logger.Info("Initializing Main Menu presenter");
            await RefreshGameInfoAsync();
            Logger.Debug("Main Menu presenter initialized successfully");
        }
        catch (Exception ex)
        {
            Logger.Error($"Failed to initialize Main Menu presenter: {ex.Message}", ex);
            OnErrorOccurred($"Failed to initialize menu: {ex.Message}");
        }
    }

    /// <summary>
    /// Handles new game command
    /// </summary>
    public async Task HandleNewGameAsync()
    {
        if (_isBusy)
        {
            Logger.Warn("New game command ignored - presenter is busy");
            Console.WriteLine("[MainMenuPresenter] New game command ignored - presenter is busy");
            return;
        }

        try
        {
            SetBusy(true);
            Logger.Info("Starting new game from Main Menu");
            Console.WriteLine("[MainMenuPresenter] Starting new game from Main Menu");
            
            await _controller.StartNewGameAsync();
            
            Logger.Debug("New game started successfully");
            Console.WriteLine("[MainMenuPresenter] New game started successfully");
        }
        catch (Exception ex)
        {
            Logger.Error($"Failed to start new game: {ex.Message}", ex);
            Console.WriteLine($"[MainMenuPresenter] Failed to start new game: {ex.Message}");
            OnErrorOccurred($"Failed to start new game: {ex.Message}");
        }
        finally
        {
            SetBusy(false);
            Console.WriteLine("[MainMenuPresenter] New game operation completed, busy state reset");
        }
    }

    /// <summary>
    /// Handles load game command
    /// </summary>
    public async Task HandleLoadGameAsync()
    {
        if (_isBusy)
        {
            Logger.Warn("Load game command ignored - presenter is busy");
            return;
        }

        try
        {
            SetBusy(true);
            Logger.Info("Loading game from Main Menu");
            
            await _controller.LoadGameAsync();
            
            Logger.Debug("Game loaded successfully");
        }
        catch (NotImplementedException)
        {
            Logger.Warn("Load game functionality not implemented");
            OnErrorOccurred("Load game feature is not yet available");
        }
        catch (Exception ex)
        {
            Logger.Error($"Failed to load game: {ex.Message}", ex);
            OnErrorOccurred($"Failed to load game: {ex.Message}");
        }
        finally
        {
            SetBusy(false);
        }
    }

    /// <summary>
    /// Handles exit application command
    /// </summary>
    public async Task HandleExitAsync()
    {
        if (_isBusy)
        {
            Logger.Warn("Exit command ignored - presenter is busy");
            return;
        }

        try
        {
            SetBusy(true);
            Logger.Info("Exiting application from Main Menu");
            
            await _controller.ExitApplicationAsync();
            
            // This line may not be reached if application exits
            Logger.Debug("Application exit completed");
        }
        catch (Exception ex)
        {
            Logger.Error($"Failed to exit application: {ex.Message}", ex);
            OnErrorOccurred($"Failed to exit application: {ex.Message}");
        }
        finally
        {
            SetBusy(false);
        }
    }

    /// <summary>
    /// Refreshes game information
    /// </summary>
    public async Task RefreshGameInfoAsync()
    {
        try
        {
            Logger.Debug("Refreshing game info");
            
            var gameInfo = await _controller.GetGameInfoAsync();
            OnGameInfoUpdated(gameInfo);
            
            Logger.Debug($"Game info updated - Load enabled: {gameInfo.IsLoadGameEnabled}");
        }
        catch (Exception ex)
        {
            Logger.Error($"Failed to refresh game info: {ex.Message}", ex);
            OnErrorOccurred($"Failed to refresh game information: {ex.Message}");
        }
    }

    #endregion

    #region Private Methods

    private void SetBusy(bool busy)
    {
        _isBusy = busy;
        Logger.Debug($"Presenter busy state changed to: {busy}");
    }

    private void OnGameInfoUpdated(GameInfoModel gameInfo)
    {
        GameInfoUpdated?.Invoke(this, gameInfo);
    }

    private void OnErrorOccurred(string error)
    {
        ErrorOccurred?.Invoke(this, error);
    }

    #endregion
} 