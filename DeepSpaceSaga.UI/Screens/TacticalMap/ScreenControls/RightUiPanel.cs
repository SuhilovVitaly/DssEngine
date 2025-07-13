using DeepSpaceSaga.UI.Tools;
using DeepSpaceSaga.Common.Abstractions.UI.Screens;
using DeepSpaceSaga.UI.Screens.TacticalMap.ScreenControls.RightUiPanelControls;

namespace DeepSpaceSaga.UI.Screens.TacticalMap.ScreenControls;

public partial class RightUiPanel : UserControl
{
    private GameObjectGeneralInfo? _generalInfoControl;
    private GameObjectWeaponInfo? _weaponInfoControl;
    private UserControl? _currentControl;

    public RightUiPanel()
    {
        InitializeComponent();
        LoadImages();
        Cursor = CursorManager.DefaultCursor;
        ShowPanel(RightPanelContentType.SelectedObjectInformation);
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
        HideCurrentControl();
    }

    private void HideCurrentControl()
    {
        if (_currentControl != null)
        {
            _currentControl.Visible = false;
            Controls.Remove(_currentControl);
            _currentControl = null;
        }
    }

    public void ShowPanel(RightPanelContentType contentType)
    {
        UserControl? controlToShow = null;

        switch (contentType)
        {
            case RightPanelContentType.SelectedObjectInformation:
                if (_generalInfoControl == null)
                {
                    _generalInfoControl = new GameObjectGeneralInfo();
                    _generalInfoControl.Dock = DockStyle.Fill;
                }
                controlToShow = _generalInfoControl;
                break;

            case RightPanelContentType.SelectedWeaponInformation:
                if (_weaponInfoControl == null)
                {
                    _weaponInfoControl = new GameObjectWeaponInfo();
                    _weaponInfoControl.Dock = DockStyle.Fill;
                }
                controlToShow = _weaponInfoControl;
                break;
        }

        if (controlToShow != null && controlToShow != _currentControl)
        {
            // Prevent flickering by suspending layout
            SuspendLayout();

            try
            {
                // Hide current control
                if (_currentControl != null)
                {
                    _currentControl.Visible = false;
                    Controls.Remove(_currentControl);
                }

                // Show new control
                Controls.Add(controlToShow);
                controlToShow.BringToFront();
                controlToShow.Visible = true;

                _currentControl = controlToShow;
            }
            finally
            {
                ResumeLayout(true);
            }
        }

        Show();
    }
}
