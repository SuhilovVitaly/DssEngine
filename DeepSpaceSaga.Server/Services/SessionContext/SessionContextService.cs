using DeepSpaceSaga.Common.Generation;
using DeepSpaceSaga.Common.Tools;

namespace DeepSpaceSaga.Server.Services.SessionContext;

public class SessionContextService : ISessionContextService, IDisposable
{
    private readonly ReaderWriterLockSlim _sessionLock = new();
    
    public ISessionInfoService SessionInfo { get; }
    public IMetricsService Metrics { get; }
    public GameSession GameSession { get; set; }

    public SessionContextService(ISessionInfoService sessionInfo, IMetricsService metrics, IGenerationTool generationTool)
    {
        SessionInfo = sessionInfo ?? throw new ArgumentNullException(nameof(sessionInfo));
        Metrics = metrics ?? throw new ArgumentNullException(nameof(metrics));

        GameSession = ScenarioGenerator.DefaultScenario(generationTool);
    }
    
    /// <summary>
    /// Acquires a read lock for thread-safe reading of session data
    /// </summary>
    public void EnterReadLock() => _sessionLock.EnterReadLock();
    
    /// <summary>
    /// Releases the read lock
    /// </summary>
    public void ExitReadLock() => _sessionLock.ExitReadLock();
    
    /// <summary>
    /// Acquires a write lock for thread-safe modification of session data
    /// </summary>
    public void EnterWriteLock() => _sessionLock.EnterWriteLock();
    
    /// <summary>
    /// Releases the write lock
    /// </summary>
    public void ExitWriteLock() => _sessionLock.ExitWriteLock();
    
    /// <summary>
    /// Disposes the lock
    /// </summary>
    public void Dispose()
    {
        _sessionLock?.Dispose();
        GC.SuppressFinalize(this);
    }
}
