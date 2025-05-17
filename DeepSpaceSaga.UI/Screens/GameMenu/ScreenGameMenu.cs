using System;
using System.Collections.Generic;
namespace DeepSpaceSaga.UI.Screens.GameMenu;

public partial class ScreenGameMenu : Form
{
    public ScreenGameMenu()
    {
        InitializeComponent();
    }

    private void crlExitGame_Click(object sender, EventArgs e)
    {
        Application.Exit();
    }
}
