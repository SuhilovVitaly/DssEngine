using DeepSpaceSaga.UI.Tools;

namespace DeepSpaceSaga.UI.CommonControls;

public class HoverPictureBox : PictureBox
{
    private Image _normalImage;
    private Image _selectedImage;

    public Image NormalImage
    {
        get => _normalImage;
        set
        {
            _normalImage = value;
            if (Image == null || Image == _selectedImage)
            {
                Image = _normalImage;
            }
        }
    }

    public Image SelectedImage
    {
        get => _selectedImage;
        set => _selectedImage = value;
    }

    public HoverPictureBox()
    {
        SetupEventHandlers();
        SizeMode = PictureBoxSizeMode.StretchImage;
        
        // Set default cursor from CursorManager only in runtime
        if (!DesignModeChecker.IsInDesignMode())
        {
            Cursor = CursorManager.DefaultCursor;
        }
    }

    private void SetupEventHandlers()
    {
        MouseEnter += HoverPictureBox_MouseEnter;
        MouseLeave += HoverPictureBox_MouseLeave;
    }

    private void HoverPictureBox_MouseEnter(object sender, EventArgs e)
    {
        if (_selectedImage != null)
        {
            Image = _selectedImage;
        }
        
        // Skip cursor change in design mode
        if (!DesignModeChecker.IsInDesignMode())
        {
            Cursor = CursorManager.SelectedCursor;
        }
    }

    private void HoverPictureBox_MouseLeave(object sender, EventArgs e)
    {
        if (_normalImage != null)
        {
            Image = _normalImage;
        }
        
        // Skip cursor change in design mode
        if (!DesignModeChecker.IsInDesignMode())
        {
            Cursor = CursorManager.DefaultCursor;
        }
    }
} 