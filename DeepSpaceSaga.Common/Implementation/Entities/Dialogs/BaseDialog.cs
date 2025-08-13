namespace DeepSpaceSaga.Common.Implementation.Entities.Dialogs;

[ExcludeFromCodeCoverage]
[DebuggerDisplay("{Key} {Title}")]
public class BaseDialog : IDialog
{
    public string Key { get; set; } = "";

    public string Title { get; set; } = "";

    public string Message { get; set; } = "";

    public bool ChainPart { get; set; } = false;

    public DialogTypes Type { get; set; }

    public DialogTrigger Trigger { get; set; }

    public DialogUiScreenType UiScreenType { get; set; }

    public string TriggerValue { get; set; }

    [JsonIgnore] // Do not deserialize interface property from JSON
    public ICharacter Reporter { get; set; }

    public List<DialogExit> Exits { get; set; } = new List<DialogExit>();
}