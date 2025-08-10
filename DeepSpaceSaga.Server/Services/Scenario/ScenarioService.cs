namespace DeepSpaceSaga.Server.Services.Scenario;

public class ScenarioService : IScenarioService
{
    private readonly IGenerationTool _generationTool;
    public ScenarioService(IGenerationTool generationTool) 
    {
        _generationTool = generationTool;
    }

    public GameSession GetScenario()
    {
        // TODO: Load celestial objects and player spacecraft from scenario file
        var session = ScenarioGenerator.DefaultScenario(_generationTool);

        session.Dialogs = new DialogsService("Default");

        return session;
    }
}
