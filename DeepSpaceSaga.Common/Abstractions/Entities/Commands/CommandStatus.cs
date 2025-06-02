namespace DeepSpaceSaga.Common.Abstractions.Entities.Commands;

public enum CommandStatus
{
    None,
    PreProcess,
    Process,
    PostProcess,
    Finalizing
}
