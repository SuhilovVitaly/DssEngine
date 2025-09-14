using System.Windows.Forms;

namespace DeepSpaceSaga.UI.Screens.Dialogs.Controls;

    /// <summary>
    /// Custom panel that supports transparency
    /// </summary>
    public class TransparentPanel : Panel
    {
        public TransparentPanel()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
        }

        /// <summary>
        /// Override CreateParams to enable transparency
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x20; // WS_EX_TRANSPARENT
                return cp;
            }
        }
    }


