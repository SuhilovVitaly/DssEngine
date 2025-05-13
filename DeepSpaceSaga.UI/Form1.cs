namespace DeepSpaceSaga.UI;

public partial class Form1 : Form
{
    private static readonly ILog Logger = LogManager.GetLogger(GeneralSettings.WinFormLoggerRepository, typeof(Form1));

    private GameManager _gameServer;

    public Form1()
    {
        InitializeComponent();

        _gameServer = Program.ServiceProvider?.GetService<GameManager>()
            ?? throw new InvalidOperationException("Failed to resolve IGameServer");

        _gameServer.OnUpdateGameData += GameServer_OnGetDataFromServer;
    }

    private void GameServer_OnGetDataFromServer(GameSessionDTO session)
    {
        CrossThreadExtensions.PerformSafely(this, RefreshSessionInfo, session);
    }

    private void RefreshSessionInfo(GameSessionDTO session)
    {
        crlSessionInfo.Text = session.FlowState + " Turn: " + session.Turn;
    }

    private void crlStartProcessing_Click(object sender, EventArgs e)
    {
        _gameServer.SessionStart();
        Logger.Debug("SessionStart command");
    }

    private void crlStopProcessing_Click(object sender, EventArgs e)
    {
        _gameServer.SessionStop();
        Logger.Debug("SessionPause command");
    }

    private void crlResumeProcessing_Click(object sender, EventArgs e)
    {
        _gameServer.SessionResume();
        Logger.Debug("SessionResume command");
    }

    private void crlPauseProcessing_Click(object sender, EventArgs e)
    {
        _gameServer.SessionPause();
        Logger.Debug("SessionPause command");
    }
}
