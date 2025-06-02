namespace DeepSpaceSaga.Common.Abstractions.Events;

public interface IGameActionEvent
{
    GameActionEventTypes EventType { get; }
    ICommand? TriggerCommand { get; set; }
    long? CelestialObjectId { get; set; }
    long? TargetObjectId { get; set; }
    long? ModuleId { get; set; }
    long Id { get; set; }
    long CalculationTurnId { get; set; }
    IDialog? Dialog { get; set; }
    List<IDialog> ConnectedDialogs { get; set; }
}
