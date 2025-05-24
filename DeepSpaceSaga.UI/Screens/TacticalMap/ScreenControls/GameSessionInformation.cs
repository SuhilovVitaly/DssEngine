namespace DeepSpaceSaga.UI.Screens.TacticalMap.ScreenControls;

public partial class GameSessionInformation : ControlWindow
{
    private GameManager _gameManager;
    private bool _isInitialized;

    public GameSessionInformation()
    {
        InitializeComponent();
        Title = "Game Session Information";
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
        txtTurn.Text = $"{session.State.Cycle:D3}:{session.State.Turn:D3}:{session.State.Tick:D3} [{session.State.ProcessedTurns:D5}]";
        txtSpeed.Text = session.State.IsPaused ? "Pause" : session.State.Speed + "";
        txtCelestialObjects.Text = session.CelestialObjects.Count() + "";
    }
}
