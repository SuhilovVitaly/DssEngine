namespace DeepSpaceSaga.UI.Screens.TacticalMap.ScreenControls;

public partial class RightUiToolbar : UserControl
{
    public RightUiToolbar()
    {
        InitializeComponent();
        LoadImages();
    }

    private void LoadImages()
    {
        try
        {
            crlTargetSystem.NormalImage = Image.FromFile(@"Images/TacticalMap/right-toolbar.png");
            crlTargetSystem.SelectedImage = Image.FromFile(@"Images/TacticalMap/right-toolbar-selected.png");
        }
        catch (Exception ex)
        {
            // Handle image loading errors
            MessageBox.Show($"Error loading images: {ex.Message}");
        }
    }
}
