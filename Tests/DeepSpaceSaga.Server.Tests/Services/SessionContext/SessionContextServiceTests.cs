namespace DeepSpaceSaga.Server.Tests.Services.SessionContext;

public class SessionContextServiceTests
{
    [Fact]
    public void Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var sessionInfoMock = new Mock<ISessionInfoService>();
        var metricsMock = new Mock<IMetricsService>();
        var generationToolMock = new Mock<IGenerationTool>();
        
        // Setup unique ID generation to prevent duplicate key errors
        var idCounter = 0;
        generationToolMock.Setup(x => x.GetId()).Returns(() => idCounter++);

        // Act
        var service = new SessionContextService(sessionInfoMock.Object, metricsMock.Object, generationToolMock.Object);

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
        var generationToolMock = new Mock<IGenerationTool>();
        
        // Setup unique ID generation to prevent duplicate key errors
        var idCounter = 10;
        generationToolMock.Setup(x => x.GetId()).Returns(() => idCounter++);
        
        var service = new SessionContextService(sessionInfoMock.Object, metricsMock.Object, generationToolMock.Object);

        // Act & Assert
        service.SessionInfo.Should().NotBeNull();
        service.Metrics.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_NullSessionInfo_ThrowsArgumentNullException()
    {
        // Arrange
        var metricsMock = new Mock<IMetricsService>();
        var generationToolMock = new Mock<IGenerationTool>();

        // Act
        var act = () => new SessionContextService(null!, metricsMock.Object, generationToolMock.Object);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Constructor_NullMetrics_ThrowsArgumentNullException()
    {
        // Arrange
        var sessionInfoMock = new Mock<ISessionInfoService>();
        var generationToolMock = new Mock<IGenerationTool>();

        // Act
        var act = () => new SessionContextService(sessionInfoMock.Object, null!, generationToolMock.Object);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }
} 