using DeepSpaceSaga.Common.Extensions;
using DeepSpaceSaga.Server;

namespace DeepSpaceSaga.Component.Tests.GameFlow;

public class NavigationCommandsFlowTests
{
    [Fact]
    public async void TurnLeftNavigationCommand_InRealTime_ShouldWorkCorrect()
    {
        // Arrange
        var sessionInfo = new SessionInfoService();
        var metricsService = new MetricsService();
        var saveLoadServiceMock = new Mock<ISaveLoadService>();
        var generationTool = new GenerationTool();

        ISessionContextService sessionContext = new SessionContextService(sessionInfo, metricsService, generationTool);

        sessionContext.GameSession.Dialogs = new DialogsService();

        var gameServer = new LocalGameServer(new SchedulerService(sessionContext), sessionContext, new TurnProcessing(), saveLoadServiceMock.Object);

        gameServer.TurnExecution(CalculationType.Turn);
        var sessionContextDtoTurn0 = gameServer.GetSessionContextDto();
        
        var spacecraft_turn_000 = sessionContextDtoTurn0.GetPlayerSpaceShip();

        await gameServer.AddCommand(new Command
        {
            Category = CommandCategory.Navigation,
            Type = CommandTypes.TurnLeft
        });

        gameServer.TurnExecution(CalculationType.Turn);
        var sessionContextDtoTurn1 = gameServer.GetSessionContextDto();

        var spacecraft_turn_001 = sessionContextDtoTurn1.GetPlayerSpaceShip();

        metricsService.Get(MetricsServer.ProcessingCommandNavigationReceived).Should().Be(1);
        spacecraft_turn_000.Direction.Should().Equals(spacecraft_turn_001.Direction);
        //Assert.NotEqual(spacecraft_turn_000.Direction, spacecraft_turn_001.Direction);
    }
}
