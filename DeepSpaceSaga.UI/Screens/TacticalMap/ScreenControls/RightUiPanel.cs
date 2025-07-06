using DeepSpaceSaga.UI.Tools;

namespace DeepSpaceSaga.UI.Screens.TacticalMap.ScreenControls;

public partial class RightUiPanel : UserControl
    {
        public RightUiPanel()
        {
            InitializeComponent();
            LoadImages();
            Cursor = CursorManager.DefaultCursor;
        }

        private void LoadImages()
        {
        // Skip loading images in design mode
        if (DesignModeChecker.IsInDesignMode()) return;

        try
            {
                var closeImagePath = Path.Combine(Application.StartupPath, "Images", "Window", "close.png");
                var closeSelectedImagePath = Path.Combine(Application.StartupPath, "Images", "Window", "close-selected.png");
                
                crlCloseRightPanel.NormalImage = Image.FromFile(closeImagePath);
                crlCloseRightPanel.SelectedImage = Image.FromFile(closeSelectedImagePath);
            }
            catch (Exception ex)
            {
                // Handle image loading errors
                MessageBox.Show($"Error loading close button images: {ex.Message}");
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Hide();
        }

    public void ShowPanel(RightPanelContentType contentType)
    {
        Show();
    }        
}
