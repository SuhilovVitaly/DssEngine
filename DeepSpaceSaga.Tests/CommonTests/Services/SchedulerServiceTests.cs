using DeepSpaceSaga.Server.Services.Scheduler;

namespace DeepSpaceSaga.Tests.CommonTests.Services;

public class SchedulerServiceTests
{
    private readonly Mock<ISessionInfoService> _sessionInfoMock;
    private readonly Mock<TurnSchedulerService> _executorMock;
    private readonly Mock<ISessionContextService> _sessionContextMock;
    private readonly Mock<IMetricsService> _metricsMock;
    private readonly SchedulerService _sut;

    public SchedulerServiceTests()
    {
        _sessionInfoMock = new Mock<ISessionInfoService>();
        _executorMock = new Mock<TurnSchedulerService>();
        _sessionContextMock = new Mock<ISessionContextService>();
        _metricsMock = new Mock<IMetricsService>();
        _sessionContextMock.Setup(x => x.Metrics).Returns(_metricsMock.Object);
        _sut = new SchedulerService(_sessionContextMock.Object);
    }
} 