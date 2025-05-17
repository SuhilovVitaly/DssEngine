namespace DeepSpaceSaga.Common.Abstractions.Services;

public interface ISessionContextService
{
    ISessionInfoService SessionInfo { get; }
    IMetricsService Metrics { get; }
}
