namespace DeepSpaceSaga.Server.Services.Scheduler;

public class SchedulerService(ISessionContextService sessionContext) : ISchedulerService
{
    private readonly TurnSchedulerService _turnSchedulerService = new(sessionContext.SessionInfo);

    public void SessionPause()
    {
        sessionContext.Metrics.Add(Common.Metrics.MetricsServer.SessionPause);
        _turnSchedulerService.Stop();
    }

    public void SessionResume()
    {
        sessionContext.Metrics.Add(Common.Metrics.MetricsServer.SessionResume);
        _turnSchedulerService.Resume();
    }

    public void SessionStart(Action<ISessionInfoService, CalculationType> turnExecutionCallBack)
    {
        if (turnExecutionCallBack == null)
        {
            throw new InvalidOperationException("TurnExecutionCallBack delegate must be set before starting the game flow");
        }

        sessionContext.Metrics.Add(Common.Metrics.MetricsServer.SessionStart);
        _turnSchedulerService.Start(turnExecutionCallBack);
    }

    public void SessionStop()
    {
        sessionContext.Metrics.Add(Common.Metrics.MetricsServer.SessionStop);
        _turnSchedulerService.Stop();
    }
}
