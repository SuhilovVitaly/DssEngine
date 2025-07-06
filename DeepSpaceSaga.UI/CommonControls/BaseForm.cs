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
        Cursor = CursorManager.DefaultCursor;
    }

    private void ApplyCustomCursors()
    {
        CursorManager.SetDefaultCursorForControl(this);
    }
} 