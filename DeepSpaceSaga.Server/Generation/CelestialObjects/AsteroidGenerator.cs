using DeepSpaceSaga.Common.Abstractions.Entities;
using DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects;
using DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects.Asteroids;
using DeepSpaceSaga.Common.Tools;

namespace DeepSpaceSaga.Server.Generation.CelestialObjects;

public class AsteroidGenerator
{
    private static readonly ILog _logger = LogManager.GetLogger(typeof(AsteroidGenerator));

    public static ICelestialObject CreateAsteroid(IGenerationTool generationTool, double direction, double x, double y, double speed, string name, bool isPreScanned = false)
    {
        try
        {
            var maxDrillAttempts = generationTool.GetInteger(2, 4);

            ICelestialObject asteroid = new BaseAsteroid(maxDrillAttempts)
            {
                Id = generationTool.GetId(),
                OwnerId = 0,
                Name = name,
                Direction = direction,
                X = x,
                Y = y,
                Speed = speed,
                Type = CelestialObjectType.Asteroid,
                IsPreScanned = isPreScanned,
                Size = generationTool.GetFloat(350)
            };

            return asteroid;
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            throw;
        }

    }
}
