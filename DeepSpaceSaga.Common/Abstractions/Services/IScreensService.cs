using DeepSpaceSaga.Common.Abstractions.UI.Screens;

namespace DeepSpaceSaga.Common.Abstractions.Services;

public interface IScreensService
{
    IScreenTacticalMap TacticalMap { get; set; }

    //IScreenTacticalMapController TacticalMapController { get; set; }

    void ShowGameMenuScreen();

    void ShowTacticalMapScreen();

    void ShowGameMenuModal();

    void CloseTacticalMapScreen();
}
