using DeepSpaceSaga.UI.Presenters;
using DeepSpaceSaga.UI.Controller.Models;

namespace DeepSpaceSaga.UI.Screens.GameMenu;

public partial class ScreenMainMenu : Form
{
    private static readonly ILog Logger = LogManager.GetLogger(GeneralSettings.WinFormLoggerRepository, typeof(ScreenMainMenu));
    private readonly IMainMenuPresenter _presenter;
    private GameInfoModel? _currentGameInfo;

    public ScreenMainMenu(IMainMenuPresenter presenter)
    {
        _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
        
        InitializeComponent();
        SetupEventHandlers();
        
        Logger.Debug("ScreenMainMenu initialized with MVP pattern");
    }

    #region Form Events

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        
        try
        {
            await _presenter.InitializeAsync();
        }
        catch (Exception ex)
        {
            Logger.Error($"Failed to initialize presenter: {ex.Message}", ex);
            ShowError($"Failed to initialize menu: {ex.Message}");
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        try
        {
            DrawBorder(e.Graphics);
            DrawGameInfo(e.Graphics);
        }
        catch (Exception ex)
        {
            Logger.Error($"Error during form painting: {ex.Message}", ex);
        }
    }

    #endregion

    #region Private Methods

    private void SetupEventHandlers()
    {
        try
        {
            Console.WriteLine("[ScreenMainMenu] Setting up event handlers...");
            
            // Subscribe to presenter events
            _presenter.GameInfoUpdated += OnGameInfoUpdated;
            _presenter.ErrorOccurred += OnErrorOccurred;
            Console.WriteLine("[ScreenMainMenu] Presenter events subscribed");

            // Setup button click handlers
            button2.Click += OnNewGameClick; // NEW GAME button
            button3.Click += OnLoadGameClick; // LOAD button  
            button1.Click += OnExitClick; // EXIT button
            Console.WriteLine("[ScreenMainMenu] Button click handlers assigned");

            Logger.Debug("Event handlers setup completed");
            Console.WriteLine("[ScreenMainMenu] Event handlers setup completed successfully");
        }
        catch (Exception ex)
        {
            Logger.Error($"Failed to setup event handlers: {ex.Message}", ex);
            Console.WriteLine($"[ScreenMainMenu] Failed to setup event handlers: {ex.Message}");
            throw;
        }
    }

    private void DrawBorder(Graphics graphics)
    {
        using Pen borderPen = new Pen(UiConstants.FormBorderColor, UiConstants.FormBorderSize);
        Rectangle borderRect = new(
            UiConstants.FormBorderSize / 2,
            UiConstants.FormBorderSize / 2,
            Width - UiConstants.FormBorderSize,
            Height - UiConstants.FormBorderSize
        );
        graphics.DrawRectangle(borderPen, borderRect);
    }

    private void DrawGameInfo(Graphics graphics)
    {
        if (_currentGameInfo == null) return;

        try
        {
            using Font titleFont = new Font("Verdana", 24, FontStyle.Bold);
            using Font versionFont = new Font("Verdana", 10, FontStyle.Regular);
            using Font statusFont = new Font("Verdana", 8, FontStyle.Italic);
            using Brush textBrush = new SolidBrush(Color.Gainsboro);

            // Measure text for centering
            var titleSize = graphics.MeasureString(_currentGameInfo.GameTitle, titleFont);
            var versionSize = graphics.MeasureString(_currentGameInfo.VersionInfo, versionFont);

            // Calculate positions
            float titleX = (Width - titleSize.Width) / 2;
            float titleY = 50;
            float versionX = (Width - versionSize.Width) / 2;
            float versionY = titleY + titleSize.Height + 10;

            // Draw title and version
            graphics.DrawString(_currentGameInfo.GameTitle, titleFont, textBrush, titleX, titleY);
            graphics.DrawString(_currentGameInfo.VersionInfo, versionFont, textBrush, versionX, versionY);

            // Draw status message if available
            if (!string.IsNullOrEmpty(_currentGameInfo.StatusMessage))
            {
                var statusSize = graphics.MeasureString(_currentGameInfo.StatusMessage, statusFont);
                float statusX = (Width - statusSize.Width) / 2;
                float statusY = Height - 50;
                graphics.DrawString(_currentGameInfo.StatusMessage, statusFont, textBrush, statusX, statusY);
            }
        }
        catch (Exception ex)
        {
            Logger.Error($"Error drawing game info: {ex.Message}", ex);
        }
    }

    private void ShowError(string message)
    {
        MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    private void UpdateButtonStates()
    {
        if (_currentGameInfo != null)
        {
            button3.Enabled = _currentGameInfo.IsLoadGameEnabled && !_presenter.IsBusy;
        }
        
        button1.Enabled = !_presenter.IsBusy; // EXIT button
        button2.Enabled = !_presenter.IsBusy; // NEW GAME button
    }

    #endregion

    #region Event Handlers

    private void OnGameInfoUpdated(object? sender, GameInfoModel gameInfo)
    {
        try
        {
            if (InvokeRequired)
            {
                Invoke(() => OnGameInfoUpdated(sender, gameInfo));
                return;
            }

            _currentGameInfo = gameInfo;
            UpdateButtonStates();
            Invalidate(); // Trigger repaint

            Logger.Debug($"Game info updated - Title: {gameInfo.GameTitle}, Load enabled: {gameInfo.IsLoadGameEnabled}");
        }
        catch (Exception ex)
        {
            Logger.Error($"Error handling game info update: {ex.Message}", ex);
        }
    }

    private void OnErrorOccurred(object? sender, string error)
    {
        try
        {
            if (InvokeRequired)
            {
                Invoke(() => OnErrorOccurred(sender, error));
                return;
            }

            ShowError(error);
            Logger.Warn($"Error from presenter: {error}");
        }
        catch (Exception ex)
        {
            Logger.Error($"Error handling presenter error: {ex.Message}", ex);
        }
    }

    private async void OnNewGameClick(object? sender, EventArgs e)
    {
        try
        {
            Logger.Debug("NEW GAME button clicked");
            Console.WriteLine("[ScreenMainMenu] NEW GAME button clicked");
            
            UpdateButtonStates();
            Console.WriteLine("[ScreenMainMenu] Button states updated, calling presenter...");
            
            await _presenter.HandleNewGameAsync();
            
            Console.WriteLine("[ScreenMainMenu] Presenter call completed");
        }
        catch (Exception ex)
        {
            Logger.Error($"Error in OnNewGameClick: {ex.Message}", ex);
            Console.WriteLine($"[ScreenMainMenu] Error in OnNewGameClick: {ex.Message}");
            ShowError($"Failed to start new game: {ex.Message}");
        }
        finally
        {
            UpdateButtonStates();
            Console.WriteLine("[ScreenMainMenu] OnNewGameClick operation completed");
        }
    }

    private async void OnLoadGameClick(object? sender, EventArgs e)
    {
        try
        {
            UpdateButtonStates();
            await _presenter.HandleLoadGameAsync();
        }
        catch (Exception ex)
        {
            Logger.Error($"Error in OnLoadGameClick: {ex.Message}", ex);
            ShowError($"Failed to load game: {ex.Message}");
        }
        finally
        {
            UpdateButtonStates();
        }
    }

    private async void OnExitClick(object? sender, EventArgs e)
    {
        try
        {
            UpdateButtonStates();
            await _presenter.HandleExitAsync();
        }
        catch (Exception ex)
        {
            Logger.Error($"Error in OnExitClick: {ex.Message}", ex);
            ShowError($"Failed to exit application: {ex.Message}");
        }
        finally
        {
            UpdateButtonStates();
        }
    }

    #endregion

    #region Legacy Event Handlers (obsolete)
    
    [Obsolete("Use MVP pattern with Presenter instead")]
    private void Event_ApplicationExit(object sender, EventArgs e)
    {
        Logger.Debug("Legacy Event_ApplicationExit called - redirecting to Presenter");
        OnExitClick(sender, e);
    }

    [Obsolete("Use MVP pattern with Presenter instead")]
    private void Event_StartNewGameSession(object sender, EventArgs e)
    {
        Logger.Debug("Legacy Event_StartNewGameSession called - redirecting to Presenter");
        OnNewGameClick(sender, e);
    }

    [Obsolete("Use MVP pattern with Presenter instead")]
    private void Event_LoadGame(object sender, EventArgs e)
    {
        Logger.Debug("Legacy Event_LoadGame called - redirecting to Presenter");
        OnLoadGameClick(sender, e);
    }

    #endregion
}
