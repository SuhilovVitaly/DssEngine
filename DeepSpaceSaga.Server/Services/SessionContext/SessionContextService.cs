namespace DeepSpaceSaga.Server.Services.SessionContext;

public class SessionContextService : ISessionContextService
{
    public ISessionInfoService SessionInfo { get; }
    public IMetricsService Metrics { get; }
    
    
    public SessionContextService(ISessionInfoService sessionInfo, IMetricsService metrics)
    {
        SessionInfo = sessionInfo ?? throw new ArgumentNullException(nameof(sessionInfo));
        Metrics = metrics ?? throw new ArgumentNullException(nameof(metrics));
    }
    
    
}
