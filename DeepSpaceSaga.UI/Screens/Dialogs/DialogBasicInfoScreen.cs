namespace DeepSpaceSaga.UI.Screens.Dialogs;

/// <summary>
/// Represents a modal dialog screen for displaying game action events with interactive buttons
/// </summary>
public partial class DialogBasicInfoScreen : Form
{
    #region Constants
    
    private const int ButtonHeight = 46;
    private const int SpacingBetweenButtons = 10;
    private const int TopMargin = 20;
    private const int BottomMargin = 20;
    private const int ImageMargin = 10;
    private const string DynamicButtonName = "crlExitScreenButton";
    private const string DefaultExitKey = "-1";
    private const string DefaultTextKey = "CLOSE";
    
    #endregion

    #region Fields

    private GameActionEventDto? _gameActionEvent;
    private IGameManager? _gameManager;
    private readonly IScreensService? _screensService;
    private DialogDto? _currentDialog;

    #endregion

    #region Events

    public event Action? OnClose;
    public event Action<DialogExit, DialogDto>? OnDialogChoice;

    #endregion

    #region Constructors

    public DialogBasicInfoScreen()
    {
        InitializeComponent();

        if (Program.ServiceProvider is null) return;

        _gameManager = Program.ServiceProvider.GetService<IGameManager>();
        _screensService = Program.ServiceProvider.GetService<IScreensService>();        

        InitializeForm();
        SetupCursors();
    }

    //public DialogBasicInfoScreen(IGameManager gameManager, IScreensService screensService)
    //{
    //    InitializeComponent();

    //    _gameManager = gameManager ?? throw new ArgumentNullException(nameof(gameManager));
    //    _screensService = screensService ?? throw new ArgumentNullException(nameof(screensService));

    //    InitializeForm();
    //    SetupCursors();
    //}

    #endregion

    #region Public Methods

    /// <summary>
    /// Displays the dialog with the specified game action event
    /// </summary>
    /// <param name="gameActionEvent">The game action event to display</param>
    public void ShowDialogEvent(GameActionEventDto gameActionEvent)
    {
        if (gameActionEvent == null)
            throw new ArgumentNullException(nameof(gameActionEvent));

        _gameActionEvent = gameActionEvent;
        _currentDialog = gameActionEvent.Dialog;

        UpdateDialogContent();
        AddDialogButtons();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Initializes the form properties
    /// </summary>
    private void InitializeForm()
    {
        FormBorderStyle = FormBorderStyle.None;
        Size = new Size(1375, 875);
        ShowInTaskbar = false;
    }

    /// <summary>
    /// Updates the dialog content with localized text and background image
    /// </summary>
    private void UpdateDialogContent()
    {
        if (_currentDialog == null || _gameManager == null)
            return;

        var messageText = GetLocalizedText(_currentDialog.Message);
        var titleText = GetLocalizedText(_currentDialog.Title);

        crlTitle.Text = titleText;
        crlMessageStatic.Text = FormatMessageText(messageText);
        panel1.BackgroundImage = CreateCompositeBackgroundImage(panel1.Size, _currentDialog.Image);
    }

    /// <summary>
    /// Gets localized text using the game manager's localization service
    /// </summary>
    /// <param name="textKey">The text key to localize</param>
    /// <returns>Localized text or the original key if localization fails</returns>
    private string GetLocalizedText(string? textKey)
    {
        if (string.IsNullOrEmpty(textKey) || _gameManager == null)
            return textKey ?? string.Empty;

        return _gameManager.Localization.GetText(textKey) ?? textKey;
    }

    /// <summary>
    /// Formats message text by replacing HTML line breaks with newlines
    /// </summary>
    /// <param name="messageText">The message text to format</param>
    /// <returns>Formatted message text</returns>
    private string FormatMessageText(string messageText)
    {
        return messageText.Replace("<BR>", Environment.NewLine + Environment.NewLine);
    }

    #endregion

    #region Button Management

    /// <summary>
    /// Adds dialog buttons based on the current dialog's exits
    /// </summary>
    private void AddDialogButtons()
    {
        if (_currentDialog == null || _gameManager == null)
            return;

        ClearDialogButtons();

        var exits = _currentDialog.Exits.OrderByDescending(x => x.NextKey).ToList();
        
        for (int i = 0; i < exits.Count; i++)
        {
            AddExitDialogButton(exits[i], i);
        }

        if (exits.Count == 0)
        {
            AddDefaultExitDialogButton();
        }

        ApplyCursorsToDynamicButtons();
    }

    /// <summary>
    /// Creates and adds an exit dialog button
    /// </summary>
    /// <param name="exit">The dialog exit configuration</param>
    /// <param name="buttonIndex">The index of the button for positioning</param>
    private void AddExitDialogButton(DialogExit exit, int buttonIndex)
    {
        var button = CreateDialogButton(exit, buttonIndex);
        ConfigureButtonEvents(button, exit);
        
        Controls.Add(button);
        button.BringToFront();
    }

    /// <summary>
    /// Creates a dialog button with the specified configuration
    /// </summary>
    /// <param name="exit">The dialog exit configuration</param>
    /// <param name="buttonIndex">The index of the button for positioning</param>
    /// <returns>Configured button</returns>
    private Button CreateDialogButton(DialogExit exit, int buttonIndex)
    {
        var position = CalculateButtonPosition(buttonIndex);
        var size = CalculateButtonSize();

        var button = new Button
        {
            Size = size,
            Location = position,
            Name = DynamicButtonName,
            Text = GetLocalizedText(exit.TextKey),
            Tag = exit,
            TabIndex = 1000,
            TabStop = false
        };

        ApplyButtonStyling(button);
        return button;
    }

    /// <summary>
    /// Calculates the position for a button based on its index
    /// </summary>
    /// <param name="buttonIndex">The index of the button</param>
    /// <returns>Button position</returns>
    private Point CalculateButtonPosition(int buttonIndex)
    {
        var height = Height - BottomMargin - (buttonIndex + 1) * (ButtonHeight + SpacingBetweenButtons);
        var x = ButtonHeight; // Left margin
        return new Point(x, height);
    }

    /// <summary>
    /// Calculates the size for dialog buttons
    /// </summary>
    /// <returns>Button size</returns>
    private Size CalculateButtonSize()
    {
        var width = Width - (ButtonHeight * 2); // Full width minus margins
        return new Size(width, ButtonHeight);
    }

    /// <summary>
    /// Applies styling to a dialog button
    /// </summary>
    /// <param name="button">The button to style</param>
    private void ApplyButtonStyling(Button button)
    {
        button.BackColor = Color.FromArgb(18, 18, 18);
        button.Cursor = CursorManager.SelectedCursor;
        button.FlatStyle = FlatStyle.Flat;
        button.Font = new Font("Verdana", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
        button.ForeColor = Color.Gainsboro;

        button.FlatAppearance.BorderColor = Color.FromArgb(42, 42, 42);
        button.FlatAppearance.MouseDownBackColor = Color.FromArgb(78, 78, 78);
        button.FlatAppearance.MouseOverBackColor = Color.FromArgb(58, 58, 58);
    }

    /// <summary>
    /// Configures click events for a dialog button
    /// </summary>
    /// <param name="button">The button to configure</param>
    /// <param name="exit">The dialog exit configuration</param>
    private void ConfigureButtonEvents(Button button, DialogExit exit)
    {
        if (exit.NextKey != DefaultExitKey)
        {
            button.Click += Event_ToNextDialog;
        }
        else
        {
            button.Click += Event_ExitScreen;
        }
    }

    /// <summary>
    /// Adds a default exit dialog button when no exits are available
    /// </summary>
    private void AddDefaultExitDialogButton()
    {
        var defaultExit = new DialogExit
        {
            Key = "default",
            NextKey = DefaultExitKey,
            TextKey = DefaultTextKey
        };

        AddExitDialogButton(defaultExit, 0);
    }

    /// <summary>
    /// Clears all dynamically created dialog buttons
    /// </summary>
    private void ClearDialogButtons()
    {
        var buttonsToRemove = Controls.OfType<Button>()
            .Where(btn => btn.Name == DynamicButtonName)
            .ToList();

        foreach (var button in buttonsToRemove)
        {
            Controls.Remove(button);
            button.Dispose();
        }
    }

    #endregion

    #region Event Handlers

    /// <summary>
    /// Handles the exit screen button click event
    /// </summary>
    /// <param name="sender">The event sender</param>
    /// <param name="e">Event arguments</param>
    private void Event_ExitScreen(object? sender, EventArgs e)
    {
        //Close();
        _screensService.CloseActiveDialogScreen();
    }

    /// <summary>
    /// Handles the next dialog button click event
    /// </summary>
    /// <param name="sender">The event sender</param>
    /// <param name="e">Event arguments</param>
    private void Event_ToNextDialog(object? sender, EventArgs e)
    {
        var exit = (sender as Button)?.Tag as DialogExit;

        if (exit == null || _gameActionEvent == null || _gameManager == null)
        {
            //Close();
            return;
        }

        OnDialogChoice?.Invoke(exit, _currentDialog);       
        //Close();
    }

    /// <summary>
    /// Finds the next dialog based on the exit key
    /// </summary>
    /// <param name="nextKey">The key of the next dialog</param>
    /// <returns>The next dialog or null if not found</returns>
    private DialogDto? FindNextDialog(string nextKey)
    {
        return _gameActionEvent?.ConnectedDialogs
            .FirstOrDefault(dialog => dialog.Key == nextKey);
    }


    #endregion

    #region Image Processing

    /// <summary>
    /// Creates a composite background image with black background and a square image on the right side
    /// </summary>
    /// <param name="panelSize">Size of the panel to create the image for</param>
    /// <param name="imageName">Name of the image to load and place on the right side</param>
    /// <returns>Composite image with black background and scaled square image on the right</returns>
    private Image CreateCompositeBackgroundImage(Size panelSize, string? imageName)
    {
        var compositeImage = new Bitmap(panelSize.Width, panelSize.Height);
        
        using (var graphics = Graphics.FromImage(compositeImage))
        {
            ConfigureGraphicsQuality(graphics);
            DrawBlackBackground(graphics, panelSize);
            
            if (!string.IsNullOrEmpty(imageName))
            {
                DrawBackgroundImage(graphics, panelSize, imageName);
            }
        }
        
        return compositeImage;
    }

    /// <summary>
    /// Configures graphics for high quality rendering
    /// </summary>
    /// <param name="graphics">The graphics object to configure</param>
    private void ConfigureGraphicsQuality(Graphics graphics)
    {
        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
    }

    /// <summary>
    /// Draws a black background
    /// </summary>
    /// <param name="graphics">The graphics object to draw on</param>
    /// <param name="panelSize">The size of the panel</param>
    private void DrawBlackBackground(Graphics graphics, Size panelSize)
    {
        graphics.FillRectangle(Brushes.Black, 0, 0, panelSize.Width, panelSize.Height);
    }

    /// <summary>
    /// Draws the background image on the right side of the panel
    /// </summary>
    /// <param name="graphics">The graphics object to draw on</param>
    /// <param name="panelSize">The size of the panel</param>
    /// <param name="imageName">The name of the image to load</param>
    private void DrawBackgroundImage(Graphics graphics, Size panelSize, string imageName)
    {
        try
        {
            using (var originalImage = ImageLoader.LoadImageByName(imageName))
            {
                var imagePosition = CalculateImagePosition(panelSize);
                using (var processedImage = ProcessImageForBackground(originalImage))
                {
                    graphics.DrawImage(processedImage, imagePosition.X, imagePosition.Y, 
                                     imagePosition.Width, imagePosition.Height);
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to load image '{imageName}': {ex.Message}");
        }
    }

    /// <summary>
    /// Calculates the position and size for the background image
    /// </summary>
    /// <param name="panelSize">The size of the panel</param>
    /// <returns>Rectangle with position and size for the image</returns>
    private Rectangle CalculateImagePosition(Size panelSize)
    {
        int squareSize = panelSize.Height;
        int xPosition = panelSize.Width - squareSize;
        return new Rectangle(xPosition, 0, squareSize, squareSize);
    }

    /// <summary>
    /// Processes an image for use as background (crops and applies fade effect)
    /// </summary>
    /// <param name="originalImage">The original image to process</param>
    /// <returns>Processed image</returns>
    private Image ProcessImageForBackground(Image originalImage)
    {
        using (var croppedImage = CropImageWithMargin(originalImage, ImageMargin))
        {
            ApplyDarkFadeToImage(croppedImage);
            return new Bitmap(croppedImage);
        }
    }
    
    /// <summary>
    /// Applies dark fade effect to the original image from center to edges
    /// </summary>
    /// <param name="image">The image to apply dark fade effect to</param>
    private void ApplyDarkFadeToImage(Image image)
    {
        using (var graphics = Graphics.FromImage(image))
        {
            ConfigureGraphicsQuality(graphics);
            
            var imageSize = new Size(image.Width, image.Height);
            var center = new Point(imageSize.Width / 2, imageSize.Height / 2);
            
            ApplyGradientFade(graphics, imageSize, center);
        }
    }

    /// <summary>
    /// Applies gradient fade effects to all four sides of the image
    /// </summary>
    /// <param name="graphics">The graphics object to draw on</param>
    /// <param name="imageSize">The size of the image</param>
    /// <param name="center">The center point of the image</param>
    private void ApplyGradientFade(Graphics graphics, Size imageSize, Point center)
    {
        // Top gradient
        using (var topBrush = new System.Drawing.Drawing2D.LinearGradientBrush(
            new Point(center.X, 0), new Point(center.X, center.Y),
            Color.Black, Color.Transparent))
        {
            graphics.FillRectangle(topBrush, 0, 0, imageSize.Width, center.Y);
        }
        
        // Bottom gradient
        using (var bottomBrush = new System.Drawing.Drawing2D.LinearGradientBrush(
            new Point(center.X, center.Y), new Point(center.X, imageSize.Height),
            Color.Transparent, Color.Black))
        {
            graphics.FillRectangle(bottomBrush, 0, center.Y, imageSize.Width, center.Y);
        }
        
        // Left gradient
        using (var leftBrush = new System.Drawing.Drawing2D.LinearGradientBrush(
            new Point(0, center.Y), new Point(center.X, center.Y),
            Color.Black, Color.Transparent))
        {
            graphics.FillRectangle(leftBrush, 0, 0, center.X, imageSize.Height);
        }
        
        // Right gradient
        using (var rightBrush = new System.Drawing.Drawing2D.LinearGradientBrush(
            new Point(center.X, center.Y), new Point(imageSize.Width, center.Y),
            Color.Transparent, Color.Black))
        {
            graphics.FillRectangle(rightBrush, center.X, 0, center.X, imageSize.Height);
        }
    }
    
    /// <summary>
    /// Crops an image with specified margin from each edge
    /// </summary>
    /// <param name="originalImage">The original image to crop</param>
    /// <param name="margin">Margin in pixels to remove from each edge</param>
    /// <returns>Cropped image with margin removed</returns>
    private Image CropImageWithMargin(Image originalImage, int margin)
    {
        var cropDimensions = CalculateCropDimensions(originalImage, margin);
        var croppedImage = new Bitmap(cropDimensions.Width, cropDimensions.Height);
        
        using (var graphics = Graphics.FromImage(croppedImage))
        {
            ConfigureGraphicsQuality(graphics);
            
            var sourceRect = new Rectangle(cropDimensions.MarginX, cropDimensions.MarginY, 
                                         cropDimensions.Width, cropDimensions.Height);
            var destRect = new Rectangle(0, 0, cropDimensions.Width, cropDimensions.Height);
            
            graphics.DrawImage(originalImage, destRect, sourceRect, GraphicsUnit.Pixel);
        }
        
        return croppedImage;
    }

    /// <summary>
    /// Calculates crop dimensions for an image with margin
    /// </summary>
    /// <param name="originalImage">The original image</param>
    /// <param name="margin">The margin to apply</param>
    /// <returns>Crop dimensions</returns>
    private (int Width, int Height, int MarginX, int MarginY) CalculateCropDimensions(Image originalImage, int margin)
    {
        int cropWidth = Math.Max(1, originalImage.Width - (margin * 2));
        int cropHeight = Math.Max(1, originalImage.Height - (margin * 2));
        int actualMarginX = Math.Min(margin, originalImage.Width / 2);
        int actualMarginY = Math.Min(margin, originalImage.Height / 2);
        
        return (cropWidth, cropHeight, actualMarginX, actualMarginY);
    }

    #endregion

    #region Cursor Management

    /// <summary>
    /// Sets up cursors for the form and its controls
    /// </summary>
    private void SetupCursors()
    {
        Cursor = CursorManager.DefaultCursor;
        
        SetControlCursors();
        CursorManager.SetDefaultCursorForControl(this);
    }

    /// <summary>
    /// Sets cursors for specific controls
    /// </summary>
    private void SetControlCursors()
    {
        if (crlTitle != null)
        {
            crlTitle.Cursor = CursorManager.SelectedCursor;
        }
        
        if (crlMessageStatic != null)
        {
            crlMessageStatic.Cursor = CursorManager.SelectedCursor;
        }
    }

    /// <summary>
    /// Applies cursors to all dynamically created buttons
    /// </summary>
    private void ApplyCursorsToDynamicButtons()
    {
        var dynamicButtons = Controls.OfType<Button>()
            .Where(btn => btn.Name == DynamicButtonName);

        foreach (var button in dynamicButtons)
        {
            button.Cursor = CursorManager.SelectedCursor;
        }
    }

    #endregion
}
