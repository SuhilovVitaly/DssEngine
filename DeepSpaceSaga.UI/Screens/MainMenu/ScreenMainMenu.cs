namespace DeepSpaceSaga.UI.Screens.MainMenu;

public partial class ScreenMainMenu : Form
{
    private static readonly ILog Logger = LogManager.GetLogger(GeneralSettings.WinFormLoggerRepository, typeof(ScreenMainMenu));
    private readonly IMainMenuController _controller;

    public ScreenMainMenu(IMainMenuController controller)
    {
        _controller = controller ?? throw new ArgumentNullException(nameof(controller));
        
        InitializeComponent();
        
        Logger.Debug("ScreenMainMenu initialized with MVP pattern");
    }

    #region Form Events


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
        var _currentGameInfo = _controller.GetGameInfo();

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

    #endregion

    #region Legacy Event Handlers (obsolete)
    private async void Event_ApplicationExit(object sender, EventArgs e)
    {
        await Event_ApplicationExitAsync();
    }
    private async Task Event_ApplicationExitAsync()
    {
        Logger.Debug("Event_ApplicationExit called - redirecting to Controller");

        try
        {
            await _controller.ExitApplicationAsync();
        }
        catch (Exception ex)
        {
            Logger.Error($"Error in Event_ApplicationExitAsync: {ex.Message}", ex);
            Console.WriteLine($"[ScreenMainMenu] Error in Event_ApplicationExitAsync: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("[ScreenMainMenu] Event_ApplicationExitAsync operation completed");
        }
    }

    private async void Event_StartNewGameSession(object sender, EventArgs e)
    {
        await Event_StartNewGameSessionAsync();
    }

    private async Task Event_StartNewGameSessionAsync()
    {
        Logger.Debug("Event_StartNewGameSession called - redirecting to Controller");

        try
        {
            Logger.Debug("NEW GAME button clicked");
            Console.WriteLine("[ScreenMainMenu] NEW GAME button clicked");

            await _controller.StartNewGameAsync();
        }
        catch (Exception ex)
        {
            Logger.Error($"Error in Event_StartNewGameSessionAsync: {ex.Message}", ex);
            Console.WriteLine($"[ScreenMainMenu] Error in Event_StartNewGameSessionAsync: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("[ScreenMainMenu] Event_StartNewGameSessionAsync operation completed");
        }
    }

    private async void Event_LoadGame(object sender, EventArgs e)
    {
        await Event_LoadGameAsync();
    }

    private async Task Event_LoadGameAsync()
    {
        Logger.Debug("Legacy Event_LoadGameAsync called - redirecting to Controller");

        try
        {
            Logger.Debug("LOAD GAME button clicked");
            Console.WriteLine("[ScreenMainMenu] LOAD GAME button clicked");

            await _controller.LoadGameAsync();
        }
        catch (Exception ex)
        {
            Logger.Error($"Error in Event_LoadGameAsync: {ex.Message}", ex);
            Console.WriteLine($"[ScreenMainMenu] Error in Event_LoadGameAsync: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("[ScreenMainMenu] Event_LoadGameAsync operation completed");
        }
    }

    #endregion
}
