namespace DeepSpaceSaga.Component.Tests.GameFlow;

public class EventCommandAcknowlegFlowTests
{
    [Fact]
    public void Properties_ShouldReturnInjectedInstances()
    {
        // Arrange
        var sessionInfo = new SessionInfoService();
        var metricsMock = new Mock<IMetricsService>();
        var saveLoadServiceMock = new Mock<ISaveLoadService>();
        var generationToolMock = new Mock<IGenerationTool>();

        var idCounter = 10;
        generationToolMock.Setup(x => x.GetId()).Returns(() => idCounter++);
        
        ISessionContextService sessionContext = new SessionContextService(sessionInfo, metricsMock.Object, generationToolMock.Object);

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
        sessionContextDtoTurn1.State.IsPaused.Should().BeFalse();
        sessionContextDtoTurn2.State.IsPaused.Should().BeTrue();
    }
}