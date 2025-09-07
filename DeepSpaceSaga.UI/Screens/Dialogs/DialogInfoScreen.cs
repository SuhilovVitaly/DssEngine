using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepSpaceSaga.UI.Screens.Dialogs
{
    public partial class DialogInfoScreen : Form
    {
        public DialogInfoScreen()
        {
            InitializeComponent();
        }

        public DialogInfoScreen(GameActionEventDto gameActionEvent, IGameManager gameManager)
        {
            InitializeComponent();
        }
    }
}
