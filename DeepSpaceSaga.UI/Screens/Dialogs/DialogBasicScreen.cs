using DeepSpaceSaga.Common.Abstractions.Entities.Commands;
using DeepSpaceSaga.Common.Abstractions.Entities.Dialogs;
using DeepSpaceSaga.Common.Abstractions.Events;
using DeepSpaceSaga.Common.Implementation.Entities.Commands;
using DeepSpaceSaga.Common.Implementation.Entities.Dialogs;
using DeepSpaceSaga.UI.Controller.Services;
using DeepSpaceSaga.UI.Screens.Dialogs.Controls;

namespace DeepSpaceSaga.UI.Screens.Dialogs;

public partial class DialogBasicScreen : Form
{
    private GameActionEventDto? _gameActionEvent;
    private IGameManager? _gameManager;
    private DialogDto? _currentDialog;

    public DialogBasicScreen()
    {
        InitializeComponent();
    }

    public DialogBasicScreen(IGameManager gameManager)
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

        if (_currentDialog != null)
        {
            AddHelpSystemControl(_currentDialog);
        }
    }    

    private void Even_NextDialog(DialogExit exit)
    {
        if (_gameManager != null && _currentDialog != null)
        {
            _gameManager.CommandExecute(new DialogExitCommand
            {
                Category = Common.Abstractions.Entities.Commands.CommandCategory.DialogExit,
                IsPauseProcessed = true,
                IsOneTimeCommand = true,
                DialogExitKey = exit.Key,
                DialogKey = _currentDialog.Key,
                DialogCommands = exit.DialogCommands,
            } );
        }

        if (_gameActionEvent != null)
        {
            foreach (var dialog in _gameActionEvent.ConnectedDialogs)
            {
                if(dialog.Key == exit.NextKey)
                {
                    switch (dialog.UiScreenType)
                    {
                        case GameEventUiScreenType.DialogOnePerson:
                            AddHelpSystemControl(dialog);
                            break;
                        case GameEventUiScreenType.DialogTwoPerson:
                            if (_currentDialog != null)
                            {
                                AddTwoPersonControl(dialog, _currentDialog);
                            }
                            break;
                        default:
                            break;
                    }

                    _currentDialog = dialog;

                    return;
                }
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

        if (_gameManager != null)
        {
            helpSystemControl.ShowGameScreen(currentDialog, previousDialog, _gameManager);
        }

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

        if (_gameManager != null)
        {
            helpSystemControl.ShowGameScreen(dialog, _gameManager);
        }

        Controls.Add(helpSystemControl);

        if(Controls.Count > 1) 
        {
            Controls.Remove(Controls[0]);
        }

        // Set focus to the new control to enable keyboard handling
        helpSystemControl.Focus();
    }

    private void Even_ScreenClose()
    {
        // TODO: Add close exit dialog key
        Close();
    }
}
