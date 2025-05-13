using DeepSpaceSaga.Common.Abstractions.Session.Entities;

namespace DeepSpaceSaga.Common.Abstractions.Services;

public interface IGameFlowService
{
    ISessionInfo SessionInfo { get; }
    Action<ISessionInfo, CalculationType> TurnExecution { get; set; }
    void SessionStart();
    void SessionPause();
    void SessionResume();
    void SessionStop();
}
