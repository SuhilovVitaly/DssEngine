namespace DeepSpaceSaga.Common.Abstractions.Entities;

public class Command: ICommand
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public CommandCategory Category { get; set; }
    public CommandTypes Type { get; set; }
    public CommandStatus Status { get; set; } = CommandStatus.None;
    public long CelestialObjectId { get; set; }
    public int MemberId { get; set; }
    public long TargetCelestialObjectId { get; set; }
    public int ModuleId { get; set; }
    public bool IsOneTimeCommand { get; set; }
    public bool IsUnique { get; set; } = true;
    public ICommand? TriggerCommand { get; set; }
    public bool IsPauseProcessed { get; set; }
}