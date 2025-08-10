namespace DeepSpaceSaga.Common.Abstractions.Dto.Ui;

public class CommandDto
{
    public Guid CommandId { get; set; }
    public CommandCategory Category { get; set; }
    public CommandTypes Type { get; set; }
    public CommandStatus Status { get; set; }
    public ICommand? TriggerCommand { get; set; }
    public long? CelestialObjectId { get; set; }
    public int MemberId { get; set; }
    public long? TargetCelestialObjectId { get; set; }
    public int ModuleId { get; set; }
    public bool IsOneTimeCommand { get; set; }
    public bool IsUnique { get; set; }
    public bool IsPauseProcessed { get; set; }
}