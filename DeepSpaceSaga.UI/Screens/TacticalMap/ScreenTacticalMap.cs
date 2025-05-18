namespace DeepSpaceSaga.UI.Screens.TacticalMap;

public partial class ScreenTacticalMap : Form
{
    public ScreenTacticalMap()
    {
        InitializeComponent();

        FormBorderStyle = FormBorderStyle.None;
        //BackColor = Color.Black;
        ShowInTaskbar = false;

        // Set size to primary screen dimensions
        Size = Screen.PrimaryScreen.Bounds.Size;
        Location = new Point(0, 0);
    }
}
