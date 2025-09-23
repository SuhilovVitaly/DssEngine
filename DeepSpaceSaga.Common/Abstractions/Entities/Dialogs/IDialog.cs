using DeepSpaceSaga.Common.Implementation.Entities.Characters;

namespace DeepSpaceSaga.Common.Abstractions.Entities.Dialogs;

public interface IDialog
{
    string Title { get; }
    string Message { get; }
    string Key { get; set; }
    bool ChainPart { get; set; }
    DialogTypes Type { get; }
    DialogTrigger Trigger { get; }
    GameEventUiScreenType UiScreenType { get; set; }
    string TriggerValue { get; }
    string Image { get; set; }
    CrewMember? Reporter { get; }
    List<DialogExit> Exits { get; set; }
}