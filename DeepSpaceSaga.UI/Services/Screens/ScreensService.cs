using DeepSpaceSaga.UI.Screens.TacticalMap;
using DeepSpaceSaga.UI.Screens.GameMenu;

namespace DeepSpaceSaga.UI.Services.Screens;

public class ScreensService : IScreensService
{
    private readonly ScreenBackground _screenBackground;

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

    public void ShowTacticalMapScreen()
    {
        try
        {
            Console.WriteLine("[ScreensService] Showing tactical map screen");
            var screen = Program.ServiceProvider.GetService<ScreenTacticalMap>();
            if (screen != null)
            {
                _screenBackground.ShowChildForm(screen);
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
}
