namespace DeepSpaceSaga.Server.Services.SessionContext;

public class SessionContextService(ISessionInfoService sessionInfo, IMetricsService metrics) : ISessionContextService
{
    public ISessionInfoService SessionInfo { get; } = sessionInfo;
    public IMetricsService Metrics { get; } = metrics;
}
