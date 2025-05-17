namespace DeepSpaceSaga.Common.Abstractions.Services;

public interface ISessionContext
{
    IMetricsService Metrics { get; }

    ISessionInfoService SessionInfo { get; }

    ISaveLoadService SaveLoadManager { get; }

}
