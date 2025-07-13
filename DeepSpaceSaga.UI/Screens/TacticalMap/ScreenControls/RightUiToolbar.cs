using DeepSpaceSaga.Common.Abstractions.Dto.Ui;
using DeepSpaceSaga.UI.Controller.Services;
using DeepSpaceSaga.UI.Tools;
using System.Drawing;
using System.Windows.Forms;

namespace DeepSpaceSaga.UI.Screens.TacticalMap.ScreenControls;

public partial class RightUiToolbar : UserControl
{
    public event Action<RightPanelContentType>? OnShowRightPanel;

    public RightUiToolbar()
    {
        InitializeComponent();
        LoadImages();
    }

    private void LoadImages()
    {
        // Skip loading images in design mode
        if (DesignModeChecker.IsInDesignMode()) return;

        try
        {
            var normalImagePath = Path.Combine(Application.StartupPath, "Images", "TacticalMap", "right-toolbar.png");
            var selectedImagePath = Path.Combine(Application.StartupPath, "Images", "TacticalMap", "right-toolbar-selected.png");

            crlTargetSystem.NormalImage = Image.FromFile(normalImagePath);
            crlTargetSystem.SelectedImage = Image.FromFile(selectedImagePath);
        }
        catch (Exception ex)
        {
            // Handle image loading errors
            MessageBox.Show($"Error loading images: {ex.Message}");
        }
    }

    private void crlTargetSystem_Click(object sender, EventArgs e)
    {
        OnShowRightPanel?.Invoke(RightPanelContentType.SelectedObjectInformation);
    }

    private void hoverPictureBox1_Click(object sender, EventArgs e)
    {
        OnShowRightPanel?.Invoke(RightPanelContentType.SelectedWeaponInformation);
    }
}
