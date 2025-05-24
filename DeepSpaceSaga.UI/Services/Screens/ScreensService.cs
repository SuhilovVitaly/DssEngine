using DeepSpaceSaga.UI.Screens.TacticalMap;

namespace DeepSpaceSaga.UI.Services.Screens;

public class ScreensService : IScreensService
{
    private readonly ScreenBackground _screenBackground;

    public ScreensService(ScreenBackground screenBackground)
    {

        _screenBackground = screenBackground;

        _screenBackground.FirstShown += (sender, e) =>
        {
            ShowGameMenuScreen();
        };
    }

    public void ShowGameMenuScreen()
    {
        var screen = Program.ServiceProvider.GetService<ScreenMainMenu>();
        _screenBackground.ShowChildForm(screen);
    }

    public void ShowTacticalMapScreen()
    {
        var screen = Program.ServiceProvider.GetService<ScreenTacticalMap>();
        _screenBackground.ShowChildForm(screen);
    }
}
