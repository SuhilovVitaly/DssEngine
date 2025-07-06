namespace DeepSpaceSaga.UI.Screens.TacticalMap.ScreenControls;

public partial class RightUiToolbar : UserControl
{
    private Image normalImage;
    private Image selectedImage;

    public RightUiToolbar()
    {
        InitializeComponent();
        LoadImages();
        SetupEventHandlers();
    }

    private void LoadImages()
    {
        try
        {
            normalImage = Image.FromFile(@"Images/TacticalMap/right-toolbar.png");
            selectedImage = Image.FromFile(@"Images/TacticalMap/right-toolbar-selected.png");
            
            // Set initial image
            crlTargetSystem.Image = normalImage;
            crlTargetSystem.SizeMode = PictureBoxSizeMode.StretchImage;
        }
        catch (Exception ex)
        {
            // Handle image loading errors
            MessageBox.Show($"Error loading images: {ex.Message}");
        }
    }

    private void SetupEventHandlers()
    {
        crlTargetSystem.MouseEnter += CrlTargetSystem_MouseEnter;
        crlTargetSystem.MouseLeave += CrlTargetSystem_MouseLeave;
    }

    private void CrlTargetSystem_MouseEnter(object sender, EventArgs e)
    {
        crlTargetSystem.Image = selectedImage;
    }

    private void CrlTargetSystem_MouseLeave(object sender, EventArgs e)
    {
        crlTargetSystem.Image = normalImage;
    }
}
