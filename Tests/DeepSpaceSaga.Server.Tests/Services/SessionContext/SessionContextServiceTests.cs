namespace DeepSpaceSaga.Server.Tests.Services.SessionContext;

public class SessionContextServiceTests : IDisposable
{
    private SessionContextService? _service;

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
        _service = new SessionContextService(sessionInfoMock.Object, metricsMock.Object, generationToolMock.Object);

        // Assert
        _service.SessionInfo.Should().BeSameAs(sessionInfoMock.Object);
        _service.Metrics.Should().BeSameAs(metricsMock.Object);
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
        
        _service = new SessionContextService(sessionInfoMock.Object, metricsMock.Object, generationToolMock.Object);

        // Act & Assert
        _service.SessionInfo.Should().NotBeNull();
        _service.Metrics.Should().NotBeNull();
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

    [Fact]
    public void LockMethods_ShouldWorkCorrectly()
    {
        // Arrange
        var sessionInfoMock = new Mock<ISessionInfoService>();
        var metricsMock = new Mock<IMetricsService>();
        var generationToolMock = new Mock<IGenerationTool>();
        
        var idCounter = 0;
        generationToolMock.Setup(x => x.GetId()).Returns(() => idCounter++);
        
        _service = new SessionContextService(sessionInfoMock.Object, metricsMock.Object, generationToolMock.Object);

        // Act & Assert - should not throw exceptions
        var action = () =>
        {
            _service.EnterReadLock();
            _service.ExitReadLock();
            _service.EnterWriteLock();
            _service.ExitWriteLock();
        };

        action.Should().NotThrow();
    }

    [Fact]
    public void MultipleReadLocks_ShouldWorkConcurrently()
    {
        // Arrange
        var sessionInfoMock = new Mock<ISessionInfoService>();
        var metricsMock = new Mock<IMetricsService>();
        var generationToolMock = new Mock<IGenerationTool>();
        
        var idCounter = 0;
        generationToolMock.Setup(x => x.GetId()).Returns(() => idCounter++);
        
        _service = new SessionContextService(sessionInfoMock.Object, metricsMock.Object, generationToolMock.Object);

        // Act & Assert - multiple read locks should work
        var action = () =>
        {
            _service.EnterReadLock();
            _service.EnterReadLock();
            _service.ExitReadLock();
            _service.ExitReadLock();
        };

        action.Should().NotThrow();
    }

    [Fact]
    public void Dispose_ShouldNotThrowException()
    {
        // Arrange
        var sessionInfoMock = new Mock<ISessionInfoService>();
        var metricsMock = new Mock<IMetricsService>();
        var generationToolMock = new Mock<IGenerationTool>();
        
        var idCounter = 0;
        generationToolMock.Setup(x => x.GetId()).Returns(() => idCounter++);
        
        _service = new SessionContextService(sessionInfoMock.Object, metricsMock.Object, generationToolMock.Object);

        // Act & Assert
        var action = () => _service.Dispose();
        action.Should().NotThrow();
    }

    public void Dispose()
    {
        _service?.Dispose();
    }
} 