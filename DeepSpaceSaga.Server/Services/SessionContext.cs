namespace DeepSpaceSaga.Server.Services;

public class SessionContext : ISessionContext
{
    public IMetricsService Metrics { get; }

    public SessionContext(IMetricsService metrics)
    {
        Metrics = metrics;
    }
}
