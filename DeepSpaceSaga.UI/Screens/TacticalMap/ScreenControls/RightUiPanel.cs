namespace DeepSpaceSaga.UI.Screens.TacticalMap.ScreenControls;

public partial class RightUiPanel : UserControl
    {
        private Image normalCloseImage;
        private Image selectedCloseImage;

        public RightUiPanel()
        {
            InitializeComponent();
            LoadImages();
            SetupEventHandlers();
        }

        private void LoadImages()
        {
            try
            {
                normalCloseImage = Image.FromFile(@"Images/Window/close.png");
                selectedCloseImage = Image.FromFile(@"Images/Window/close-selected.png");

                // Set initial image for close button
                crlCloseRightPanel.Image = normalCloseImage;
                crlCloseRightPanel.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            catch (Exception ex)
            {
                // Handle image loading errors
                MessageBox.Show($"Error loading close button images: {ex.Message}");
            }
        }

        private void SetupEventHandlers()
        {
            crlCloseRightPanel.MouseEnter += PictureBox1_MouseEnter;
            crlCloseRightPanel.MouseLeave += PictureBox1_MouseLeave;
        }

        private void PictureBox1_MouseEnter(object sender, EventArgs e)
        {
            crlCloseRightPanel.Image = selectedCloseImage;
        }

        private void PictureBox1_MouseLeave(object sender, EventArgs e)
        {
            crlCloseRightPanel.Image = normalCloseImage;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Hide();
        }
    }
