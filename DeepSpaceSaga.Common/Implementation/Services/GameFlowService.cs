using DeepSpaceSaga.Common.Abstractions.Services;
using DeepSpaceSaga.Common.Abstractions.Session.Entities;
using DeepSpaceSaga.Common.Metrics;

namespace DeepSpaceSaga.Common.Implementation.Services;

public class GameFlowService : IGameFlowService
{
    // Public delegate for turn execution that can be set externally
    public Action<ISessionInfoService, CalculationType> TurnExecution { get; set; }

    public ISessionInfoService SessionInfo { get; }
    private readonly IExecutor _executor;
    private readonly ISessionContext _sessionContext;

    public GameFlowService(ISessionInfoService sessionInfo, IExecutor executor, ISessionContext sessionContext)
    {
        SessionInfo = sessionInfo;
        _executor = executor;
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

    public void SessionStart()
    {
        _sessionContext.Metrics.Add(MetricsServer.SessionStart);
        _executor.Start(TurnExecution);
    }

    public void SessionStop()
    {
        _sessionContext.Metrics.Add(MetricsServer.SessionStop);
        _executor.Stop();
    }
}
