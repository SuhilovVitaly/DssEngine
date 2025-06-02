namespace DeepSpaceSaga.Common.Abstractions.Entities.Characters.Skills;

public interface ICharacterSkill
{
    CharacterSkillType SkillType { get; }
    int Level { get; }
    int MaxLevel { get; }
    int TrainingEfficiency { get; }
}