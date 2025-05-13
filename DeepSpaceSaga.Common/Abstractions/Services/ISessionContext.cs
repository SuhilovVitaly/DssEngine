namespace DeepSpaceSaga.Common.Abstractions.Services;

public interface ISessionContext
{
    IMetricsService Metrics { get; }
}
