using DeepSpaceSaga.Common.Abstractions.Dto.Ui;
using DeepSpaceSaga.UI.Controller.Services;

namespace DeepSpaceSaga.UI.Screens.TacticalMap.ScreenControls;

public partial class GameSessionInformation : ControlWindow
{
    private IGameManager _gameManager;
    private bool _isInitialized;

    public IGameManager GameManager
    {
        get => _gameManager;
        set
        {
            if (_gameManager != null)
            {
                _gameManager.OnUpdateGameData -= UpdateGameData;

                _gameManager.OuterSpace.OnSelectCelestialObject -= OuterSpace_OnSelectCelestialObject;
                _gameManager.OuterSpace.OnShowCelestialObject -= OuterSpace_OnShowCelestialObject;
                _gameManager.OuterSpace.OnHideCelestialObject -= OuterSpace_OnHideCelestialObject;
            }
            
            _gameManager = value;
            
            if (_gameManager != null && !DesignMode)
            {
                _gameManager.OnUpdateGameData += UpdateGameData;

                _gameManager.OuterSpace.OnSelectCelestialObject += OuterSpace_OnSelectCelestialObject;
                _gameManager.OuterSpace.OnShowCelestialObject += OuterSpace_OnShowCelestialObject;
                _gameManager.OuterSpace.OnHideCelestialObject += OuterSpace_OnHideCelestialObject;
                _isInitialized = true;
            }
        }
    }

    // Constructor for Designer.cs compatibility
    public GameSessionInformation()
    {
        InitializeComponent();
        Title = "Game Session Information";
    }

    // Constructor with dependency injection
    public GameSessionInformation(IGameManager gameManager) : this()
    {
        GameManager = gameManager ?? throw new ArgumentNullException(nameof(gameManager));        
    }

    private void OuterSpace_OnHideCelestialObject(CelestialObjectDto obj)
    {
        UpdateGameData(_gameManager.GameSession());
    }

    private void OuterSpace_OnShowCelestialObject(CelestialObjectDto obj)
    {
        UpdateGameData(_gameManager.GameSession());
    }

    private void OuterSpace_OnSelectCelestialObject(CelestialObjectDto obj)
    {
        UpdateGameData(_gameManager.GameSession());
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
    }

    private void UpdateGameData(GameSessionDto session)
    {
        if (IsDisposed) return;
        CrossThreadExtensions.PerformSafely(this, RereshControls, session);
    }

    private void RereshControls(GameSessionDto session)
    {
        if (IsDisposed) return;
        txtTurn.Text = $"{session.State.Cycle:D3}:{session.State.Turn:D3}:{session.State.Tick:D3} [{session.State.ProcessedTurns:D5}]";
        txtSpeed.Text = session.State.IsPaused ? "Pause" : session.State.Speed + "";
        txtCelestialObjects.Text = session.CelestialObjects.Count() + "";
        crlScreenCoordinates.Text = _gameManager?.ScreenInfo?.MousePosition?.X + ":" + _gameManager?.ScreenInfo?.MousePosition?.Y;
        crlScreenCoordinatesRelative.Text = _gameManager?.ScreenInfo?.RelativeMousePosition?.X + ":" + _gameManager?.ScreenInfo?.RelativeMousePosition?.Y;
        crlGameCoordinates.Text = _gameManager?.TacticalMapMousePosition?.X + ":" + _gameManager?.TacticalMapMousePosition?.Y;

        crlActiveId.Text = _gameManager?.OuterSpace.ActiveObjectId.ToString();
        crlSelectedId.Text = _gameManager?.OuterSpace.SelectedObjectId.ToString();
    }
}
