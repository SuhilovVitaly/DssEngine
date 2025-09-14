using DeepSpaceSaga.UI.Controller.Services;
using DeepSpaceSaga.Common.Implementation.Entities.Dialogs;

namespace DeepSpaceSaga.UI.Screens.Dialogs;

public partial class DialogBasicInfoScreen : Form
{
    private GameActionEventDto? _gameActionEvent;
    private IGameManager? _gameManager;
    private DialogDto? _currentDialog;
    private string? _pendingMessageText;

    public event Action? OnClose;
    public event Action<DialogExit>? OnNextDialog;

    const int buttonHeight = 46;
    const int spacingBetweenButtons = 10;
    const int topMargin = 20;
    const int bottomMargin = 20;

    public DialogBasicInfoScreen()
    {
        InitializeComponent();
        InitializeTextOutput();
    }

    public DialogBasicInfoScreen(IGameManager gameManager)
    {
        InitializeComponent();
        InitializeTextOutput();

        _gameManager = gameManager;

        FormBorderStyle = FormBorderStyle.None;
        Size = new Size(1375, 875);
        ShowInTaskbar = false;
    }

    private void InitializeTextOutput()
    {
        // Subscribe to text output completion event
        crlMessage.TextOutputCompleted += OnTextOutputCompleted;
        
        // Subscribe to space pressed after completion event
        crlMessage.SpacePressedAfterCompletion += OnSpacePressedAfterCompletion;
        
        // Subscribe to Load event to ensure proper initialization
        this.Load += DialogBasicInfoScreen_Load;
    }

    private void DialogBasicInfoScreen_Load(object? sender, EventArgs e)
    {
        // If there's pending message text, set it now that the control is fully loaded
        if (!string.IsNullOrEmpty(_pendingMessageText))
        {
            System.Diagnostics.Debug.WriteLine($"DialogBasicInfoScreen_Load: Setting pending text '{_pendingMessageText}'");
            crlMessage.Clear();
            crlMessage.Text = _pendingMessageText;
            _pendingMessageText = null;
        }
    }

    public void ShowDialogEvent(GameActionEventDto gameActionEvent)
    {
        _gameActionEvent = gameActionEvent;
        _currentDialog = gameActionEvent.Dialog;
        
        // Clear any existing buttons first
        ClearDialogButtons();
        
        // Unsubscribe from previous events to avoid multiple subscriptions
        crlMessage.TextOutputCompleted -= OnTextOutputCompleted;
        crlMessage.SpacePressedAfterCompletion -= OnSpacePressedAfterCompletion;
        
        // Subscribe to text output completion event
        crlMessage.TextOutputCompleted += OnTextOutputCompleted;
        
        // Subscribe to space pressed after completion event
        crlMessage.SpacePressedAfterCompletion += OnSpacePressedAfterCompletion;
        
        // Use RPG text output control - set text after making visible
        var messageText = _gameManager?.Localization.GetText(_currentDialog?.Message ?? "") ?? _currentDialog?.Message ?? "";
        
        // Debug: Check if text output is working
        System.Diagnostics.Debug.WriteLine($"DialogBasicInfoScreen: Setting text '{messageText}' with speed {crlMessage.TextOutputSpeedMs}ms");
        
        // Store text for later - it will be set when form becomes visible
        _pendingMessageText = messageText;
        System.Diagnostics.Debug.WriteLine("DialogBasicInfoScreen: Storing text for later display");
    }

    private void OnTextOutputCompleted()
    {
        if (_currentDialog != null && _gameManager != null)
        {
            AddDialogButtons();
        }
    }
    
    private void OnSpacePressedAfterCompletion()
    {
        if (_currentDialog != null && _gameManager != null)
        {
            SimulateFirstButtonClick();
        }
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
            currentExit = 1;
        }

        // Calculate and set dynamic height for ExitButtonsContainer
        UpdateExitButtonsContainerHeight(currentExit);
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
        
        // Reset panel height to default
        ExitButtonsContainer.Height = 273; // Default height from designer
        ExitButtonsContainer.Location = new Point(ExitButtonsContainer.Location.X, 600); // Default location from designer
    }

    private void AddExitDialogButton(DialogExit exit, int currentExit)
    {
        var button = new Button
        {
            Size = new Size(1300, buttonHeight),
            Location = new Point(buttonHeight, 20 + currentExit * (buttonHeight + spacingBetweenButtons)),
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

    private void SimulateFirstButtonClick()
    {
        if (_currentDialog == null || _currentDialog.Exits.Count == 0)
        {
            // No exits available, create default exit
            AddDefaultExitDialogButton();
            return;
        }

        // Get the first exit (ordered by NextKey)
        var firstExit = _currentDialog.Exits.OrderBy(x => x.NextKey).First();
        AddExitDialogButton(firstExit, 0);
    }

    private void UpdateExitButtonsContainerHeight(int buttonCount)
    {
        if (buttonCount <= 0)
            return;
        
        int totalHeight = topMargin + (buttonCount * buttonHeight) + ((buttonCount - 1) * spacingBetweenButtons) + bottomMargin;
        
        // Update the height of ExitButtonsContainer
        ExitButtonsContainer.Height = totalHeight;
        
        // Update the location to keep it at the bottom
        ExitButtonsContainer.Location = new Point(ExitButtonsContainer.Location.X, this.Height - totalHeight);
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
