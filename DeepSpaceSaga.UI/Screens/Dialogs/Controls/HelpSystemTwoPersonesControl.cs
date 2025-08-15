using DeepSpaceSaga.Common.Abstractions.Entities.Dialogs;
using DeepSpaceSaga.Common.Implementation.Entities.Dialogs;

namespace DeepSpaceSaga.UI.Screens.Dialogs.Controls;

public partial class HelpSystemTwoPersonesControl : UserControl
{
    public event Action? OnClose;
    public event Action<DialogExit> OnNextDialog;

    public HelpSystemTwoPersonesControl()
    {
        InitializeComponent();
    }

    private void Event_ExitScreen(object sender, EventArgs e)
    {
        OnClose?.Invoke();
        Visible = false;
    }

    private void Event_ToNextDialog(object sender, EventArgs e)
    {
        var evnt = (sender as Button).Tag as DialogExit;

        if(evnt != null)
        {
            OnNextDialog?.Invoke(evnt);
        }
        
        Visible = false;
    }

    public void ShowGameScreen(DialogDto currentDialog, DialogDto previousDialog, IGameManager gameManager)
    {
        try
        {
            crlMessageTitle.Text = gameManager.Localization.GetText(currentDialog.Title);
            
            // Previous message shows immediately
            crlMessagePrevious.Text = gameManager.Localization.GetText(previousDialog.Message);
            
            // Current message shows in RPG style using the control
            crlMessage.Text = gameManager.Localization.GetText(currentDialog.Message);

            crlNamePrevious.Text = currentDialog.Reporter.FirstName + " " + previousDialog.Reporter.LastName;
            crlRankPrevious.Text = gameManager.Localization.GetText(previousDialog.Reporter.Rank);

            crlName.Text = previousDialog.Reporter.FirstName + " " + currentDialog.Reporter.LastName;
            crlRank.Text = gameManager.Localization.GetText(currentDialog.Reporter.Rank);

            crlPortraitPrevious.Image = ImageLoader.LoadCharacterImage(previousDialog.Reporter.Portrait);
            crlPortrait.Image = ImageLoader.LoadCharacterImage(currentDialog.Reporter.Portrait);

            int currentExit = 0;

            foreach (var exit in currentDialog.Exits.OrderBy(x => x.NextKey))
            {
                AddExitDialogButton(exit, currentDialog, gameManager, currentExit);

                currentExit++;
            }

            if (currentExit == 0)
            {
                AddDefaultExitDialogButton(currentDialog, gameManager);
            }

            Visible = true;
        }
        catch (Exception)
        {

            throw;
        }        
    }

    private void AddDefaultExitDialogButton(DialogDto dialog, IGameManager gameManager)
    {
        var exit = new DialogExit
        { 
            Key = "-1",
            NextKey = "-1",
            TextKey = "dialog.exit.continue"
        };

        AddExitDialogButton(exit, dialog, gameManager, 0);
    }

    private void AddExitDialogButton(DialogExit exit, DialogDto dialog, IGameManager gameManager, int currentExit)
    {
        var button = new Button
        {
            Size = new Size(1300, 46),
            Location = new Point(46, 760 - currentExit * 50),
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
        button.Text = gameManager.Localization.GetText(exit.TextKey);
        button.UseVisualStyleBackColor = false;

        if(exit.NextKey != "-1")
        {
            button.Click += Event_ToNextDialog;
        }
        else
        {
            button.Click += Event_ExitScreen;
        }
        
        button.Tag = exit;

        Controls.Add(button);
    }
}
