using DeepSpaceSaga.UI.Tools;

namespace DeepSpaceSaga.UI.CommonControls;

public class BaseForm : Form
{
    public BaseForm()
    {
        InitializeCustomCursor();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        ApplyCustomCursors();
    }

    private void InitializeCustomCursor()
    {
        // Skip setting cursor in design mode
        if (DesignModeChecker.IsInDesignMode()) return;
        
        Cursor = CursorManager.DefaultCursor;
    }

    private void ApplyCustomCursors()
    {
        // Skip applying cursors in design mode
        if (DesignModeChecker.IsInDesignMode()) return;
        
        CursorManager.SetDefaultCursorForControl(this);
    }
} 