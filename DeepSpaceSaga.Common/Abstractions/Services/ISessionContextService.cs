namespace DeepSpaceSaga.Common.Abstractions.Services;

public interface ISessionContextService
{   
    ISessionInfoService SessionInfo { get; }
    IMetricsService Metrics { get; }
    GameSession GameSession { get; set; }
    
    /// <summary>
    /// Acquires a read lock for thread-safe reading of session data
    /// </summary>
    void EnterReadLock();
    
    /// <summary>
    /// Releases the read lock
    /// </summary>
    void ExitReadLock();
    
    /// <summary>
    /// Acquires a write lock for thread-safe modification of session data
    /// </summary>
    void EnterWriteLock();
    
    /// <summary>
    /// Releases the write lock
    /// </summary>
    void ExitWriteLock();
}
