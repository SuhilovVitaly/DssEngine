using DeepSpaceSaga.Server.Generation.Modules;

namespace DeepSpaceSaga.Server.Generation.CelestialObjects
{
    public static class SpacecraftGenerator
    {
        public static ICelestialObject BuildPlayerSpacecraft(int id, float size, double direction, double x, double y, double speed, string name)
        {
            var randomizer = new GenerationTool();

            var spacecraft = new BaseSpaceship
            {
                Id = id,
                Agility = 0,
                Direction = direction,
                MaxSpeed = 20,
                Name = name,
                OwnerId = id,
                Size = size,
                Speed = speed,
                Type  = CelestialObjectType.SpaceshipPlayer,
                X = x,
                Y = y,
                IsPreScanned = true,
            };

            spacecraft.ModulesS.Add(MiningModulesGenerator.CreateMiningLaser(randomizer, spacecraft.Id, "MLC8002"));

            return spacecraft;
        }
    }
}
