namespace DeepSpaceSaga.Server.Services;

public class SessionContext(ISessionInfoService sessionInfo, IMetricsService metrics) : ISessionContext
{
    public ISessionInfoService SessionInfo { get; } = sessionInfo;
    public IMetricsService Metrics { get; } = metrics;
}
