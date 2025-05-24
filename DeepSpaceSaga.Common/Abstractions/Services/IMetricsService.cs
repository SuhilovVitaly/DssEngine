namespace DeepSpaceSaga.Common.Abstractions.Services;

public interface IMetricsService
{
    void Add(string metric, double incrementValue = 1);

    void AddMilliseconds(string metric, double milliseconds);

    double Get(string metric);

    double GetAverageMillisecondst(string metric);

    void Reset();
}
