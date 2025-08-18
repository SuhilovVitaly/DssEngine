using DeepSpaceSaga.Server;
using DeepSpaceSaga.Server.Services.Metrics;

namespace DeepSpaceSaga.Component.Tests.GameFlow;

public class EventCommandAcknowlegFlowTests
{
    [Fact]
    public void Properties_ShouldReturnInjectedInstances()
    {
        // Arrange
        var sessionInfo = new SessionInfoService();
        var metricsService = new MetricsService();
        var saveLoadServiceMock = new Mock<ISaveLoadService>();
        var generationToolMock = new Mock<IGenerationTool>();

        var idCounter = 10;
        generationToolMock.Setup(x => x.GetId()).Returns(() => idCounter++);
        
        ISessionContextService sessionContext = new SessionContextService(sessionInfo, metricsService, generationToolMock.Object);

        var gameServer = new LocalGameServer(new SchedulerService(sessionContext), sessionContext, new TurnProcessing(), saveLoadServiceMock.Object);
        
        gameServer.TurnExecution(CalculationType.Turn);
        var sessionContextDtoTurn0 = gameServer.GetSessionContextDto();
        
        gameServer.TurnExecution(CalculationType.Turn);
        var sessionContextDtoTurn1 = gameServer.GetSessionContextDto();

        gameServer.AddCommand(new Command
        {
            Category = CommandCategory.CommandAccept,
            IsPauseProcessed = true
        });
        
        gameServer.TurnExecution(CalculationType.Turn);
        var sessionContextDtoTurn2 = gameServer.GetSessionContextDto();

        // Act & Assert
        metricsService.Get(MetricsServer.ServerTurnRealTimeProcessing).Should().Be(3);
        metricsService.Get(MetricsServer.ServerTurnPauseProcessing).Should().Be(1);
        
        metricsService.Get(MetricsServer.ServerCommandReceived).Should().Be(1);
        
        metricsService.Get(MetricsServer.ProcessingEventAcknowledgeReceived).Should().Be(1);
        metricsService.Get(MetricsServer.ProcessingEventAcknowledgeProcessed).Should().Be(1);
        metricsService.Get(MetricsServer.ProcessingEventAcknowledgeRemoved).Should().Be(1);
    }
}