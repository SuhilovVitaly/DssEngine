using System.Collections.Concurrent;

namespace DeepSpaceSaga.Server.Services.Metrics
{
    public class MetricsService : IMetricsService
    {
        private readonly ConcurrentDictionary<string, MetricData> _metrics = new();

        /// <summary>
        /// Adds or updates the value of a metric by the specified increment.
        /// </summary>
        /// <param name="metric">The metric to update.</param>
        /// <param name="incrementValue">The value to add to the metric.</param>
        public void Add(string metric, double incrementValue = 1)
        {
            _metrics.AddOrUpdate(
                metric,
                new MetricData(incrementValue, 1, incrementValue),
                (_, data) => data with
                {
                    CurrentValue = data.CurrentValue + incrementValue,
                    Count = data.Count + 1,
                    Average = (data.CurrentValue + incrementValue) / (data.Count + 1)
                }
            );
        }

        /// <summary>
        /// Records a performance metric in milliseconds and updates its average.
        /// </summary>
        /// <param name="metric">The metric to update.</param>
        /// <param name="milliseconds">The time in milliseconds to record.</param>
        public void AddMilliseconds(string metric, double milliseconds)
        {
            _metrics.AddOrUpdate(
                metric,
                new MetricData(milliseconds, 1, milliseconds),
                (_, data) => data with
                {
                    CurrentValue = data.CurrentValue + milliseconds,
                    Count = data.Count + 1,
                    Average = (data.CurrentValue + milliseconds) / (data.Count + 1)
                }
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
            if (_metrics.TryGetValue(metric, out var data))
            {
                return data.CurrentValue;
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
            if (_metrics.TryGetValue(metric, out var data))
            {
                value = data.CurrentValue;
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
            if (_metrics.TryGetValue(metric, out var data))
            {
                return data.Average;
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
            if (_metrics.TryGetValue(metric, out var data))
            {
                average = data.Average;
                return true;
            }

            average = 0;
            return false;
        }

        /// <summary>
        /// A record to store metric data.
        /// </summary>
        private record MetricData(double CurrentValue, int Count, double Average);
    }
}
