using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepSpaceSaga.UI.Screens.TacticalMap.ScreenControls.RightUiPanelControls
{
    public partial class GameObjectGeneralInfo : UserControl
    {
        private IGameManager _gameManager;
        private bool _isInitialized;

        public GameObjectGeneralInfo()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitializeGameManager();
        }

        private void InitializeGameManager()
        {
            if (_isInitialized || DesignModeChecker.IsInDesignMode()) return;

            try
            {
                _gameManager = Program.ServiceProvider?.GetService<IGameManager>();
                if (_gameManager != null)
                {
                    //_gameManager.OnUpdateGameData += UpdateGameData;
                    _isInitialized = true;
                }
            }
            catch
            {
                // Ignore exceptions in design mode
                if (!DesignModeChecker.IsInDesignMode()) throw;
            }
        }
    }
}
