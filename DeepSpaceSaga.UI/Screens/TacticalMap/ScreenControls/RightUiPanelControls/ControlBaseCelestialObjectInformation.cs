using DeepSpaceSaga.Common.Abstractions.Dto.Ui;

namespace DeepSpaceSaga.UI.Screens.TacticalMap.ScreenControls.RightUiPanelControls;

public partial class ControlBaseCelestialObjectInformation : UserControl
{
    private IGameManager _gameManager;
    private bool _isInitialized;
    private readonly SKControl _skControl;
    private GameSessionDto _sessionDto;
    private int _selectedObjectId;
    private int _shownObjectId;

    public ControlBaseCelestialObjectInformation()
    {
        InitializeComponent();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        InitializeGameManager();
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
                _gameManager.OuterSpace.OnSelectCelestialObject += SelectCelestialObject;
                _gameManager.OuterSpace.OnShowCelestialObject += ShowCelestialObject;
                _gameManager.OuterSpace.OnHideCelestialObject += HideCelestialObject;
                _isInitialized = true;
            }
        }
        catch
        {
            // Ignore exceptions in design mode
            if (!DesignModeChecker.IsInDesignMode()) throw;
        }
    }

    private void HideCelestialObject(CelestialObjectDto dto)
    {
        _shownObjectId = 0;
    }

    private void ShowCelestialObject(CelestialObjectDto dto)
    {
        _shownObjectId = dto.Id;
    }

    private void SelectCelestialObject(CelestialObjectDto dto)
    {
        _selectedObjectId = dto.Id;
    }

    private void UpdateGameData(GameSessionDto session)
    {
        if (IsDisposed) return;
        CrossThreadExtensions.PerformSafely(this, RefreshControls, session);
    }

    private void RefreshControls(GameSessionDto session)
    {
        if (IsDisposed) return;

        crlCelestialObjectId.Text = _shownObjectId == 0 ? _selectedObjectId.ToString() : _shownObjectId.ToString();
    }

}
