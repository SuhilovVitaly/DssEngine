using DeepSpaceSaga.UI.Screens.GameMenu;

namespace DeepSpaceSaga.UI.Services.Screens;

internal class ScreensService : IScreensService
{
    private ScreenBackground _screenBackground;
    public ScreensService(ScreenBackground screenBackground)
    {
        _screenBackground = screenBackground;
    }

    public void ShowGameMenuScreen()
    {
        var screen = Program.ServiceProvider.GetService<ScreenGameMenu>();

        _screenBackground.OpenWindow(screen);
    }
}
