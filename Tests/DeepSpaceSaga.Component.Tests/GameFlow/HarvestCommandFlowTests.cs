namespace DeepSpaceSaga.Component.Tests.GameFlow;

public class HarvestCommandFlowTests
{
    [Fact]
    public void Properties_ShouldReturnInjectedInstances()
    {
        // Arrange
        var sessionInfo = new SessionInfoService();
        var metricsMock = new Mock<IMetricsService>();
        var saveLoadServiceMock = new Mock<ISaveLoadService>();
        var generationToolMock = new Mock<IGenerationTool>();

        ISessionContextService sessionContext =
            new SessionContextService(sessionInfo, metricsMock.Object, generationToolMock.Object);

            var gameServer =new LocalGameServer(new SchedulerService(sessionContext), sessionContext, new TurnProcessing(),
            saveLoadServiceMock.Object);

            gameServer.TurnExecution(CalculationType.Turn);
        var sessionContextDtoTurn0 = gameServer.GetSessionContextDto();

        // gameServer.AddCommand(new Command
        // {
        //     CelestialObjectId = gameEvent.Id,
        //     Category = CommandCategory.CommandAccept,
        //     TargetCelestialObjectId = gameEvent.Id,
        //     IsPauseProcessed = true
        // });
    }
}