namespace DeepSpaceSaga.Common.Abstractions.Entities;

public class Command
{
    public Guid CommandId { get; set; } = Guid.NewGuid();
}