using DeepSpaceSaga.Common.Abstractions.Services;
using DeepSpaceSaga.UI.Render.Rendering.TacticalMap;

namespace DeepSpaceSaga.UI.Screens.TacticalMap.ScreenControls;

public partial class GameTacticalMap : UserControl
{
    private GameManager _gameManager;
    private bool _isInitialized;
    private readonly SKControl _skControl;
    GameSessionDto _sessionDto;

    public GameTacticalMap()
    {
        InitializeComponent();

        _skControl = new SKControl
        {
            Dock = DockStyle.Fill,
            Visible = true
        };

        _skControl.PaintSurface += OnPaintSurface;
        _skControl.BringToFront();
        Controls.Add(_skControl);
        _skControl.BringToFront();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        InitializeGameManager();
    }

    private void InitializeGameManager()
    {
        if (_isInitialized || DesignMode) return;

        try
        {
            _gameManager = Program.ServiceProvider?.GetService<GameManager>();
            if (_gameManager != null)
            {
                _gameManager.OnUpdateGameData += UpdateGameData;
                _isInitialized = true;
            }
        }
        catch
        {
            // Ignore exceptions in design mode
            if (!DesignMode) throw;
        }
    }

    private void UpdateGameData(GameSessionDto session)
    {
        if (IsDisposed) return;
        CrossThreadExtensions.PerformSafely(this, RereshControls, session);
    }

    private void RereshControls(GameSessionDto session)
    {
        if (IsDisposed) return;
        _sessionDto = session;
        InformationAboutContol.Text = $"{session.State.ProcessedTurns:D5}";

        _skControl.Invalidate();
    }

    private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        if(_sessionDto == null) return;

        var canvas = e.Surface.Canvas;

        _gameManager.ScreenInfo.GraphicSurface = canvas;

        canvas.Clear(SKColors.Black);

        DrawTacticalMap.DrawTacticalMapScreen(_sessionDto, _gameManager.ScreenInfo);
    }
}
