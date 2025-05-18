namespace DeepSpaceSaga.UI.Screens.GameMenu;

public partial class ScreenGameMenu : Form
{
    private static readonly ILog Logger = LogManager.GetLogger(GeneralSettings.WinFormLoggerRepository, typeof(ScreenGameMenu));

    private GameManager _gameManager;

    public ScreenGameMenu(GameManager gameManager)
    {
        InitializeComponent();

        _gameManager = gameManager ?? throw new InvalidOperationException("Failed to resolve GameManager");

        Logger?.Info($"ScreenGameMenu builded susseccefully");
    }

    private void crlExitGame_Click(object sender, EventArgs e)
    {
        Logger?.Info($"Exit application");
        Application.Exit();
    }

    private void Event_NewGameStart(object sender, EventArgs e)
    {
        Logger?.Info($"New game session start");
        _gameManager.Screens.ShowTacticalMapScreen();
    }
}
