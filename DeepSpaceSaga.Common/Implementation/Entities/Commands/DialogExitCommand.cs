namespace DeepSpaceSaga.Common.Implementation.Entities.Commands;

public class DialogExitCommand : Command, ICommand
{
    public required string DialogKey { get; set; }
    public required string DialogExitKey { get; set; }
}
