using DeepSpaceSaga.Common.Implementation;

namespace DeepSpaceSaga.Common.Abstractions.Services
{
    public interface IWorkerService
    {
        event Action<GameSessionDTO>? OnGetDataFromServer;

        void StartProcessing();

        Task StopProcessing();
    }
}
