using DeepSpaceSaga.UI.Screens.TacticalMap;

namespace DeepSpaceSaga.UI.Services.Screens;

public class ScreensService(ScreenBackground screenBackground) : IScreensService
{
    private readonly ScreenBackground _screenBackground = screenBackground;
    private Form _screen;

    public void ShowGameMenuScreen()
    {
        _screen = Program.ServiceProvider.GetService<ScreenGameMenu>();

        _screenBackground.OpenWindow(_screen);
    }

    public void ShowTacticalMapScreen()
    {
        _screen.Visible = false;
        _screen = Program.ServiceProvider.GetService<ScreenTacticalMap>();

        _screenBackground.OpenWindow(_screen);
    }
}
