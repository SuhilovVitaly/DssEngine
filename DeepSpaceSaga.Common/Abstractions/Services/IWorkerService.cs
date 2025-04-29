using DeepSpaceSaga.Common.Implementation;

namespace DeepSpaceSaga.Common.Abstractions.Services
{
    public interface IWorkerService
    {
        event Action<string, GameSessionDTO>? OnGetDataFromServer;

        void StartProcessing();

        Task StopProcessing();
    }
}
