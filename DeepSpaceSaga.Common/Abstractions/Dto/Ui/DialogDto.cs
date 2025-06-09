namespace DeepSpaceSaga.Common.Abstractions.Dto.Ui;

public class DialogDto
{
    public string Title { get; set; }
    public string Message { get; set; }
    public string Key { get; set; }
    public bool ChainPart { get; set; }
    public DialogTypes Type { get; set; }
    public DialogTrigger Trigger { get; set; }
    public DialogUiScreenType UiScreenType { get; set; }
    public string TriggerValue { get; set; }
    public ICharacter Reporter { get; set; }
    public List<DialogExit> Exits { get; set; }
}
