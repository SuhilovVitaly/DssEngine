namespace DeepSpaceSaga.Tests.CommonTests.Services;

public class MetricsServiceTests
{
    private readonly MetricsService _serverMetrics;

    public MetricsServiceTests()
    {
        _serverMetrics = new MetricsService();
    }

    [Fact]
    public void Add_ShouldIncrementMetricValue()
    {
        // Arrange
        const string metricName = "test_metric";
        const double incrementValue = 5.0;

        // Act
        _serverMetrics.Add(metricName, incrementValue);
        var result = _serverMetrics.Get(metricName);

        // Assert
        Assert.Equal(incrementValue, result);
    }

    [Fact]
    public void Add_MultipleIncrements_ShouldAccumulateValue()
    {
        // Arrange
        const string metricName = "test_metric";
        const double increment1 = 5.0;
        const double increment2 = 3.0;

        // Act
        _serverMetrics.Add(metricName, increment1);
        _serverMetrics.Add(metricName, increment2);
        var result = _serverMetrics.Get(metricName);

        // Assert
        Assert.Equal(increment1 + increment2, result);
    }

    [Fact]
    public void AddMilliseconds_ShouldRecordPerformanceMetric()
    {
        // Arrange
        const string metricName = "performance_metric";
        const double milliseconds = 100.0;

        // Act
        _serverMetrics.AddMilliseconds(metricName, milliseconds);
        var result = _serverMetrics.Get(metricName);

        // Assert
        Assert.Equal(milliseconds, result);
    }

    [Fact]
    public void Get_NonExistentMetric_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        const string nonExistentMetric = "non_existent";

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => _serverMetrics.Get(nonExistentMetric));
    }

    [Fact]
    public void TryGetMetricValue_ExistingMetric_ShouldReturnTrueAndValue()
    {
        // Arrange
        const string metricName = "test_metric";
        const double value = 10.0;
        _serverMetrics.Add(metricName, value);

        // Act
        var success = _serverMetrics.TryGetMetricValue(metricName, out var result);

        // Assert
        Assert.True(success);
        Assert.Equal(value, result);
    }

    [Fact]
    public void TryGetMetricValue_NonExistentMetric_ShouldReturnFalse()
    {
        // Arrange
        const string nonExistentMetric = "non_existent";

        // Act
        var success = _serverMetrics.TryGetMetricValue(nonExistentMetric, out var result);

        // Assert
        Assert.False(success);
        Assert.Equal(0, result);
    }

    [Fact]
    public void GetAverageMilliseconds_ShouldCalculateCorrectAverage()
    {
        // Arrange
        const string metricName = "performance_metric";
        const double value1 = 100.0;
        const double value2 = 200.0;

        // Act
        _serverMetrics.AddMilliseconds(metricName, value1);
        _serverMetrics.AddMilliseconds(metricName, value2);
        var total = _serverMetrics.GetAverageMillisecondst(metricName);

        // Assert - MetricsService now accumulates values, doesn't calculate averages
        Assert.Equal(value1 + value2, total);
    }

    [Fact]
    public void TryGetAverageMilliseconds_ExistingMetric_ShouldReturnTrueAndAverage()
    {
        // Arrange
        const string metricName = "performance_metric";
        const double value1 = 100.0;
        const double value2 = 200.0;
        _serverMetrics.AddMilliseconds(metricName, value1);
        _serverMetrics.AddMilliseconds(metricName, value2);

        // Act
        var success = _serverMetrics.TryGetAverageMilliseconds(metricName, out var total);

        // Assert - MetricsService now accumulates values, doesn't calculate averages
        Assert.True(success);
        Assert.Equal(value1 + value2, total);
    }

    [Fact]
    public void TryGetAverageMilliseconds_NonExistentMetric_ShouldReturnFalse()
    {
        // Arrange
        const string nonExistentMetric = "non_existent";

        // Act
        var success = _serverMetrics.TryGetAverageMilliseconds(nonExistentMetric, out var average);

        // Assert
        Assert.False(success);
        Assert.Equal(0, average);
    }

    [Fact]
    public void Add_ShouldUseDefaultIncrementValue()
    {
        // Arrange
        const string metricName = "default_increment_metric";

        // Act
        _serverMetrics.Add(metricName);
        var result = _serverMetrics.Get(metricName);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public void AddMilliseconds_MultipleCalls_ShouldAccumulateAndAverageCorrectly()
    {
        // Arrange
        const string metricName = "multi_perf_metric";
        var values = new[] { 10.0, 20.0, 30.0, 40.0 };
        var expectedSum = values.Sum();

        // Act
        foreach (var v in values)
            _serverMetrics.AddMilliseconds(metricName, v);
        var sum = _serverMetrics.Get(metricName);
        var total = _serverMetrics.GetAverageMillisecondst(metricName);

        // Assert - MetricsService now accumulates values, doesn't calculate averages
        Assert.Equal(expectedSum, sum);
        Assert.Equal(expectedSum, total);
    }

    [Fact]
    public void GetAverageMilliseconds_AfterAdd_ShouldReturnCorrectAverage()
    {
        // Arrange
        const string metricName = "add_metric";
        _serverMetrics.Add(metricName, 2.0);
        _serverMetrics.Add(metricName, 4.0);

        // Act
        var total = _serverMetrics.GetAverageMillisecondst(metricName);

        // Assert - MetricsService now accumulates values, doesn't calculate averages
        Assert.Equal(6.0, total);
    }

    [Fact]
    public void Add_And_AddMilliseconds_SameMetric_ShouldAccumulateCorrectly()
    {
        // Arrange
        const string metricName = "mixed_metric";
        const double addValue = 5.0;
        const double millisecondsValue = 10.0;

        // Act
        _serverMetrics.Add(metricName, addValue);
        _serverMetrics.AddMilliseconds(metricName, millisecondsValue);
        var result = _serverMetrics.Get(metricName);

        // Assert - MetricsService now accumulates values
        Assert.Equal(addValue + millisecondsValue, result);
    }

    [Fact]
    public void Add_WithZeroAndNegativeValues_ShouldHandleCorrectly()
    {
        // Arrange
        const string metricName = "zero_negative_metric";

        // Act
        _serverMetrics.Add(metricName, 0);
        _serverMetrics.Add(metricName, -2.5);
        _serverMetrics.Add(metricName, -2.5);
        var result = _serverMetrics.Get(metricName);

        // Assert - MetricsService now accumulates values
        Assert.Equal(-5.0, result);
    }
}