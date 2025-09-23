
using DeepSpaceSaga.UI.Screens.CombatStage;

namespace DeepSpaceSaga.UI.Services.Screens;

public class ScreensService : IScreensService
{
    public event Action<DialogExit, DialogDto>? OnDialogChoice;

    private readonly ScreenBackground _screenBackground;
    private IScreenTacticalMap? _tacticalMap;
    private Form? _activeDialogScreen = null;
    private Form? _previousDialogScreen = null;

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

    public async Task ShowDialogScreen(GameActionEventDto gameActionEvent)
    {
        switch (gameActionEvent.Dialog?.UiScreenType)
        {
            case GameEventUiScreenType.InfoWithChoices:
                await ShowDialogBasicInfo(gameActionEvent);
                break;
            case GameEventUiScreenType.DialogOnePerson:
                await ShowDialogPersons(gameActionEvent);
                break;
            case GameEventUiScreenType.DialogTwoPerson:
                await ShowDialogPersons(gameActionEvent);
                break;
            case GameEventUiScreenType.SceneCombatStage:
                await ShowScreenCombatStage(gameActionEvent);
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
                //_activeDialogScreen = screen;
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
        var screen = new DialogBasicInfoScreen();

        screen.ShowDialogEvent(gameActionEvent);
        screen.OnDialogChoice += Screen_OnDialogChoice;

        await OpenDialogScreen(screen);
    }

    private async Task ShowScreenCombatStage(GameActionEventDto gameActionEvent)
    {
        var screen = Program.ServiceProvider.GetService<ScreenCombatStage>();

        screen.ShowDialogEvent(gameActionEvent);

        await OpenDialogScreen(screen);
    }

    private void Screen_OnDialogChoice(DialogExit dialogExit, DialogDto currentDialog)
    {
        OnDialogChoice?.Invoke(dialogExit, currentDialog);
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

    public void CloseActiveDialogScreen()
    {
        if (_activeDialogScreen != null)
        {
            _activeDialogScreen.Close();
            _activeDialogScreen = null;
        }
    }

    private void ClosePreviousDialogScreen()
    {
        if (_previousDialogScreen != null)
        {
            _previousDialogScreen.Close();
            _previousDialogScreen = null;
        }
    }

    private async Task OpenDialogScreen(Form? screen)
    {
        _previousDialogScreen = _activeDialogScreen;
        _activeDialogScreen = screen;

        // Close previous window with small delay
        if (_previousDialogScreen != null)
        {
            _ = Task.Run(async () =>
            {
                await Task.Delay(100); // Small delay for smoothness
                _screenBackground.Invoke(new Action(() => ClosePreviousDialogScreen()));
            });
        }

        // Show new window
        await _screenBackground.OpenWindow(screen);
        screen.Focus();
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