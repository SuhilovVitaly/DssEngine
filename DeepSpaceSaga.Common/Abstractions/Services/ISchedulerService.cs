namespace DeepSpaceSaga.Common.Abstractions.Services;

public interface ISchedulerService
{
    void SessionStart(Action<ISessionInfoService, CalculationType> turnExecutionCallBack);
    void SessionPause();
    void SessionResume();
    void SessionStop();
}
