namespace DeepSpaceSaga.Common.Abstractions.Services;

public interface IProcessingService
{
    GameSessionDto ScenarioStartProcess(ISessionContextService sessionContext);

    GameSessionDto DialogProcess(ISessionContextService sessionContext);

    GameSessionDto Process(ISessionContextService sessionContext);

    GameSessionDto PauseProcess(ISessionContextService sessionContext);
}
