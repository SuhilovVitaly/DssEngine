using DeepSpaceSaga.Common.Implementation.Entities.Dialogs;

namespace DeepSpaceSaga.Common.Implementation.Entities.Commands;

public class DialogExitCommand : Command, ICommand
{
    public required string DialogKey { get; set; }
    public required string DialogExitKey { get; set; }
    public List<DialogCommand> DialogCommands { get; set; } = new List<DialogCommand>();
}
