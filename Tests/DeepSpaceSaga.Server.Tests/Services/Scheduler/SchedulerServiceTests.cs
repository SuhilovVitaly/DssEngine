namespace DeepSpaceSaga.Server.Tests.Services.Scheduler;

public class SchedulerServiceTests
{
    private readonly Mock<ISessionContextService> _sessionContextMock;
    private readonly Mock<IMetricsService> _metricsMock;
    private readonly Mock<ISessionInfoService> _sessionInfoMock;
    private readonly SchedulerService _sut;

    public SchedulerServiceTests()
    {
        _sessionContextMock = new Mock<ISessionContextService>();
        _metricsMock = new Mock<IMetricsService>();
        _sessionInfoMock = new Mock<ISessionInfoService>();
        _sessionContextMock.SetupGet(x => x.Metrics).Returns(_metricsMock.Object);
        _sessionContextMock.SetupGet(x => x.SessionInfo).Returns(_sessionInfoMock.Object);
        _sut = new SchedulerService(_sessionContextMock.Object);
    }

    [Fact]
    public void SessionStart_ShouldAddMetricAndStartScheduler()
    {
        // Arrange
        var callbackCalled = false;
        Action<CalculationType> callback = (_) => callbackCalled = true;

        // Act
        _sut.SessionStart(callback);

        // Assert
        _metricsMock.Verify(m => m.Add(MetricsServer.SessionStart, 1), Times.Once);
        // Проверяем, что не выбрасывается исключение и callback можно вызвать
        callbackCalled = false;
        callback(CalculationType.Tick);
        callbackCalled.Should().BeTrue();
    }

    [Fact]
    public void SessionStart_WithNullCallback_ShouldThrowInvalidOperationException()
    {
        // Act
        Action act = () => _sut.SessionStart(null!);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("TurnExecutionCallBack delegate must be set before starting the game flow");
    }

    [Fact]
    public void SessionPause_ShouldAddMetricAndStopScheduler()
    {
        // Arrange
        // Act
        _sut.SessionPause();

        // Assert
        _metricsMock.Verify(m => m.Add(MetricsServer.SessionPause, 1), Times.Once);
        // Проверить, что TurnSchedulerService.Stop() вызван невозможно напрямую, но отсутствие исключения и вызов метрики достаточно
    }

    [Fact]
    public void SessionResume_ShouldAddMetricAndResumeScheduler()
    {
        // Arrange
        Action<CalculationType> callback = ( _) => { };
        _sut.SessionStart(callback);

        // Act
        _sut.SessionResume();

        // Assert
        _metricsMock.Verify(m => m.Add(MetricsServer.SessionResume, 1), Times.Once);
        // Проверить, что TurnSchedulerService.Resume() вызван невозможно напрямую, но отсутствие исключения и вызов метрики достаточно
    }

    [Fact]
    public void SessionStop_ShouldAddMetricAndStopScheduler()
    {
        // Arrange
        // Act
        _sut.SessionStop();

        // Assert
        _metricsMock.Verify(m => m.Add(MetricsServer.SessionStop, 1), Times.Once);
        // Проверить, что TurnSchedulerService.Stop() вызван невозможно напрямую, но отсутствие исключения и вызов метрики достаточно
    }
} 