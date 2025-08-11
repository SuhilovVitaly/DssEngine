namespace DeepSpaceSaga.Common.Abstractions.UI.Screens;

public interface IScreenTacticalMap
{
    void CloseRightPanel();

    void StartDialog(GameActionEventDto gameActionEvent);
}
