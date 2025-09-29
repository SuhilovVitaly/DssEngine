namespace DeepSpaceSaga.UI.Screens.CombatStage;

public partial class ScreenCombatStage : Form
{
    private GameActionEventDto? _gameActionEvent;
    private IGameManager? _gameManager;
    private DialogDto? _currentDialog;
    private ICloseCombatService _closeCombatService;

    public ScreenCombatStage()
    {
        InitializeComponent();
    }

    public ScreenCombatStage(IGameManager gameManager)
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

        _closeCombatService = Program.ServiceProvider.GetService<ICloseCombatService>();
        _closeCombatService.Initialization(_gameManager.GetMainCharacter(), _gameManager.GetCharacter(Int32.Parse(gameActionEvent.Dialog.Tag)));

        if (_currentDialog != null)
        {
            ShowGameScreen(gameActionEvent);
            ShowFightStage(gameActionEvent);
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        // Draw border
        using Pen borderPen = new Pen(UiConstants.FormBorderColor, UiConstants.FormBorderSize);
        Rectangle borderRect = new(
            UiConstants.FormBorderSize / 2,
            UiConstants.FormBorderSize / 2,
            Width - UiConstants.FormBorderSize,
            Height - UiConstants.FormBorderSize
        );
        e.Graphics.DrawRectangle(borderPen, borderRect);
    }

    private void crlMainMenu_Click(object sender, EventArgs e)
    {
        _gameManager?.Screens.CloseActiveDialogScreen();
    }

    private void ShowFightStage(GameActionEventDto gameActionEvent)
    {
        // Display combined fight status image
        SetFightStatusImage("0-0-0-0-0");
    }

    private void SetFightStatusImage(string overlayImageName)
    {
        try
        {
            // Load background image
            var backgroundImage = ImageLoader.LoadImageByName("command-center");
            
            // Load overlay image
            var overlayImage = ImageLoader.LoadImageByName(overlayImageName);
            
            // Create combined image
            var combinedImage = CombineImages(backgroundImage, overlayImage);
            
            // Set the image to pictureBox2 (center fight status area)
            picCurrentFightStartus.Image = combinedImage;
        }
        catch (Exception ex)
        {
            // Handle image loading errors gracefully
            System.Diagnostics.Debug.WriteLine($"Error loading fight status images: {ex.Message}");
        }
    }

    private Image CombineImages(Image background, Image overlay)
    {
        // Get the target size from picCurrentFightStartus PictureBox
        var targetSize = picCurrentFightStartus.Size;
        
        // Create a new bitmap with the target size
        var combinedBitmap = new Bitmap(targetSize.Width, targetSize.Height);
        
        using (var graphics = Graphics.FromImage(combinedBitmap))
        {
            // Set high quality rendering
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            
            // Draw background image scaled to target size
            graphics.DrawImage(background, 0, 0, targetSize.Width, targetSize.Height);
            
            // Draw overlay image scaled to target size with transparency support
            graphics.DrawImage(overlay, 0, 0, targetSize.Width, targetSize.Height);
        }
        
        return combinedBitmap;
    }

    private void ShowGameScreen(GameActionEventDto gameActionEvent)
    {
        crlName.Text = _closeCombatService.Pleer.FirstName + " " + _closeCombatService.Pleer.LastName;
        crlRank.Text = _gameManager.Localization.GetText(_closeCombatService.Pleer.Rank);
        crlPortrait.Image = ImageLoader.LoadCharacterImage(_closeCombatService.Pleer.Portrait);

        label1.Text = _closeCombatService.Enemy.FirstName + " " + _closeCombatService.Enemy.LastName;
        label2.Text = _gameManager.Localization.GetText(_closeCombatService.Enemy.Rank);
        pictureBox1.Image = ImageLoader.LoadCharacterImage(_closeCombatService.Enemy.Portrait);
    }
}
