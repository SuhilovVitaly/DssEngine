using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSpaceSaga.UI.Controller.Screens
{
    public class ScreenTacticalMapController: IScreenTacticalMapController
    {
        private readonly IGameManager _gameManager;

        public ScreenTacticalMapController(IGameManager gameManager)
        {
            _gameManager = gameManager ?? throw new ArgumentNullException(nameof(gameManager));
            //_gameManager.Screens.
        }

        public void CloseRightPanel()
        {
            
        }
    }
}
