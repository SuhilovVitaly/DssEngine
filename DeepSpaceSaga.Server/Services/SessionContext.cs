namespace DeepSpaceSaga.Server.Services;

public class SessionContext : ISessionContext
{
    public IMetricsService Metrics { get; }

    public ISessionInfoService SessionInfo { get; }

    public ISaveLoadService SaveLoadManager { get; }

    public SessionContext(IMetricsService metrics, ISessionInfoService sessionInfo, ISaveLoadService saveLoadManager)
    {
        Metrics = metrics;
        SessionInfo = sessionInfo;
        SaveLoadManager = saveLoadManager;
    }
}
