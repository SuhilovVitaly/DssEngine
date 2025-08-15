namespace DeepSpaceSaga.Common.Implementation.Entities.Characters;

public class PersonPortrait : IPersonPortrait
{
    public int Id { get; init; }

    public string File { get; init; }

    public Gender Gender { get; init; }

    public PersonPortrait(int id, string file, Gender gender)
    {
        Id = id;
        File = file;
        Gender = gender;
    }

}
