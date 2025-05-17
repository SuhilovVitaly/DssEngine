using DeepSpaceSaga.Common.Implementation.Services.GameLoopTools;

namespace DeepSpaceSaga.Server.Services;

public class SchedulerService : ISchedulerService
{
    public ISessionInfoService SessionInfo { get; }
    private readonly Executor _executor;
    private readonly ISessionContext _sessionContext;

    public SchedulerService(ISessionInfoService sessionInfo, ISessionContext sessionContext)
    {
        SessionInfo = sessionInfo;
        _executor = new Executor(SessionInfo);
        _sessionContext = sessionContext;
    }

    public void SessionPause()
    {
        _sessionContext.Metrics.Add(MetricsServer.SessionPause);
        _executor.Stop();
    }

    public void SessionResume()
    {
        _sessionContext.Metrics.Add(MetricsServer.SessionResume);
        _executor.Resume();
    }

    public void SessionStart(Action<ISessionInfoService, CalculationType> turnExecution)
    {
        if (turnExecution == null)
        {
            throw new InvalidOperationException("TurnExecution delegate must be set before starting the game flow");
        }

        _sessionContext.Metrics.Add(MetricsServer.SessionStart);
        _executor.Start(turnExecution);
    }

    public void SessionStop()
    {
        _sessionContext.Metrics.Add(MetricsServer.SessionStop);
        _executor.Stop();
    }
}
