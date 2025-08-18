namespace DeepSpaceSaga.Server.Services.Metrics;

public class MetricsService : IMetricsService
{
    private readonly ConcurrentDictionary<string, long> _counters = new();
    private readonly ConcurrentDictionary<string, double> _gauges = new();

    /// <summary>
    /// Adds or updates the value of a metric by the specified increment.
    /// </summary>
    /// <param name="metric">The metric to update.</param>
    /// <param name="incrementValue">The value to add to the metric.</param>
    public void Add(string metric, double incrementValue = 1)
    {
        _gauges.AddOrUpdate(
            metric,
            incrementValue,
            (_, value) => value + incrementValue
        );
    }

    public void Reset()
    {
        _gauges.Clear();
    }

    /// <summary>
    /// Records a performance metric in milliseconds and updates its average.
    /// </summary>
    /// <param name="metric">The metric to update.</param>
    /// <param name="milliseconds">The time in milliseconds to record.</param>
    public void AddMilliseconds(string metric, double milliseconds)
    {
        _gauges.AddOrUpdate(
            metric,
            milliseconds,
            (_, value) => value + milliseconds
        );
    }

    /// <summary>
    /// Gets the current value of a metric.
    /// </summary>
    /// <param name="metric">The metric to retrieve.</param>
    /// <returns>The current value of the metric.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the metric is not found.</exception>
    public double Get(string metric)
    {
        if (_gauges.TryGetValue(metric, out var value))
        {
            return value;
        }

        throw new KeyNotFoundException($"Metric '{metric}' not found.");
    }

    /// <summary>
    /// Attempts to get the current value of a metric.
    /// </summary>
    /// <param name="metric">The metric to retrieve.</param>
    /// <param name="value">The current value of the metric if found.</param>
    /// <returns>True if the metric was found, otherwise false.</returns>
    public bool TryGetMetricValue(string metric, out double value)
    {
        if (_gauges.TryGetValue(metric, out value))
        {
            return true;
        }

        value = 0;
        return false;
    }

    /// <summary>
    /// Gets the precomputed average value of a performance metric in milliseconds.
    /// </summary>
    /// <param name="metric">The metric to retrieve.</param>
    /// <returns>The average value of the metric in milliseconds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the metric is not found.</exception>
    public double GetAverageMillisecondst(string metric)
    {
        if (_gauges.TryGetValue(metric, out var value))
        {
            return value;
        }

        throw new KeyNotFoundException($"Metric '{metric}' not found.");
    }

    /// <summary>
    /// Attempts to get the precomputed average value of a performance metric in milliseconds.
    /// </summary>
    /// <param name="metric">The metric to retrieve.</param>
    /// <param name="average">The precomputed average value if the metric is found.</param>
    /// <returns>True if the metric was found, otherwise false.</returns>
    public bool TryGetAverageMilliseconds(string metric, out double average)
    {
        if (_gauges.TryGetValue(metric, out average))
        {
            return true;
        }

        average = 0;
        return false;
    }
}
