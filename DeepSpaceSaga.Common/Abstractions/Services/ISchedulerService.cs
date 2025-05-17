namespace DeepSpaceSaga.Common.Abstractions.Services;

public interface ISchedulerService
{
    ISessionInfoService SessionInfo { get; }
    void SessionStart(Action<ISessionInfoService, CalculationType> turnExecution);
    void SessionPause();
    void SessionResume();
    void SessionStop();
}
