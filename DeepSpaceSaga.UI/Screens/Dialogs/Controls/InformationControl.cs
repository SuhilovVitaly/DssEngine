namespace DeepSpaceSaga.UI.Screens.Dialogs.Controls;

public partial class InformationControl : UserControl
{
    public event Action? OnClose;
    public event Action<DialogExit>? OnNextDialog;
    
    private DialogDto? _currentDialog;
    private IGameManager? _currentGameManager;

    public InformationControl()
    {
        InitializeComponent();
        
        // Enable key handling for space key
        this.TabStop = true;
        
        // Subscribe to key events of child controls
        SubscribeToChildKeyEvents();
    }

    private void SubscribeToChildKeyEvents()
    {
        // Subscribe to key events of all child controls
        foreach (Control control in this.Controls)
        {
            control.KeyDown += (sender, e) => 
            {
                if (e.KeyCode == Keys.Space)
                {
                    crlMessage?.SkipTextOutput();
                    e.Handled = true;
                }
            };
        }
    }

    private void Event_ExitScreen(object? sender, EventArgs e)
    {
        OnClose?.Invoke();
        Visible = false;
    }

    private void Event_ToNextDialog(object? sender, EventArgs e)
    {
        var evnt = (sender as Button)?.Tag as DialogExit;

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
            // Clear any existing buttons first
            ClearDialogButtons();
            
            crlMessageTitle.Text = gameManager.Localization.GetText(dialog.Title);
            
            // Unsubscribe from previous events to avoid multiple subscriptions
            crlMessage.TextOutputCompleted -= OnTextOutputCompleted;
            crlMessage.SpacePressedAfterCompletion -= OnSpacePressedAfterCompletion;
            
            // Subscribe to text output completion event
            crlMessage.TextOutputCompleted += OnTextOutputCompleted;
            crlMessage.SpacePressedAfterCompletion += OnSpacePressedAfterCompletion;
            
            // Store dialog and gameManager for use in event handlers
            _currentDialog = dialog;
            _currentGameManager = gameManager;
            
            // Use RPG text output control
            crlMessage.Text = gameManager.Localization.GetText(dialog.Message);

            crlName.Text = dialog.Reporter.FirstName + " " + dialog.Reporter.LastName;
            crlRank.Text = gameManager.Localization.GetText(dialog.Reporter.Rank);

            crlPortrait.Image = ImageLoader.LoadCharacterImage(dialog.Reporter.Portrait);

            Visible = true;
        }
        catch (Exception)
        {

            throw;
        }        
    }

    private void OnTextOutputCompleted()
    {
        if (_currentDialog != null && _currentGameManager != null)
        {
            AddDialogButtons(_currentDialog, _currentGameManager);
        }
    }
    
    private void OnSpacePressedAfterCompletion()
    {
        if (_currentDialog != null && _currentGameManager != null)
        {
            SimulateFirstButtonClick(_currentDialog, _currentGameManager);
        }
    }
    
    private void ClearDialogButtons()
    {
        // Remove all buttons that were added dynamically
        var buttonsToRemove = Controls.OfType<Button>().Where(b => b.Name == "crlExitScreenButton").ToList();
        foreach (var button in buttonsToRemove)
        {
            Controls.Remove(button);
            button.Dispose();
        }
    }

    private void SimulateFirstButtonClick(DialogDto dialog, IGameManager gameManager)
    {
        // Find the first available exit
        var firstExit = dialog.Exits.OrderBy(x => x.NextKey).FirstOrDefault();
        
        if (firstExit != null)
        {
            // Simulate click on first button
            if (firstExit.NextKey != "-1")
            {
                OnNextDialog?.Invoke(firstExit);
            }
            else
            {
                OnClose?.Invoke();
            }
            Visible = false;
        }
        else
        {
            // No exits available, just close
            OnClose?.Invoke();
            Visible = false;
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

    private void AddDialogButtons(DialogDto dialog, IGameManager gameManager)
    {
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
    }
}
