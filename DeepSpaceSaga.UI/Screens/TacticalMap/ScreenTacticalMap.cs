using DeepSpaceSaga.Common.Abstractions.Dto.Ui;
using DeepSpaceSaga.UI.Controller.Services;

namespace DeepSpaceSaga.UI.Screens.TacticalMap;

public partial class ScreenTacticalMap : Form
{
    private readonly SKControl _skControl;
    private IGameManager _gameManager;
    private readonly IScreensService _screensService;
    private bool isDrawInProcess = false;
    private GameSessionDto _gameSessionDto;

    public ScreenTacticalMap(IGameManager gameManager, IScreensService screensService)
    {
        _gameManager = gameManager ?? throw new ArgumentNullException(nameof(gameManager));
        _screensService = screensService ?? throw new ArgumentNullException(nameof(screensService));

        InitializeComponent();

        // Initialize dependencies for child controls
        sessionInformationControl.GameManager = _gameManager;


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
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape)
        {
            _gameManager.SessionPause();
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
}
