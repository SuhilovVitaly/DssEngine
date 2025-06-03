namespace DeepSpaceSaga.Common.Abstractions.Services;

public interface ISchedulerService
{
    void SessionStart(Action<CalculationType> turnExecutionCallBack);
    void SessionPause();
    void SessionResume();
    void SessionStop();
}
