namespace DeepSpaceSaga.Common.Abstractions.Services;

public interface ISessionContext
{
    ISessionInfoService SessionInfo { get; }
    IMetricsService Metrics { get; }
}
