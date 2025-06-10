namespace DeepSpaceSaga.Server.Generation.CelestialObjects
{
    public static class SpacecraftGenerator
    {
        public static ICelestialObject BuildPlayerSpacecraft(int id, float size, double direction, double x, double y, double speed, string name)
        {
            return new BaseSpaceship
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
        }
    }
}
