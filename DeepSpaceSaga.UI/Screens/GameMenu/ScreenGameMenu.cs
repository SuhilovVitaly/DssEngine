namespace DeepSpaceSaga.UI.Screens.MainMenu;

public partial class ScreenGameMenu : Form
{
    private GameManager _gameManager;

    public ScreenGameMenu(GameManager gameManager)
    {
        InitializeComponent();
        _gameManager = gameManager;

        // Enable key handling
        KeyPreview = true;
        KeyDown += OnKeyDown;
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape)
        {
            Close();
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        // Draw border
        using Pen borderPen = new Pen(UiConstants.FormBorderColor, UiConstants.FormBorderSize);
        Rectangle borderRect = new(
            UiConstants.FormBorderSize / 2,
            UiConstants.FormBorderSize / 2,
            Width - UiConstants.FormBorderSize,
            Height - UiConstants.FormBorderSize
        );
        e.Graphics.DrawRectangle(borderPen, borderRect);
    }

    private void button1_Click(object sender, EventArgs e)
    {
        
    }

    private void button2_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void button3_Click(object sender, EventArgs e)
    {
        
    }

    private void Event_SaveGame(object sender, EventArgs e)
    {
        
    }

    private void Event_LoadGame(object sender, EventArgs e)
    {
        
    }
}
