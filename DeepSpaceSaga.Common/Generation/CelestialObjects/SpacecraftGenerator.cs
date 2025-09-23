using DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects.Spacecrafts;
using DeepSpaceSaga.Common.Generation.CelestialObjects.Modules;
using DeepSpaceSaga.Common.Implementation.Entities.Characters;

namespace DeepSpaceSaga.Common.Generation.CelestialObjects
{
    public static class SpacecraftGenerator
    {
        public static ICelestialObject BuildPlayerSpacecraft(int id, float size, double direction, double x, double y, double speed, string name)
        {
            var randomizer = new GenerationTool();

            ISpacecraft spacecraft = new BaseSpaceship
            {
                Id = id,
                Agility = 0,
                Direction = direction,
                MaxSpeed = 20,
                Name = name,
                OwnerId = id,
                Size = size,
                Speed = speed,
                Type = CelestialObjectType.SpaceshipPlayer,
                X = x,
                Y = y,
                IsPreScanned = true,
            };

            ICharacter mainCharacter = new CrewMember
            {
                Id = 1,
                FirstName = "Vitold",
                LastName = "Deen",
                Age = 31,
                Gender = Gender.Male,
                Rank = "Prisoner",
                Portrait = "Vitold_Suni.png",
                Salary = 0,
                Skills = new Dictionary<CharacterSkillType, ICharacterSkill>()
            };

            spacecraft.AddCrewMember(mainCharacter);

            spacecraft.ModulesS.Add(MiningModulesGenerator.CreateMiningLaser(randomizer, spacecraft.Id, "MLC8002"));

            return spacecraft;
        }
    }
}
