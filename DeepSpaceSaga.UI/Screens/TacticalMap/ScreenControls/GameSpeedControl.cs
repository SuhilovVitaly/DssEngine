using DeepSpaceSaga.Common.Abstractions.Dto.Ui;
using DeepSpaceSaga.UI.Controller.Services;

namespace DeepSpaceSaga.UI.Screens.MainGameScreen;

public partial class GameSpeedControl : UserControl
{
    private int _lastGameSpeed = 1;
    private bool _lastGameIsPaused = true;
    private IGameManager _gameManager;
    public GameSpeedControl()
    {
        InitializeComponent();
    }

    public void UpdateGameData(GameSessionDto session)
    {
        if(_gameManager is null)
        {
            _gameManager = Program.ServiceProvider?.GetService<IGameManager>() ?? throw new InvalidOperationException("Failed to resolve GameManager");
        }

        CrossThreadExtensions.PerformSafely(this, RereshControls, session);
    }

    private void RereshControls(GameSessionDto session)
    {
        if (session.State.IsPaused)
        {
            cmdPause.ForeColor = Color.OrangeRed;

            button1.ForeColor = Color.DarkGray;
            button2.ForeColor = Color.DarkGray;
            button3.ForeColor = Color.DarkGray;
            button4.ForeColor = Color.DarkGray;
            button5.ForeColor = Color.DarkGray;
        }
        else
        {
            cmdPause.ForeColor = Color.DarkGray;
            if (_lastGameSpeed != session.State.Speed || _lastGameIsPaused != session.State.IsPaused)
            {
                button1.ForeColor = Color.DarkGray;
                button2.ForeColor = Color.DarkGray;
                button3.ForeColor = Color.DarkGray;
                button4.ForeColor = Color.DarkGray;
                button5.ForeColor = Color.DarkGray;

                switch (session.State.Speed)
                {
                    case 1:
                        button1.ForeColor = Color.OrangeRed;
                        break;
                    case 2:
                        button2.ForeColor = Color.OrangeRed;
                        break;
                    case 3:
                        button3.ForeColor = Color.OrangeRed;
                        break;
                    case 4:
                        button4.ForeColor = Color.OrangeRed;
                        break;
                    case 5:
                        button5.ForeColor = Color.OrangeRed;
                        break;
                }
            }
        }

        _lastGameSpeed = session.State.Speed;
        _lastGameIsPaused = session.State.IsPaused;
    }

    private void cmdPause_Click(object sender, EventArgs e)
    {
        _gameManager.SessionPause();
        crlEmpty.Select();
    }

    private void button1_Click(object sender, EventArgs e)
    {
        _gameManager.SetGameSpeed(1);
        crlEmpty.Select();
    }

    private void button2_Click(object sender, EventArgs e)
    {
        _gameManager.SetGameSpeed(2);
        crlEmpty.Select();
    }

    private void button3_Click(object sender, EventArgs e)
    {
        _gameManager.SetGameSpeed(3);
        crlEmpty.Select();
    }

    private void button4_Click(object sender, EventArgs e)
    {
        _gameManager.SetGameSpeed(4);
        crlEmpty.Select();
    }

    private void button5_Click(object sender, EventArgs e)
    {
        _gameManager.SetGameSpeed(5);
        crlEmpty.Select();
    }
}
