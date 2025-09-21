using DeepSpaceSaga.Common.Abstractions.UI.Screens;

namespace DeepSpaceSaga.Common.Abstractions.Services;

public interface IScreensService
{
    IScreenTacticalMap TacticalMap { get; set; }

    void ShowGameMenuScreen();

    void ShowTacticalMapScreen();

    Task ShowGameMenuModal();

    void CloseTacticalMapScreen();

    Task ShowDialogScreen(GameActionEventDto gameActionEvent);
}
