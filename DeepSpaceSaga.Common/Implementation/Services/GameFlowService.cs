using DeepSpaceSaga.Common.Abstractions.Services;
using DeepSpaceSaga.Common.Abstractions.Session.Entities;

namespace DeepSpaceSaga.Common.Implementation.Services;

public class GameFlowService : IGameFlowService
{
    // Public delegate for turn execution that can be set externally
    public Action<ISessionInfo, CalculationType> TurnExecution { get; set; }

    public ISessionInfo SessionInfo { get; }
    private readonly IExecutor _executor;

    public GameFlowService(ISessionInfo sessionInfo, IExecutor executor)
    {
        SessionInfo = sessionInfo;
        _executor = executor;
    }

    public void SessionPause()
    {
        _executor.Stop();
    }

    public void SessionResume()
    {
        _executor.Resume();
    }

    public void SessionStart()
    {
        _executor.Start(TurnExecution);
    }

    public void SessionStop()
    {
        _executor.Stop();
    }
}
