namespace DeepSpaceSaga.Common.Abstractions.Dto;

public class GameActionEventDto
{
    public long? CelestialObjectId { get; set; }
    public long? TargetObjectId { get; set; }
    public long? ModuleId { get; set; }
    public long Id { get; set; }
    public long CalculationTurnId { get; set; }
    public GameActionEventTypes EventType { get; set; } = GameActionEventTypes.None;
    public DialogDto? Dialog { get; set; }
    public List<DialogDto> ConnectedDialogs { get; set; }
}
