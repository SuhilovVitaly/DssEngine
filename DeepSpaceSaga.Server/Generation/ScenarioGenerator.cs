using DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects;
using DeepSpaceSaga.Common.Tools;
using DeepSpaceSaga.Server.Generation.CelestialObjects;

namespace DeepSpaceSaga.Server.Generation;

public class ScenarioGenerator
{
    public static GameSession DefaultScenario(IGenerationTool _generationTool)
    {
        var session = new GameSession();

        var celestialObjects = new List<ICelestialObject>
        {
            AsteroidGenerator.CreateAsteroid(_generationTool, 217, 10100, 10100, 0, _generationTool.GenerateCelestialObjectName(), true),
            AsteroidGenerator.CreateAsteroid(_generationTool, 327, 9900, 9900, 0, _generationTool.GenerateCelestialObjectName(), true),
            AsteroidGenerator.CreateAsteroid(_generationTool, 327, 9500, 9500, 0, _generationTool.GenerateCelestialObjectName(), true),
            AsteroidGenerator.CreateAsteroid(_generationTool, 327, 10000, 10000, 0, _generationTool.GenerateCelestialObjectName(), true)
        };

        foreach (var celestialObject in celestialObjects)
        {
            session.CelestialObjects.Add(celestialObject.Id, celestialObject);
        }

        return session;
    }
}
