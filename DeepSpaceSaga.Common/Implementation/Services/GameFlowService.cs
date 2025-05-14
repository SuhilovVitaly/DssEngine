using DeepSpaceSaga.Common.Abstractions.Services;
using DeepSpaceSaga.Common.Abstractions.Session.Entities;
using DeepSpaceSaga.Common.Metrics;

namespace DeepSpaceSaga.Common.Implementation.Services;

public class GameFlowService : IGameFlowService
{
    // Public delegate for turn execution that can be set externally
    public Action<ISessionInfoService, CalculationType> TurnExecution { get; set; }

    public ISessionInfoService SessionInfo { get; }
    private readonly ITurnSchedulerService _turnSchedulerService;
    private readonly ISessionContext _sessionContext;

    public GameFlowService(ISessionInfoService sessionInfo, ITurnSchedulerService turnSchedulerService, ISessionContext sessionContext)
    {
        SessionInfo = sessionInfo;
        _turnSchedulerService = turnSchedulerService;
        _sessionContext = sessionContext;
    }

    public void SessionPause()
    {
        _sessionContext.Metrics.Add(MetricsServer.SessionPause);
        _turnSchedulerService.Stop();
    }

    public void SessionResume()
    {
        _sessionContext.Metrics.Add(MetricsServer.SessionResume);
        _turnSchedulerService.Resume();
    }

    public void SessionStart()
    {
        if (TurnExecution == null)
        {
            throw new InvalidOperationException("TurnExecution delegate must be set before starting the game flow");
        }

        _sessionContext.Metrics.Add(MetricsServer.SessionStart);
        _turnSchedulerService.Start(TurnExecution);
    }

    public void SessionStop()
    {
        _sessionContext.Metrics.Add(MetricsServer.SessionStop);
        _turnSchedulerService.Stop();
    }
}
