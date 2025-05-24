namespace DeepSpaceSaga.Server.Services.Scheduler;

public class SchedulerService(ISessionContextService sessionContext) : ISchedulerService
{
    private readonly TurnSchedulerService _turnSchedulerService = new(sessionContext);

    public void SessionPause()
    {
        sessionContext.Metrics.Add(MetricsServer.SessionPause);
        _turnSchedulerService.Stop();
        sessionContext.SessionInfo.IsPaused = true;
        sessionContext.SessionInfo.IsCalculationInProgress = false;
    }

    public void SessionResume()
    {
        sessionContext.Metrics.Add(MetricsServer.SessionResume);
        _turnSchedulerService.Resume();
        sessionContext.SessionInfo.IsPaused = false;
        sessionContext.SessionInfo.IsCalculationInProgress = false;
    }

    public void SessionStart(Action<ISessionInfoService, CalculationType> turnExecutionCallBack)
    {
        if (turnExecutionCallBack == null)
        {
            throw new InvalidOperationException("TurnExecutionCallBack delegate must be set before starting the game flow");
        }

        sessionContext.Metrics.Add(MetricsServer.SessionStart);
        _turnSchedulerService.Start(turnExecutionCallBack);
        sessionContext.SessionInfo.IsPaused = false;
        sessionContext.SessionInfo.IsCalculationInProgress = false;
    }

    public void SessionStop()
    {
        sessionContext.Metrics.Add(MetricsServer.SessionStop);
        _turnSchedulerService.Stop();
    }
}
