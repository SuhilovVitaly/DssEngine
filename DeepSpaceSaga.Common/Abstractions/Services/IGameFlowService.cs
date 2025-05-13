namespace DeepSpaceSaga.Common.Abstractions.Services;

public interface IGameFlowService
{
    ISessionInfoService SessionInfo { get; }
    Action<ISessionInfoService, CalculationType> TurnExecution { get; set; }
    void SessionStart();
    void SessionPause();
    void SessionResume();
    void SessionStop();
}
