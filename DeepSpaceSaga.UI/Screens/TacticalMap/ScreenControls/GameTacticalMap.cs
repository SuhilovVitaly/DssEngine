namespace DeepSpaceSaga.UI.Screens.TacticalMap.ScreenControls;

public partial class GameTacticalMap : UserControl
{
    private GameManager _gameManager;
    private bool _isInitialized;
    private readonly SKControl _skControl;
    private GameSessionDto _sessionDto;
    
    // Performance optimization: cache last rendered data
    private int _lastProcessedTurns = -1;

    public GameTacticalMap()
    {
        InitializeComponent();

        _skControl = new SKControl
        {
            Dock = DockStyle.Fill,
            Visible = true
        };

        _skControl.PaintSurface += OnPaintSurface;
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
        CrossThreadExtensions.PerformSafely(this, RefreshControls, session);
    }

    private void RefreshControls(GameSessionDto session)
    {
        if (IsDisposed) return;
        
        _sessionDto = session;
        InformationAboutContol.Text = $"{session.State.ProcessedTurns:D5}";

        // Only invalidate if data actually changed
        if (_lastProcessedTurns != session.State.ProcessedTurns)
        {
            _lastProcessedTurns = session.State.ProcessedTurns;
            _skControl.Invalidate();
        }
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
