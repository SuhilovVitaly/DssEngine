using DeepSpaceSaga.Common.Generation.CelestialObjects;

namespace DeepSpaceSaga.Common.Generation;

public class ScenarioGenerator
{
    public static GameSession DefaultScenario(IGenerationTool _generationTool)
    {
        var session = new GameSession();

        var celestialObjects = new List<ICelestialObject>
        {
            SpacecraftGenerator.BuildPlayerSpacecraft(100, 10, 90, 10000, 10000, 10, "Glowworm"),
            AsteroidGenerator.CreateAsteroid(_generationTool, 217, 10100, 10100, 2, _generationTool.GenerateCelestialObjectName(), true),
            AsteroidGenerator.CreateAsteroid(_generationTool, 327, 9900, 9900, 5, _generationTool.GenerateCelestialObjectName(), true),
            AsteroidGenerator.CreateAsteroid(_generationTool, 327, 9500, 9500, 2, _generationTool.GenerateCelestialObjectName(), true),
            AsteroidGenerator.CreateAsteroid(_generationTool, 327, 10200, 10300, 1, _generationTool.GenerateCelestialObjectName(), true)
        };

        foreach (var celestialObject in celestialObjects)
        {
            session.CelestialObjects.TryAdd(celestialObject.Id, celestialObject);
        }

        return session;
    }
}
