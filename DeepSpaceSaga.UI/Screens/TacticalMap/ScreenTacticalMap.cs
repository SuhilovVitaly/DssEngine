using DeepSpaceSaga.Common.Abstractions.Dto;

namespace DeepSpaceSaga.UI.Screens.TacticalMap;

public partial class ScreenTacticalMap : Form
{
    private readonly SKControl _skControl;
    private GameManager _gameManager;
    private bool isDrawInProcess = false;
    private GameSessionDto _gameSessionDto;

    public ScreenTacticalMap(GameManager gameManager)
    {
        InitializeComponent();

        _gameManager = gameManager ?? throw new ArgumentNullException(nameof(GameManager));

        _gameManager.OnUpdateGameData += UpdateGameData;
        _gameManager.OnUpdateGameData += ControlGameSpeed.UpdateGameData;

        FormBorderStyle = FormBorderStyle.None;
        //BackColor = Color.Black;
        ShowInTaskbar = false;

        // Set size to primary screen dimensions
        Size = Screen.PrimaryScreen.Bounds.Size;
        Location = new Point(0, 0);

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
        _skControl.Invalidate();
    }

    private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        if(_gameSessionDto == null) return;

        var canvas = e.Surface.Canvas;

        canvas.Clear(SKColors.Black);

        //var session = Global.GameManager.GetSession();

        //Global.GameManager.Screens.Settings.GraphicSurface = canvas;
        //Global.GameManager.Screens.Settings.CenterScreenOnMap = session.GetPlayerSpaceShip().GetLocation();

        //Global.Resources.DrawTool.DrawTacticalMapScreen(session, Global.GameManager.OuterSpace, _gameManager.Screens.Settings);
    }
}
