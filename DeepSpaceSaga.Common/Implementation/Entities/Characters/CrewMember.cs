namespace DeepSpaceSaga.Common.Implementation.Entities.Characters
{
    [DebuggerDisplay("{FirstName} {LastName} Age = {Age}")]
    public class CrewMember : ICharacter
    {
        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Rank { get; set; }

        public string Portrait { get; set; }

        public Gender Gender { get; set; }

        public int Age { get; set; }

        public int Id { get; set; }

        public int Salary { get; set; }

        public IPersonPortrait UiInfo { get; set; }

        public Dictionary<CharacterSkillType, ICharacterSkill> Skills { get; set; }

        public PersonStats Stats { get; set; } = new PersonStats();
    }
}
