using DeepSpaceSaga.Server.Services;

namespace DeepSpaceSaga.Tests.CommonTests.Services;

public class SchedulerServiceTests
{
    private readonly Mock<ISessionInfoService> _sessionInfoMock;
    private readonly Mock<ITurnSchedulerService> _executorMock;
    private readonly Mock<ISessionContext> _sessionContextMock;
    private readonly Mock<IMetricsService> _metricsMock;
    private readonly SchedulerService _sut;

    public SchedulerServiceTests()
    {
        _sessionInfoMock = new Mock<ISessionInfoService>();
        _executorMock = new Mock<ITurnSchedulerService>();
        _sessionContextMock = new Mock<ISessionContext>();
        _metricsMock = new Mock<IMetricsService>();
        _sessionContextMock.Setup(x => x.Metrics).Returns(_metricsMock.Object);
        _sut = new SchedulerService(_sessionContextMock.Object);
    }
} 