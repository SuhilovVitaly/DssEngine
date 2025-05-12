using DeepSpaceSaga.Common.Abstractions.Session.Entities;

namespace DeepSpaceSaga.Controller;

public class WorkerService : IWorkerService, IDisposable
{
    private static readonly ILog Logger = LogManager.GetLogger(Settings.LoggerRepository, typeof(WorkerService));

    public event Action<string, GameSessionDTO>? OnGetDataFromServer;

    private Executor _gameLoopExecutor;
    private readonly IGameServer _gameServer;
    private bool _isDisposed;
    private CancellationTokenSource? _cancellationTokenSource;
    private Task? _gameLoop;
    private bool _isRunning;

    public WorkerService(Executor gameLoopExecutor, IGameServer server)
    {
        _gameServer = server;
        _gameLoopExecutor = gameLoopExecutor;
    }

    public void StartProcessing()
    {
        if (_isDisposed)
            throw new ObjectDisposedException(nameof(WorkerService));
            
        if (_isRunning)
        {
            Logger.Error("StartProcessing called but already running");
            return;
        }

        Logger.Info("StartProcessing called");
        _cancellationTokenSource = new CancellationTokenSource();

        _gameLoopExecutor.Start(Calculation);

        _isRunning = true;
        Logger.Info("Game loop started in background");
    }

    public void PauseProcessing()
    {
        _gameLoopExecutor.Stop();
    }

    public void ResumeProcessing()
    {
        _gameLoopExecutor.Resume();
    }

    public async Task StopProcessing()
    {
        if (!_isRunning)
        {
            Logger.Info("StopProcessing called but not running");
            return;
        }

        Logger.Info("StopProcessing called");
        if (_cancellationTokenSource != null)
        {
            _cancellationTokenSource.Cancel();
            if (_gameLoop != null)
            {
                await _gameLoop;
                Logger.Info("Game loop stopped");
            }
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
            _gameLoop = null;
            _isRunning = false;
        }
    }

    private void Calculation(SessionInfo state, CalculationType type)
    {
        try
        {
            Debug.WriteLine($"{state.ToString()} {type} calculation");
            var session = _gameServer.TurnCalculation(type);
            OnGetDataFromServer?.Invoke(state.ToString(), session);
        }
        catch (Exception ex)
        {
            Logger.Error($"Error during calculation: {ex.Message}", ex);
        }
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
