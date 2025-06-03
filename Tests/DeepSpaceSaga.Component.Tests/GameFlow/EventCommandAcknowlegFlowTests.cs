namespace DeepSpaceSaga.Component.Tests.GameFlow;

public class EventCommandAcknowlegFlowTests
{
    [Fact]
    public void Properties_ShouldReturnInjectedInstances()
    {
        // Arrange
        var sessionInfo = new SessionInfoService();
        var metricsMock = new Mock<IMetricsService>();
        var generationToolMock = new Mock<IGenerationTool>();

        var idCounter = 10;
        generationToolMock.Setup(x => x.GetId()).Returns(() => idCounter++);
        
        ISessionContextService sessionContext = new SessionContextService(sessionInfo, metricsMock.Object, generationToolMock.Object);

        var gameServer = new LocalGameServer(new SchedulerService(sessionContext), sessionContext, new TurnProcessing());
        
        gameServer.TurnExecution(sessionInfo, CalculationType.Turn);
        var sessionContextDtoTurn0 = gameServer.GetSessionContextDto();
        
        gameServer.TurnExecution(sessionInfo, CalculationType.Turn);
        var sessionContextDtoTurn1 = gameServer.GetSessionContextDto();

        var gameEvent = sessionContextDtoTurn1.GameActionEvents.FirstOrDefault().Value;
        
        gameServer.AddCommand(new Command
        {
            CelestialObjectId = gameEvent.Id,
            Category = CommandCategory.CommandAccept,
            TargetCelestialObjectId = gameEvent.Id,
            IsPauseProcessed = true
        });
        
        gameServer.TurnExecution(sessionInfo, CalculationType.Turn);
        var sessionContextDtoTurn2 = gameServer.GetSessionContextDto();

        // Act & Assert
        gameEvent.Id.Should().Be(22);
        sessionContextDtoTurn0.GameActionEvents.Count.Should().Be(0);
        sessionContextDtoTurn1.GameActionEvents.Count.Should().Be(1);
        sessionContextDtoTurn2.GameActionEvents.Count.Should().Be(0);
        
        sessionContextDtoTurn1.State.IsPaused.Should().BeFalse();
        sessionContextDtoTurn2.State.IsPaused.Should().BeTrue();
    }
}