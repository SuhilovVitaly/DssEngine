namespace DeepSpaceSaga.Server.Generation.CelestialObjects;

public class AsteroidGenerator
{
    private static readonly ILog _logger = LogManager.GetLogger(typeof(AsteroidGenerator));

    public static ICelestialObject BuildAsteroid(int id,
        int maxDrillAttempts, float size, double direction, double x, double y, double speed, string name, bool isPreScanned = false)
    {
        try
        {
            ICelestialObject asteroid = new BaseAsteroid(maxDrillAttempts)
            {
                Id = id,
                OwnerId = 0,
                Name = name,
                Direction = direction,
                X = x,
                Y = y,
                Speed = speed,
                Type = CelestialObjectType.Asteroid,
                IsPreScanned = isPreScanned,
                Size = size
            };

            return asteroid;
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            throw;
        }

    }

    public static ICelestialObject CreateAsteroid(IGenerationTool generationTool, double direction, double x, double y, double speed, string name, bool isPreScanned = false)
    {
        return BuildAsteroid(
                generationTool.GetId(),
                generationTool.GetInteger(2, 4),
                generationTool.GetFloat(350),
                direction,
                x, y, speed, name, isPreScanned
            );
    }
}
