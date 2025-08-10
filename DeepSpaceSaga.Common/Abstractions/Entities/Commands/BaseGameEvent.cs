namespace DeepSpaceSaga.Common.Abstractions.Entities.Commands;

public class BaseGameEvent : IGameActionEvent
{
    public ICommand? TriggerCommand { get; set; }
    public long? CelestialObjectId { get; set; }
    public long? TargetObjectId { get; set; }
    public long? ModuleId { get; set; }
    public string Key { get; set; }
    public long CalculationTurnId { get; set; }
    public GameActionEventTypes EventType { get; set; } = GameActionEventTypes.None;
    public IDialog? Dialog { get; set; }
    public List<IDialog> ConnectedDialogs { get; set; }
}
