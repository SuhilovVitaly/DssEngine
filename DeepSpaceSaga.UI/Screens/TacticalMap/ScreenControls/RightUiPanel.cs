namespace DeepSpaceSaga.UI.Screens.TacticalMap.ScreenControls;

public partial class RightUiPanel : UserControl
    {
        public RightUiPanel()
        {
            InitializeComponent();
            LoadImages();
        }

        private void LoadImages()
        {
            try
            {
                crlCloseRightPanel.NormalImage = Image.FromFile(@"Images/Window/close.png");
                crlCloseRightPanel.SelectedImage = Image.FromFile(@"Images/Window/close-selected.png");
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
    }
