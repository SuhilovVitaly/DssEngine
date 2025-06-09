using DeepSpaceSaga.Common.Abstractions.Dto.Ui;
using DeepSpaceSaga.UI.Controller.Services;

namespace DeepSpaceSaga.UI.Screens.TacticGame;

public partial class ScreenTacticGame : Form
{
    private static readonly ILog Logger = LogManager.GetLogger(GeneralSettings.WinFormLoggerRepository, typeof(ScreenTacticGame));

    private GameManager _gameManager;

    public ScreenTacticGame(GameManager gameManager)
    {
        InitializeComponent();

        _gameManager = gameManager ?? throw new InvalidOperationException("Failed to resolve GameManager");

        _gameManager.OnUpdateGameData += GameServer_OnGetDataFromServer;

        KeyPreview = true;
        KeyDown += Window_KeyDown;
    }

    private void GameServer_OnGetDataFromServer(GameSessionDto session)
    {
        CrossThreadExtensions.PerformSafely(this, RefreshSessionInfo, session);
    }

    private void RefreshSessionInfo(GameSessionDto session)
    {
        
    }

    private void Window_KeyDown(object? sender, KeyEventArgs e)
    {
        _ = KeyDownAsync(e);
    }

    private async Task KeyDownAsync(KeyEventArgs e)
    {
        Logger.Debug($"Window_KeyDown - Handle the KeyDown event {e.KeyCode} ");

        switch (e.KeyCode)
        {
            case Keys.Escape:
                //_gameManager.Events.Pause();
                _gameManager.Screens.ShowGameMenuScreen();
                break;
        }
    }
}
