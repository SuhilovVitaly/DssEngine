using DeepSpaceSaga.UI.Screens.Dialogs;
using System.Windows.Forms;

namespace DeepSpaceSaga.UI.Services.Screens;

public class ScreensService : IScreensService
{
    private readonly ScreenBackground _screenBackground;
    private IScreenTacticalMap? _tacticalMap;
    private Form? _activeDialogScreen = null;
    public IScreenTacticalMap TacticalMap
    {
        get
        {
            if (_tacticalMap == null)
            {
                _tacticalMap = Program.ServiceProvider.GetService<IScreenTacticalMap>();
            }
            return _tacticalMap!;
        }
        set => _tacticalMap = value;
    }

    public ScreensService(ScreenBackground screenBackground)
    {
        _screenBackground = screenBackground;

        _screenBackground.FirstShown += (sender, e) =>
        {
            Console.WriteLine("[ScreensService] Background screen shown, showing main menu");
            ShowGameMenuScreen();
        };
    }

    public void ShowGameMenuScreen()
    {
        try
        {
            Console.WriteLine("[ScreensService] Showing main menu screen");
            var screen = Program.ServiceProvider.GetService<ScreenMainMenu>();
            if (screen != null)
            {
                _screenBackground.ShowChildForm(screen);
                Console.WriteLine("[ScreensService] Main menu screen displayed successfully");
            }
            else
            {
                Console.WriteLine("[ScreensService] ERROR: Failed to get ScreenMainMenu from service provider");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ScreensService] ERROR showing main menu: {ex.Message}");
        }
    }

    public void ShowDialogNextScreen(GameActionEventDto gameActionEvent)
    {

    }

    public async Task ShowDialogScreen(GameActionEventDto gameActionEvent)
    {
        if(_activeDialogScreen != null)
        {
            _activeDialogScreen.Hide();

            _activeDialogScreen.Close();
            _activeDialogScreen = null; 
        }

        switch (gameActionEvent.Dialog?.UiScreenType)
        {
            case DialogUiScreenType.Info:
                await ShowDialogBasicInfo(gameActionEvent);
                break;
            case DialogUiScreenType.OnePerson:
                await ShowDialogPersons(gameActionEvent);
                break;
            case DialogUiScreenType.TwoPerson:
                await ShowDialogPersons(gameActionEvent);
                break;
        }
    }

    private async Task ShowDialogPersons(GameActionEventDto gameActionEvent)
    {
        try
        {
            Console.WriteLine("[ScreensService] Showing dialog screen");
            var screen = Program.ServiceProvider.GetService<DialogBasicScreen>();
            screen.ShowDialogEvent(gameActionEvent);

            if (screen != null)
            {
                _activeDialogScreen = screen;
                await _screenBackground.OpenWindow(screen);
                Console.WriteLine("[ScreensService] Dialog screen displayed successfully");
            }
            else
            {
                Console.WriteLine("[ScreensService] ERROR: Failed to get DialogBasicInfoScreen from service provider");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ScreensService] ERROR showing dialog: {ex.Message}");
        }
    }

    private async Task ShowDialogBasicInfo(GameActionEventDto gameActionEvent)
    {
        try
        {
            Console.WriteLine("[ScreensService] Showing dialog screen");
            
            // Create a new instance each time to avoid modal dialog conflicts
            var screen = new DialogBasicInfoScreen(
                Program.ServiceProvider.GetService<IGameManager>(),
                Program.ServiceProvider.GetService<IScreensService>()
            );
            
            screen.ShowDialogEvent(gameActionEvent);
            screen.OnNextDialog += Screen_OnNextDialog;

            if (screen != null)
            {
                _activeDialogScreen = screen;
                await _screenBackground.OpenWindow(screen);
                Console.WriteLine("[ScreensService] Dialog screen displayed successfully");
            }
            else
            {
                Console.WriteLine("[ScreensService] ERROR: Failed to create DialogBasicInfoScreen");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ScreensService] ERROR showing dialog: {ex.Message}");
        }
    }

    private void Screen_OnNextDialog(DialogExit obj)
    {
        _activeDialogScreen.Hide();
        _activeDialogScreen.Close();
        _activeDialogScreen = null;
    }

    public void ShowTacticalMapScreen()
    {
        try
        {
            Console.WriteLine("[ScreensService] Showing tactical map screen");
            var screen = Program.ServiceProvider.GetService<IScreenTacticalMap>();
            if (screen != null)
            {
                _screenBackground.ShowChildForm(screen as Form);
                Console.WriteLine("[ScreensService] Tactical map screen displayed successfully");
            }
            else
            {
                Console.WriteLine("[ScreensService] ERROR: Failed to get ScreenTacticalMap from service provider");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ScreensService] ERROR showing tactical map: {ex.Message}");
        }
    }

    public async Task ShowGameMenuModal()
    {
        try
        {
            Console.WriteLine("[ScreensService] Showing game menu modal");
            var screen = Program.ServiceProvider.GetService<ScreenGameMenu>();
            if (screen != null)
            {
                await _screenBackground.OpenWindow(screen);
                Console.WriteLine("[ScreensService] Game menu modal displayed successfully");
            }
            else
            {
                Console.WriteLine("[ScreensService] ERROR: Failed to get ScreenGameMenu from service provider");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ScreensService] ERROR showing game menu modal: {ex.Message}");
        }
    }

    public void CloseTacticalMapScreen()
    {
        try
        {
            Console.WriteLine("[ScreensService] Closing tactical map screen");
            var existingScreen = _screenBackground.Controls.OfType<ScreenTacticalMap>().FirstOrDefault();
            if (existingScreen != null)
            {
                if (_screenBackground.InvokeRequired)
                {
                    _screenBackground.Invoke(() => 
                    {
                        existingScreen.Close();
                        _screenBackground.Controls.Remove(existingScreen);
                    });
                }
                else
                {
                    existingScreen.Close();
                    _screenBackground.Controls.Remove(existingScreen);
                }
                Console.WriteLine("[ScreensService] Tactical map screen closed and removed successfully");
            }
            else
            {
                Console.WriteLine("[ScreensService] No tactical map screen found to close");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ScreensService] ERROR closing tactical map screen: {ex.Message}");
        }
    }
}
