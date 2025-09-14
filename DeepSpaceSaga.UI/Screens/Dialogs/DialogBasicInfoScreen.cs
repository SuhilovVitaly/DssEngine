using DeepSpaceSaga.UI.Controller.Services;
using DeepSpaceSaga.Common.Implementation.Entities.Dialogs;

namespace DeepSpaceSaga.UI.Screens.Dialogs;

public partial class DialogBasicInfoScreen : Form
{
    private GameActionEventDto? _gameActionEvent;
    private IGameManager? _gameManager;
    private DialogDto? _currentDialog;

    public event Action? OnClose;
    public event Action<DialogExit>? OnNextDialog;

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
        
        // Add dialog buttons to ExitButtonsContainer
        AddDialogButtons();
    }

    private void AddDialogButtons()
    {
        if (_currentDialog == null || _gameManager == null)
            return;

        // Clear existing buttons
        ClearDialogButtons();

        int currentExit = 0;

        foreach (var exit in _currentDialog.Exits.OrderBy(x => x.NextKey))
        {
            AddExitDialogButton(exit, currentExit);
            currentExit++;
        }

        if (currentExit == 0)
        {
            AddDefaultExitDialogButton();
        }
    }

    private void ClearDialogButtons()
    {
        // Remove all buttons that were added dynamically
        var buttonsToRemove = ExitButtonsContainer.Controls.OfType<Button>().Where(b => b.Name == "crlExitScreenButton").ToList();
        foreach (var button in buttonsToRemove)
        {
            ExitButtonsContainer.Controls.Remove(button);
            button.Dispose();
        }
    }

    private void AddExitDialogButton(DialogExit exit, int currentExit)
    {
        var button = new Button
        {
            Size = new Size(1300, 46),
            Location = new Point(46, 20 + currentExit * 50),
            BackColor = Color.FromArgb(18, 18, 18),
            Cursor = Cursors.Hand
        };
        button.FlatAppearance.BorderColor = Color.FromArgb(42, 42, 42);
        button.FlatAppearance.MouseDownBackColor = Color.FromArgb(78, 78, 78);
        button.FlatAppearance.MouseOverBackColor = Color.FromArgb(58, 58, 58);
        button.FlatStyle = FlatStyle.Flat;
        button.Font = new Font("Verdana", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
        button.ForeColor = Color.Gainsboro;
        button.Name = "crlExitScreenButton";
        button.TabIndex = 1;
        button.Text = _gameManager?.Localization.GetText(exit.TextKey) ?? exit.TextKey;
        button.UseVisualStyleBackColor = false;

        if (exit.NextKey != "-1")
        {
            button.Click += Event_ToNextDialog;
        }
        else
        {
            button.Click += Event_ExitScreen;
        }

        button.Tag = exit;

        ExitButtonsContainer.Controls.Add(button);
    }

    private void AddDefaultExitDialogButton()
    {
        var exit = new DialogExit
        {
            Key = "default",
            NextKey = "-1",
            TextKey = "CLOSE"
        };

        AddExitDialogButton(exit, 0);
    }

    private void Event_ExitScreen(object? sender, EventArgs e)
    {
        OnClose?.Invoke();
        Close();
    }

    private void Event_ToNextDialog(object? sender, EventArgs e)
    {
        var evnt = (sender as Button)?.Tag as DialogExit;

        if (evnt != null)
        {
            OnNextDialog?.Invoke(evnt);
        }

        Close();
    }

    private void crlMainMenu_Click(object sender, EventArgs e)
    {
        Close();
    }
}
