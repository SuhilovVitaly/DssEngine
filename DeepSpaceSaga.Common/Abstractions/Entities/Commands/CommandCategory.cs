namespace DeepSpaceSaga.Common.Abstractions.Entities.Commands;

public enum CommandCategory
{
    None,
    Scan,
    Mining,
    Navigation,
    ContentGeneration,
    ModuleActionFinished,
    CargoOperations,
    EventAcknowledgement,
    TrackActivity,
    CrewInteraction
}
