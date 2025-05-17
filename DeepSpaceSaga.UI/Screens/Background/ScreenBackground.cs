namespace DeepSpaceSaga.UI.Screens.Background;

public partial class ScreenBackground : Form
{
    public ScreenBackground()
    {
        InitializeComponent();

        FormBorderStyle = FormBorderStyle.None;
        BackColor = Color.Black;
        ShowInTaskbar = false;

        // Set size to primary screen dimensions
        Size = Screen.PrimaryScreen.Bounds.Size;
        Location = new Point(0, 0);
    }

    public void OpenWindow(Form childForm)
    {
        try
        {
            CrossThreadExtensions.PerformSafely(this, OpenModalWindow, childForm);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    private void OpenModalWindow(Form childForm)
    {
        childForm.ShowInTaskbar = false;
        childForm.Visible = true;
        childForm.ShowDialog(this);
    }
}
