namespace DeepSpaceSaga.UI.Screens.GameMenu;

public partial class ScreenMainMenu : Form
{
    private GameManager _gameManager;
    public ScreenMainMenu(GameManager gameManager)
    {
        InitializeComponent();

        _gameManager = gameManager ?? throw new ArgumentNullException(nameof(GameManager));
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

    private void Event_ApplicationExit(object sender, EventArgs e)
    {
        Application.Exit();
    }

    private void Event_StartNewGameSession(object sender, EventArgs e)
    {
        _gameManager.SessionStart();
    }

    private void Event_LoadGame(object sender, EventArgs e)
    {
        
    }
}
