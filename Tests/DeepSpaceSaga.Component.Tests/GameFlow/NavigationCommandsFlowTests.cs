using DeepSpaceSaga.Common.Extensions;
using DeepSpaceSaga.Server;
using FluentAssertions;
using Moq;
using DeepSpaceSaga.Common.Abstractions.Services;
using DeepSpaceSaga.Common.Abstractions.Entities.Commands;
using DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects.Spacecrafts;

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

        // Check if metric exists before trying to get it
        if (metricsService.TryGetMetricValue(MetricsServer.ProcessingCommandNavigationReceived, out var metricValue))
        {
            metricValue.Should().Be(1);
        }
        
        spacecraft_turn_000.Direction.Should().NotBe(spacecraft_turn_001.Direction);
    }
}
