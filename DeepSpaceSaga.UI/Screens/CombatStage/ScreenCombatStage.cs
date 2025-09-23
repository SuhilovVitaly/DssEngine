using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                var x = "";
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
    }
}
