using DeepSpaceSaga.Common.Abstractions.Dto.Ui;
using DeepSpaceSaga.Common.Abstractions.Events;
using DeepSpaceSaga.Common.Abstractions.Services;
using DeepSpaceSaga.UI.Controller.Services;
using DeepSpaceSaga.UI.Screens.Dialogs;

namespace DeepSpaceSaga.UI.Screens.TacticalMap;

public partial class ScreenTacticalMap : Form, IScreenTacticalMap
{
    private readonly SKControl _skControl;
    private IGameManager _gameManager;
    private readonly IScreensService _screensService;
    private readonly IScreenTacticalMapController _controller;
    private bool isDrawInProcess = false;
    private GameSessionDto _gameSessionDto;

    public ScreenTacticalMap(IGameManager gameManager, IScreensService screensService, IScreenTacticalMapController controller)
    {
        _gameManager = gameManager ?? throw new ArgumentNullException(nameof(gameManager));
        _screensService = screensService ?? throw new ArgumentNullException(nameof(screensService));
        _controller = controller;

        InitializeComponent();

        // Initialize dependencies for child controls
        sessionInformationControl.GameManager = _gameManager;

        //_gameManager.Screens.TacticalMap = this;
        _gameManager.OnUpdateGameData += UpdateGameData;
        _gameManager.OnUpdateGameData += ControlGameSpeed.UpdateGameData;

        FormBorderStyle = FormBorderStyle.None;
        //BackColor = Color.Black;
        ShowInTaskbar = false;

        // Set size to primary screen dimensions
        Size = new Size((int)_gameManager.ScreenInfo.Width, (int)_gameManager.ScreenInfo.Height);// Screen.PrimaryScreen.Bounds.Size;
        Location = new Point(0, 0);

        // Enable key handling
        KeyPreview = true;
        KeyDown += OnKeyDown;
        MouseWheel += Window_MouseWheel;

        GameTacticalMapControl.Dock = DockStyle.Fill;

        Cursor = CursorManager.DefaultCursor;

        crlRightToolbar.OnShowRightPanel += crlRightPanel.ShowPanel;
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape)
        {
            _screensService.ShowGameMenuModal();
        }
    }

    private void UpdateGameData(GameSessionDto sessionDto)
    {
        if (isDrawInProcess) return;

        isDrawInProcess = true;

        CrossThreadExtensions.PerformSafely(this, RefreshControls, sessionDto);
        isDrawInProcess = false;
    }

    private void RefreshControls(GameSessionDto sessionDto)
    {
        _gameSessionDto = sessionDto;
        _skControl?.Invalidate();
    }

    private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        if(_gameSessionDto == null) return;

        var canvas = e.Surface.Canvas;

        canvas.Clear(SKColors.Black);
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        base.OnFormClosed(e);
    }

    private void Window_MouseWheel(object? sender, MouseEventArgs e)
    {
        if (e.Delta > 0)
        {
            _gameManager.ScreenInfo.Zoom.In();
        }
        else
        {
            _gameManager.ScreenInfo.Zoom.Out();
        }
        Refresh();
    }

    public void CloseRightPanel()
    {
        crlRightPanel.Visible = false;
    }

    public void StartDialog(GameActionEventDto gameActionEvent)
    {
        try
        {
            CrossThreadExtensions.PerformSafely(this, OpenModalDialogWindow, gameActionEvent);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    private void OpenModalDialogWindow(GameActionEventDto gameActionEvent)
    {
        var screenDialog = new DialogBasicScreen(gameActionEvent, _gameManager)
        {
            FormBorderStyle = FormBorderStyle.None,
            Size = new Size(1375, 875),
            StartPosition = FormStartPosition.Manual
        };

        var x = Location.X + (Width - screenDialog.Width) / 2;
        var y = Location.Y + (Height - screenDialog.Height) / 2;
        screenDialog.Location = new Point(x, y);

        screenDialog.ShowDialog(this);
    }
}
