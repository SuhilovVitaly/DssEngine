using System.Windows.Forms;
using DeepSpaceSaga.Common.Abstractions.Dto.Ui;
using DeepSpaceSaga.Common.Extensions;
using DeepSpaceSaga.UI.Controller.Services;
using DeepSpaceSaga.UI.Rendering.Tools;
using DeepSpaceSaga.UI.Tools;
using SkiaSharp.Views.Desktop;

namespace DeepSpaceSaga.UI.Screens.TacticalMap.ScreenControls;

public partial class GameTacticalMap : UserControl
{
    private IGameManager _gameManager;
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

        _skControl.MouseClick += MapClick;
        _skControl.MouseMove += MapMouseMove;

        // Enable key events for UserControl
        this.SetStyle(ControlStyles.Selectable, true);
        this.TabStop = true;
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        InitializeGameManager();
        
        // Set focus to enable key events
        this.Focus();
    }

    private void InitializeGameManager()
    {
        if (_isInitialized || DesignModeChecker.IsInDesignMode()) return;

        try
        {
            _gameManager = Program.ServiceProvider?.GetService<IGameManager>();
            if (_gameManager != null)
            {
                _gameManager.OnUpdateGameData += UpdateGameData;
                _isInitialized = true;
            }
        }
        catch
        {
            // Ignore exceptions in design mode
            if (!DesignModeChecker.IsInDesignMode()) throw;
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
        InformationAboutContol.Text = $"{session.State.ProcessedTurns:D5}{Environment.NewLine}Zoom: {_gameManager.ScreenInfo.Zoom.Scale}";

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

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Keys.Add)
        {
            _gameManager?.ScreenInfo?.Zoom?.In();
            return true;
        }
        else if (keyData == Keys.Subtract)
        {
            _gameManager?.ScreenInfo?.Zoom?.Out();
            return true;
        }
        
        return base.ProcessCmdKey(ref msg, keyData);
    }

    private void MapMouseMove(object sender, MouseEventArgs e)
    {
        if(_sessionDto == null) { return; }

        var screenCoordinates = e.Location.ToSpaceMapCoordinates();

        var mouseScreenCoordinates = UiTools.ToRelativeCoordinates(_gameManager.ScreenInfo, screenCoordinates, _gameManager.ScreenInfo.Center);

        var celestialCoordinates = UiTools.ToTacticalMapCoordinates(_gameManager.ScreenInfo, mouseScreenCoordinates, _gameManager.ScreenInfo.CenterScreenOnMap);

        _gameManager.OuterSpace.HandleMouseMove(_sessionDto, celestialCoordinates, screenCoordinates);

        _gameManager.ScreenInfo.SetMousePosition(celestialCoordinates, screenCoordinates);
    }

    private void MapClick(object sender, MouseEventArgs e)
    {
        if (_sessionDto == null) { return; }

        var location = e.Location.ToSpaceMapCoordinates();

        var mouseScreenCoordinates = UiTools.ToRelativeCoordinates(_gameManager.ScreenInfo, location, _gameManager.ScreenInfo.Center);

        var mouseLocation = UiTools.ToTacticalMapCoordinates(_gameManager.ScreenInfo, mouseScreenCoordinates, _gameManager.ScreenInfo.CenterScreenOnMap);

       // var objectsInClickPoint = _sessionDto.

        if (e.Button == MouseButtons.Left)
        {
            _gameManager.OuterSpace.HandleMouseClick(_sessionDto, mouseLocation);
        }

        if (e.Button == MouseButtons.Right)
        {
            //_gameManager.OuterSpace.TacticalMapLeftMouseClick(mouseLocation);
        }
    }
}
