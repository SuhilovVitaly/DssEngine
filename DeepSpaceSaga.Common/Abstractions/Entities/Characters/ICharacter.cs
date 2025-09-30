namespace DeepSpaceSaga.Common.Abstractions.Entities.Characters;

public interface ICharacter
{
    string LastName { get; }
    string FirstName { get; }
    string Rank { get; }
    string Portrait { get; }
    Gender Gender { get; }
    int Age { get; set; }
    int Id { get; }
    int Salary { get; }

    IPersonPortrait UiInfo { get; set; }

    Dictionary<CharacterSkillType, ICharacterSkill> Skills { get; }

    PersonStats Stats { get; set;}
}
