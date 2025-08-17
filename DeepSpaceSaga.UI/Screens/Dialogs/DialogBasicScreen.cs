using DeepSpaceSaga.Common.Abstractions.Entities.Commands;
using DeepSpaceSaga.Common.Abstractions.Entities.Dialogs;
using DeepSpaceSaga.Common.Abstractions.Events;
using DeepSpaceSaga.Common.Implementation.Entities.Commands;
using DeepSpaceSaga.Common.Implementation.Entities.Dialogs;
using DeepSpaceSaga.UI.Screens.Dialogs.Controls;

namespace DeepSpaceSaga.UI.Screens.Dialogs;

public partial class DialogBasicScreen : Form
{
    private GameActionEventDto _gameActionEvent;
    private IGameManager _gameManager;
    private DialogDto _currentDialog;

    public DialogBasicScreen()
    {
        InitializeComponent();
    }

    public DialogBasicScreen(GameActionEventDto gameActionEvent, IGameManager gameManager)
    {
        if(gameActionEvent.Dialog is null)
        {
            return;
        }

        _gameActionEvent = gameActionEvent;
        _gameManager = gameManager;
        _currentDialog = gameActionEvent.Dialog;

        var helpSystemControl = new HelpSystemControl
        {
            Dock = DockStyle.Fill
        };

        helpSystemControl.OnClose += Even_ScreenClose;
        helpSystemControl.OnNextDialog += Even_NextDialog;

        helpSystemControl.ShowGameScreen(gameActionEvent.Dialog, gameManager);

        Controls.Add(helpSystemControl);        
    }

    private void Even_NextDialog(DialogExit exit)
    {
         

        foreach (var dialog in _gameActionEvent.ConnectedDialogs)
        {
            if(dialog.Key == exit.NextKey)
            {
                switch (dialog.UiScreenType)
                {
                    case DialogUiScreenType.OnePerson:
                        AddHelpSystemControl(dialog);
                        break;
                    case DialogUiScreenType.TwoPerson:
                        AddTwoPersonControl(dialog, _currentDialog);
                        break;
                    default:
                        break;
                }

                _currentDialog = dialog;

                return;
            }
        }
    }

    private void AddTwoPersonControl(DialogDto currentDialog, DialogDto previousDialog)
    {
        var helpSystemControl = new HelpSystemTwoPersonesControl
        {
            Dock = DockStyle.Fill
        };

        helpSystemControl.OnClose += Even_ScreenClose;
        helpSystemControl.OnNextDialog += Even_NextDialog;

        helpSystemControl.ShowGameScreen(currentDialog, previousDialog, _gameManager);

        Controls.Add(helpSystemControl);

        if (Controls.Count > 1)
        {
            Controls.Remove(Controls[0]);
        }
    }

    private void AddHelpSystemControl(DialogDto dialog)
    {
        var helpSystemControl = new HelpSystemControl
        {
            Dock = DockStyle.Fill
        };

        helpSystemControl.OnClose += Even_ScreenClose;
        helpSystemControl.OnNextDialog += Even_NextDialog;

        helpSystemControl.ShowGameScreen(dialog, _gameManager);

        Controls.Add(helpSystemControl);

        if(Controls.Count > 1) 
        {
            Controls.Remove(Controls[0]);
        }
    }

    private void Even_ScreenClose()
    {
        // TODO: Last  exit key

        Close();
    }
}
