using DeepSpaceSaga.UI.Controller.Services;
using DeepSpaceSaga.Common.Implementation.Entities.Dialogs;
using DeepSpaceSaga.UI.Services.Screens;

namespace DeepSpaceSaga.UI.Screens.Dialogs;

public partial class DialogBasicInfoScreen : Form
{
    private GameActionEventDto? _gameActionEvent;
    private IGameManager? _gameManager;
    private readonly IScreensService _screensService;
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
    }

    public DialogBasicInfoScreen(IGameManager gameManager, IScreensService screensService)
    {
        InitializeComponent();

        _gameManager = gameManager ?? throw new ArgumentNullException(nameof(gameManager));
        _screensService = screensService ?? throw new ArgumentNullException(nameof(screensService));

        FormBorderStyle = FormBorderStyle.None;
        Size = new Size(1375, 875);
        ShowInTaskbar = false;        
    }   

    public void ShowDialogEvent(GameActionEventDto gameActionEvent)
    {
        _gameActionEvent = gameActionEvent;
        _currentDialog = gameActionEvent.Dialog;

        var messageText = _gameManager?.Localization.GetText(_currentDialog?.Message ?? "") ?? _currentDialog?.Message ?? "";

        crlTitle.Text = _gameManager?.Localization.GetText(_currentDialog?.Title ?? "") ?? _currentDialog?.Message ?? "";

        crlMessageStatic.Text = messageText.Replace("<BR>", Environment.NewLine + Environment.NewLine);

        panel1.BackgroundImage = CreateCompositeBackgroundImage(panel1.Size, _currentDialog?.Image);

        AddDialogButtons();
    }

    private void AddDialogButtons()
    {
        if (_currentDialog == null || _gameManager == null)
            return;

        int currentExit = 0;

        foreach (var exit in _currentDialog.Exits.OrderByDescending(x => x.NextKey))
        {
            AddExitDialogButton(exit, currentExit);
            currentExit++;
        }

        if (currentExit == 0)
        {
            AddDefaultExitDialogButton();
            currentExit = 1;
        }
    }

    private void AddExitDialogButton(DialogExit exit, int currentExit)
    {
        var height = this.Height - bottomMargin - (currentExit + 1) * (buttonHeight + spacingBetweenButtons);
        
        // Calculate button width and position for centering
        int buttonWidth = this.Width - (buttonHeight * 2); // Full width minus margins
        int buttonX = buttonHeight; // Left margin

        var button = new Button
        {
            Size = new Size(buttonWidth, buttonHeight),
            Location = new Point(buttonX, height),
            BackColor = Color.FromArgb(18, 18, 18),
            Cursor = Cursors.Hand,
            TabStop = false
        };
        button.FlatAppearance.BorderColor = Color.FromArgb(42, 42, 42);
        button.FlatAppearance.MouseDownBackColor = Color.FromArgb(78, 78, 78);
        button.FlatAppearance.MouseOverBackColor = Color.FromArgb(58, 58, 58);
        button.FlatStyle = FlatStyle.Flat;
        button.Font = new Font("Verdana", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
        button.ForeColor = Color.Gainsboro;
        button.Name = "crlExitScreenButton";
        button.TabIndex = 1000;
        button.Text = _gameManager?.Localization.GetText(exit.TextKey) ?? exit.TextKey;
        //button.UseVisualStyleBackColor = false;

        if (exit.NextKey != "-1")
        {
            button.Click += Event_ToNextDialog;
        }
        else
        {
            button.Click += Event_ExitScreen;
        }

        button.Tag = exit;

        

        this.Controls.Add(button);

        button.BringToFront();
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

            foreach (var dialog in _gameActionEvent.ConnectedDialogs)
            {
                if(dialog.Key == evnt.NextKey)
                {
                    var gameEvent = _gameManager.GetGameActionEvent(dialog.Key);
                    Close();
                    _screensService.ShowDialogScreen(gameEvent);
                }
            }
        }        
    }

    /// <summary>
    /// Creates a composite background image with black background and a square image on the right side
    /// </summary>
    /// <param name="panelSize">Size of the panel to create the image for</param>
    /// <param name="imageName">Name of the image to load and place on the right side</param>
    /// <returns>Composite image with black background and scaled square image on the right</returns>
    private Image CreateCompositeBackgroundImage(Size panelSize, string? imageName)
    {
        // Create a black background image
        var compositeImage = new Bitmap(panelSize.Width, panelSize.Height);
        
        using (var graphics = Graphics.FromImage(compositeImage))
        {
            // Set high quality rendering settings
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            
            // Fill with black background
            graphics.FillRectangle(Brushes.Black, 0, 0, panelSize.Width, panelSize.Height);
            
            // Load and draw the square image on the right side if imageName is provided
            if (!string.IsNullOrEmpty(imageName))
            {
                try
                {
                    using (var originalImage = ImageLoader.LoadImageByName(imageName))
                    {
                        // Calculate the size for the square image to fit the panel height
                        int squareSize = panelSize.Height;
                        
                        // Calculate position to place the image on the right side
                        int xPosition = panelSize.Width - squareSize;
                        int yPosition = 0;
                        
                        // Crop image with 10 pixel margin from each edge
                        using (var croppedImage = CropImageWithMargin(originalImage, 10))
                        {
                            // Apply dark fade effect to the cropped image
                            ApplyDarkFadeToImage(croppedImage);
                            
                            // Draw the scaled square image on the right side
                            graphics.DrawImage(croppedImage, xPosition, yPosition, squareSize, squareSize);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log the error but don't crash - just show black background
                    System.Diagnostics.Debug.WriteLine($"Failed to load image '{imageName}': {ex.Message}");
                }
            }
        }
        
        return compositeImage;
    }
    
    /// <summary>
    /// Applies dark fade effect to the original image from center to edges
    /// </summary>
    /// <param name="image">The image to apply dark fade effect to</param>
    private void ApplyDarkFadeToImage(Image image)
    {
        using (var graphics = Graphics.FromImage(image))
        {
            // Set high quality rendering settings
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            
            // Use the actual image dimensions
            int imageWidth = image.Width;
            int imageHeight = image.Height;
            
            // Calculate center point based on actual image size
            int centerX = imageWidth / 2;
            int centerY = imageHeight / 2;
            
            // Top gradient
            using (var topBrush = new System.Drawing.Drawing2D.LinearGradientBrush(
                new Point(centerX, 0), new Point(centerX, centerY),
                Color.Black, Color.Transparent))
            {
                graphics.FillRectangle(topBrush, 0, 0, imageWidth, centerY);
            }
            
            // Bottom gradient
            using (var bottomBrush = new System.Drawing.Drawing2D.LinearGradientBrush(
                new Point(centerX, centerY), new Point(centerX, imageHeight),
                Color.Transparent, Color.Black))
            {
                graphics.FillRectangle(bottomBrush, 0, centerY, imageWidth, centerY);
            }
            
            // Left gradient
            using (var leftBrush = new System.Drawing.Drawing2D.LinearGradientBrush(
                new Point(0, centerY), new Point(centerX, centerY),
                Color.Black, Color.Transparent))
            {
                graphics.FillRectangle(leftBrush, 0, 0, centerX, imageHeight);
            }
            
            // Right gradient
            using (var rightBrush = new System.Drawing.Drawing2D.LinearGradientBrush(
                new Point(centerX, centerY), new Point(imageWidth, centerY),
                Color.Transparent, Color.Black))
            {
                graphics.FillRectangle(rightBrush, centerX, 0, centerX, imageHeight);
            }
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
        // Calculate crop dimensions
        int cropWidth = Math.Max(1, originalImage.Width - (margin * 2));
        int cropHeight = Math.Max(1, originalImage.Height - (margin * 2));
        
        // Ensure we don't crop more than the image size
        int actualMarginX = Math.Min(margin, originalImage.Width / 2);
        int actualMarginY = Math.Min(margin, originalImage.Height / 2);
        
        // Create cropped image
        var croppedImage = new Bitmap(cropWidth, cropHeight);
        
        using (var graphics = Graphics.FromImage(croppedImage))
        {
            // Set high quality rendering settings
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            
            // Draw the cropped portion of the original image
            var sourceRect = new Rectangle(actualMarginX, actualMarginY, cropWidth, cropHeight);
            var destRect = new Rectangle(0, 0, cropWidth, cropHeight);
            
            graphics.DrawImage(originalImage, destRect, sourceRect, GraphicsUnit.Pixel);
        }
        
        return croppedImage;
    }    
}
