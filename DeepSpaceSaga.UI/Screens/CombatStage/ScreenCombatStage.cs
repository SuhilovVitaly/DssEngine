using DeepSpaceSaga.UI.Controller.Services;

namespace DeepSpaceSaga.UI.Screens.CombatStage
{
    public partial class ScreenCombatStage : Form
    {
        private GameActionEventDto? _gameActionEvent;
        private IGameManager? _gameManager;
        private DialogDto? _currentDialog;

        public ScreenCombatStage()
        {
            InitializeComponent();
        }

        public ScreenCombatStage(IGameManager gameManager)
        {
            InitializeComponent();

            _gameManager = gameManager;

            FormBorderStyle = FormBorderStyle.None;
            Size = new Size(1375, 875);
            ShowInTaskbar = false;
        }

        public void ShowDialogEvent(GameActionEventDto gameActionEvent)
        {
            _gameActionEvent = gameActionEvent;
            _currentDialog = gameActionEvent.Dialog;

            if (_currentDialog != null)
            {
                ShowGameScreen(gameActionEvent);
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

        private void crlMainMenu_Click(object sender, EventArgs e)
        {
            _gameManager?.Screens.CloseActiveDialogScreen();
        }

        private void ShowGameScreen(GameActionEventDto gameActionEvent)
        {
            var mainCharacter = _gameManager.GetMainCharacter();

            
            crlName.Text = mainCharacter.FirstName + " " + mainCharacter.LastName;
            crlRank.Text = _gameManager.Localization.GetText(mainCharacter.Rank);
            crlPortrait.Image = ImageLoader.LoadCharacterImage(mainCharacter.Portrait);


            var enemyCharacter = _gameManager.GetCharacter(Int32.Parse(gameActionEvent.Dialog.Tag));

            label1.Text = enemyCharacter.FirstName + " " + enemyCharacter.LastName;
            label2.Text = _gameManager.Localization.GetText(enemyCharacter.Rank);
            pictureBox1.Image = ImageLoader.LoadCharacterImage(enemyCharacter.Portrait);

        }
    }
}
