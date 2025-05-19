namespace DeepSpaceSaga.Tests.ServerTests.Services.SessionContext;

public class SessionContextServiceTests
{
    [Fact]
    public void Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var sessionInfoMock = new Mock<ISessionInfoService>();
        var metricsMock = new Mock<IMetricsService>();

        // Act
        var service = new SessionContextService(sessionInfoMock.Object, metricsMock.Object);

        // Assert
        service.SessionInfo.Should().BeSameAs(sessionInfoMock.Object);
        service.Metrics.Should().BeSameAs(metricsMock.Object);
    }

    [Fact]
    public void Properties_ShouldReturnInjectedInstances()
    {
        // Arrange
        var sessionInfoMock = new Mock<ISessionInfoService>();
        var metricsMock = new Mock<IMetricsService>();
        var service = new SessionContextService(sessionInfoMock.Object, metricsMock.Object);

        // Act & Assert
        service.SessionInfo.Should().NotBeNull();
        service.Metrics.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_NullSessionInfo_ThrowsArgumentNullException()
    {
        // Arrange
        var metricsMock = new Mock<IMetricsService>();

        // Act
        var act = () => new SessionContextService(null!, metricsMock.Object);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Constructor_NullMetrics_ThrowsArgumentNullException()
    {
        // Arrange
        var sessionInfoMock = new Mock<ISessionInfoService>();

        // Act
        var act = () => new SessionContextService(sessionInfoMock.Object, null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }
}