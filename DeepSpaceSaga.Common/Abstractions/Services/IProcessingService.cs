namespace DeepSpaceSaga.Common.Abstractions.Services;

public interface IProcessingService
{
    GameSessionDto Process(ISessionContextService sessionContext);

    GameSessionDto PauseProcess(ISessionContextService sessionContext);
}
