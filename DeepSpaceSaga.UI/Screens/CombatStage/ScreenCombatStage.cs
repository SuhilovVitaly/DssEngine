using DeepSpaceSaga.UI.Controller.Services;

namespace DeepSpaceSaga.UI.Screens.CombatStage
{
    public partial class ScreenCombatStage : Form
    {
        private GameActionEventDto? _gameActionEvent;
        private IGameManager? _gameManager;
        private DialogDto? _currentDialog;

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

        /// <summary>
        /// Sets the fight status image by combining background and overlay images
        /// </summary>
        /// <param name="overlayImageName">Name of the overlay image file (without extension)</param>
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

        /// <summary>
        /// Combines background and overlay images with transparency support
        /// </summary>
        /// <param name="background">Background image</param>
        /// <param name="overlay">Overlay image with transparency</param>
        /// <returns>Combined image</returns>
        private Image CombineImages(Image background, Image overlay)
        {
            // Create a new bitmap with the same size as background
            var combinedBitmap = new Bitmap(background.Width, background.Height);
            
            using (var graphics = Graphics.FromImage(combinedBitmap))
            {
                // Set high quality rendering
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                
                // Draw background image
                graphics.DrawImage(background, 0, 0, background.Width, background.Height);
                
                // Draw overlay image with transparency support
                graphics.DrawImage(overlay, 0, 0, overlay.Width, overlay.Height);
            }
            
            return combinedBitmap;
        }

        private void ShowGameScreen(GameActionEventDto gameActionEvent)
        {
            var mainCharacter = _gameManager.GetMainCharacter();

            
            crlName.Text = mainCharacter.FirstName + " " + mainCharacter.LastName;
            crlRank.Text = _gameManager.Localization.GetText(mainCharacter.Rank);
            crlPortrait.Image = ImageLoader.LoadCharacterImage(mainCharacter.Portrait);


            var enemyCharacter = _gameManager.GetCharacter(Int32.Parse(gameActionEvent.Dialog.Tag));

            label1.Text = enemyCharacter.FirstName + " " + enemyCharacter.LastName;
            label2.Text = _gameManager.Localization.GetText(enemyCharacter.Rank);
            pictureBox1.Image = ImageLoader.LoadCharacterImage(enemyCharacter.Portrait);

        }
    }
}
