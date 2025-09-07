using DeepSpaceSaga.UI.Controller.Services;

namespace DeepSpaceSaga.UI.Screens.Dialogs;

public partial class DialogBasicInfoScreen : Form
{
    private GameActionEventDto _gameActionEvent;
    private IGameManager _gameManager;
    private DialogDto _currentDialog;

    public DialogBasicInfoScreen()
    {
        InitializeComponent();
    }

    public DialogBasicInfoScreen(IGameManager gameManager)
    {
        InitializeComponent();

        _gameManager = gameManager;

        FormBorderStyle = FormBorderStyle.None;
        Size = new Size(1375, 875);
        ShowInTaskbar = false;
    }

    public void ShowDialogEvent(GameActionEventDto gameActionEvent)
    {
        _gameActionEvent = gameActionEvent;
        _currentDialog = gameActionEvent.Dialog;
    }

    private void crlMainMenu_Click(object sender, EventArgs e)
    {
        Close();
    }
}
