namespace DeepSpaceSaga.Common.Abstractions.Services;

public interface IWorkerService
{
    event Action<string, GameSessionDTO>? OnGetDataFromServer;

    void StartProcessing();

    void PauseProcessing();

    void ResumeProcessing();

    Task StopProcessing();
}
