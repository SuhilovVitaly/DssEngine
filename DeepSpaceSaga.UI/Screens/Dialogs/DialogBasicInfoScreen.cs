using DeepSpaceSaga.Common.Abstractions.Entities.Commands;
using DeepSpaceSaga.Common.Abstractions.Entities.Dialogs;
using DeepSpaceSaga.Common.Abstractions.Events;
using DeepSpaceSaga.Common.Implementation.Entities.Commands;
using DeepSpaceSaga.Common.Implementation.Entities.Dialogs;
using DeepSpaceSaga.UI.Screens.Dialogs.Controls;

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
        BackColor = Color.Black;
    }

    public DialogBasicInfoScreen(GameActionEventDto gameActionEvent, IGameManager gameManager)
    {
        if(gameActionEvent.Dialog is null)
        {
            return;
        }

        _gameActionEvent = gameActionEvent;
        _gameManager = gameManager;
        _currentDialog = gameActionEvent.Dialog;

        //BackColor = Color.Black;
    }
}
