using DeepSpaceSaga.UI.Controller.Screens.Presenters;

namespace DeepSpaceSaga.UI.Screens.GameMenu;

public partial class ScreenGameMenu : Form
{
    private readonly IGameMenuPresenter _presenter;    

    public ScreenGameMenu(IGameMenuPresenter presenter)
    {
        _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
        
        InitializeComponent();
        SetupEventHandlers();

        Cursor = CursorManager.DefaultCursor;
        crlResume.Cursor = CursorManager.SelectedCursor;
        crlSave.Cursor = CursorManager.SelectedCursor;
        crlLoad.Cursor = CursorManager.SelectedCursor;
        crlSettings.Cursor = CursorManager.SelectedCursor;
        crlMainMenu.Cursor = CursorManager.SelectedCursor;
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
            ShowError($"Failed to initialize menu: {ex.Message}");
        }
    }

    #endregion

    #region Private Methods

    private void SetupEventHandlers()
    {
        // Enable key handling
        KeyPreview = true;
        KeyDown += OnKeyDown;
        
        // Handle form closing to resume game
        FormClosed += OnFormClosed;
        
        // Subscribe to presenter events
        _presenter.ErrorOccurred += OnErrorOccurred;
        
        // Setup button click handlers
        crlResume.Click += OnResumeClick; // RESUME button
        crlSave.Click += OnSaveClick; // SAVE button  
        crlLoad.Click += OnLoadClick; // LOAD button
        crlMainMenu.Click += OnMainMenuClick; // MAIN MENU button
    }

    private void ShowError(string message)
    {
        MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    #endregion

    #region Event Handlers

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape)
        {
            Close();
        }
    }

    private async void OnFormClosed(object sender, FormClosedEventArgs e)
    {
        // Handle menu closing through presenter
        await _presenter.HandleMenuClosingAsync();
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
        }
        catch (Exception ex)
        {
            // Log error but don't throw to prevent cascading failures
            Console.WriteLine($"Error handling presenter error: {ex.Message}");
        }
    }

    private async void OnResumeClick(object? sender, EventArgs e)
    {
        try
        {
            await _presenter.HandleResumeGameAsync();
            Close(); // Close menu after resuming
        }
        catch (Exception ex)
        {
            ShowError($"Failed to resume game: {ex.Message}");
        }
    }

    private async void OnSaveClick(object? sender, EventArgs e)
    {
        try
        {
            await _presenter.HandleSaveGameAsync();
        }
        catch (Exception ex)
        {
            ShowError($"Failed to save game: {ex.Message}");
        }
    }

    private async void OnLoadClick(object? sender, EventArgs e)
    {
        try
        {
            await _presenter.HandleLoadGameAsync();
        }
        catch (Exception ex)
        {
            ShowError($"Failed to load game: {ex.Message}");
        }
    }

    private async void OnMainMenuClick(object? sender, EventArgs e)
    {
        try
        {
            await _presenter.HandleGoToMainMenuAsync();
            Close(); // Close menu after going to main menu
        }
        catch (Exception ex)
        {
            ShowError($"Failed to go to main menu: {ex.Message}");
        }
    }

    #endregion

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        // Draw border
        using Pen borderPen = new Pen(UiConstants.FormBorderColor, UiConstants.FormBorderSize);
        Rectangle borderRect = new(
            UiConstants.FormBorderSize / 2,
            UiConstants.FormBorderSize / 2,
            Width - UiConstants.FormBorderSize,
            Height - UiConstants.FormBorderSize
        );
        e.Graphics.DrawRectangle(borderPen, borderRect);
    }
}
