using DeepSpaceSaga.Common.Abstractions.Services;
using DeepSpaceSaga.Common.Implementation;
using DeepSpaceSaga.Server;

namespace DeepSpaceSaga.Controller
{
    public class WorkerService: IWorkerService
    {
        private IGameServer _gameServer;

        public WorkerService()
        {
            _gameServer = new LocalGameServer();
        }

        public event Action<GameSessionDTO>? OnGetDataFromServer;
    }
}
