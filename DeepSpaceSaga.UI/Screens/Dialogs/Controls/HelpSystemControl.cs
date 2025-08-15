using DeepSpaceSaga.Common.Abstractions.Entities.Dialogs;
using DeepSpaceSaga.Common.Implementation.Entities.Dialogs;

namespace DeepSpaceSaga.UI.Screens.Dialogs.Controls;

public partial class HelpSystemControl : UserControl
{
    public event Action? OnClose;
    public event Action<DialogExit> OnNextDialog;

    // Speed of text output in milliseconds between words
    public int TextOutputSpeedMs { get; set; } = 100;

    public HelpSystemControl()
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

    public void ShowGameScreen(DialogDto dialog, IGameManager gameManager)
    {
        try
        {
            crlMessageTitle.Text = gameManager.Localization.GetText(dialog.Title);
            
            // Start RPG-style text output instead of setting text immediately
            StartRpgTextOutput(gameManager.Localization.GetText(dialog.Message));

            crlName.Text = dialog.Reporter.FirstName + " " + dialog.Reporter.LastName;
            crlRank.Text = gameManager.Localization.GetText(dialog.Reporter.Rank);

            crlPortrait.Image = ImageLoader.LoadCharacterImage(dialog.Reporter.Portrait);

            int currentExit = 0;

            foreach (var exit in dialog.Exits.OrderBy(x => x.NextKey))
            {
                AddExitDialogButton(exit, dialog, gameManager, currentExit);

                currentExit++;
            }

            if (currentExit == 0)
            {
                AddDefaultExitDialogButton(dialog, gameManager);
            }

            Visible = true;
        }
        catch (Exception)
        {

            throw;
        }        
    }

    private async void StartRpgTextOutput(string fullText)
    {
        try
        {
            crlMessage.Text = "";
            var words = fullText.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (var word in words)
            {
                crlMessage.Text += word + " ";
                await Task.Delay(TextOutputSpeedMs);
            }
        }
        catch (Exception ex)
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
