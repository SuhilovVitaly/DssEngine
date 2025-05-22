namespace DeepSpaceSaga.Server.Services.SessionContext;

public class SessionContextService : ISessionContextService
{
    public ISessionInfoService SessionInfo { get; }
    public IMetricsService Metrics { get; }
    public GameSession GameSession { get; set; }

    public SessionContextService(ISessionInfoService sessionInfo, IMetricsService metrics)
    {
        SessionInfo = sessionInfo ?? throw new ArgumentNullException(nameof(sessionInfo));
        Metrics = metrics ?? throw new ArgumentNullException(nameof(metrics));

        GameSession = new GameSession();
    }    
}
