using DeepSpaceSaga.UI.Screens.GameMenu;

namespace DeepSpaceSaga.UI.Services.Screens;

internal class ScreensService(ScreenBackground screenBackground) : IScreensService
{
    private readonly ScreenBackground _screenBackground = screenBackground;

    public void ShowGameMenuScreen()
    {
        var screen = Program.ServiceProvider.GetService<ScreenGameMenu>();

        _screenBackground.OpenWindow(screen);
    }
}
