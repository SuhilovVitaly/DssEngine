namespace DeepSpaceSaga.Common.Abstractions.Services;

public interface IScreensService
{
    event Action<DialogExit, DialogDto>? OnDialogChoice;

    IScreenTacticalMap TacticalMap { get; set; }

    void ShowGameMenuScreen();

    void ShowTacticalMapScreen();

    Task ShowGameMenuModal();

    void CloseTacticalMapScreen();

    Task ShowDialogScreen(GameActionEventDto gameActionEvent);
}
