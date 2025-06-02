namespace DeepSpaceSaga.Common.Abstractions.Entities.Characters;

public interface IPersonPortrait
{
    int Id { get; init; }

    string File { get; init; }

    Gender Gender { get; init; }
}
