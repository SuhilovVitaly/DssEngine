using DeepSpaceSaga.Common.Abstractions.Services;
using DeepSpaceSaga.Common.Implementation;
using DeepSpaceSaga.Controller.GameLoopTools;
using DeepSpaceSaga.Server;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace DeepSpaceSaga.Controller
{
    public class WorkerService : IWorkerService, IDisposable
    {
        public event Action<GameSessionDTO>? OnGetDataFromServer;

        private Executor _gameLoopExecutor;
        private readonly IGameServer _gameServer;
        private bool _isDisposed;
        private CancellationTokenSource? _cancellationTokenSource;
        private Task? _gameLoop;
        private bool _isRunning;

        public WorkerService(Executor gameLoopExecutor)
        {
            _gameServer = new LocalGameServer();
            _gameLoopExecutor = gameLoopExecutor;
        }

        public void StartProcessing()
        {
            if (_isRunning)
            {
                Debug.WriteLine("StartProcessing called but already running");
                return;
            }

            Debug.WriteLine("StartProcessing called");
            _cancellationTokenSource = new CancellationTokenSource();

            _gameLoopExecutor.Start(Calculation);

            _isRunning = true;
            Debug.WriteLine("Game loop started in background");
        }

        public async Task StopProcessing()
        {
            if (!_isRunning)
            {
                Debug.WriteLine("StopProcessing called but not running");
                return;
            }

            Debug.WriteLine("StopProcessing called");
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                if (_gameLoop != null)
                {
                    await _gameLoop;
                    Debug.WriteLine("Game loop stopped");
                }
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;
                _gameLoop = null;
                _isRunning = false;
            }
        }

        private void Calculation(ExecutorState state, CalculationType type)
        {
            Console.WriteLine($"{state} {type} calculation");
            var session = _gameServer.TurnCalculation();
            OnGetDataFromServer?.Invoke(session);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    Debug.WriteLine("Disposing WorkerService");
                    StopProcessing().Wait();
                    _cancellationTokenSource?.Dispose();
                }
                _isDisposed = true;
            }
        }

        ~WorkerService()
        {
            Dispose(false);
        }
    }
}
