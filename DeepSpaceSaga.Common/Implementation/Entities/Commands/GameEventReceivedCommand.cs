namespace DeepSpaceSaga.Common.Implementation.Entities.Commands;

public class GameEventReceivedCommand : Command, ICommand
{
    public required string DialogKey { get; set; }
}
