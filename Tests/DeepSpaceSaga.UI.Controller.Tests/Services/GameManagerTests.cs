using DeepSpaceSaga.Common.Abstractions.Dto.Ui;
using DeepSpaceSaga.Common.Abstractions.Entities;
using DeepSpaceSaga.Common.Abstractions.Entities.Commands;
using DeepSpaceSaga.Common.Abstractions.Services;
using DeepSpaceSaga.Common.Abstractions.UI;
using DeepSpaceSaga.Common.Abstractions.UI.Screens;
using DeepSpaceSaga.Common.Tools;
using DeepSpaceSaga.UI.Controller.Services;
using Moq;

namespace DeepSpaceSaga.UI.Controller.Tests.Services;

public class GameManagerTests
{
    private static GameManager CreateManager(
        out Mock<IGameServer> gameServer,
        out Mock<IScreensService> screens,
        out Mock<IGenerationTool> generationTool,
        out Mock<IOuterSpaceService> outerSpace,
        out Mock<IScreenResolution> screenResolution,
        out Mock<IScenarioService> scenarioService,
        out Mock<ILocalizationService> localization)
    {
        gameServer = new Mock<IGameServer>(MockBehavior.Strict);
        screens = new Mock<IScreensService>(MockBehavior.Strict);
        generationTool = new Mock<IGenerationTool>(MockBehavior.Loose);
        outerSpace = new Mock<IOuterSpaceService>(MockBehavior.Loose);
        screenResolution = new Mock<IScreenResolution>(MockBehavior.Strict);
        scenarioService = new Mock<IScenarioService>(MockBehavior.Strict);
        localization = new Mock<ILocalizationService>(MockBehavior.Strict);

        // Provide minimal expectations for properties used in ctor and methods
        screenResolution.SetupAllProperties();
        screens.SetupProperty(s => s.TacticalMap);

        var manager = new GameManager(
            gameServer.Object,
            screens.Object,
            generationTool.Object,
            outerSpace.Object,
            screenResolution.Object,
            scenarioService.Object,
            localization.Object);

        return manager;
    }

    [Fact]
    public void Constructor_ShouldWireOnTurnExecute_AndExposeLocalization()
    {
        // Arrange
        var manager = CreateManager(
            out var gameServer,
            out var screens,
            out var generationTool,
            out var outerSpace,
            out var screenResolution,
            out var scenarioService,
            out var localization);

        var received = 0;
        manager.OnUpdateGameData += _ => received++;
        var dto = new GameSessionDto { Id = Guid.NewGuid() };

        // Act
        gameServer.Raise(s => s.OnTurnExecute += null, dto);

        // Assert
        Assert.Equal(1, received);
        Assert.Equal(dto, manager.GameSession());
        Assert.Equal(localization.Object, manager.Localization);
    }

    [Fact]
    public async Task SaveGame_ShouldCallServer_AndReturnZero()
    {
        // Arrange
        var manager = CreateManager(
            out var gameServer,
            out var screens,
            out var generationTool,
            out var outerSpace,
            out var screenResolution,
            out var scenarioService,
            out var localization);

        gameServer.Setup(s => s.SaveGame("slot1")).Returns(Task.CompletedTask).Verifiable();

        // Act
        var result = await manager.SaveGame("slot1");

        // Assert
        Assert.Equal(0, result);
        gameServer.Verify();
    }

    [Fact]
    public async Task LoadGame_ShouldCallServer_AndReturnZero()
    {
        // Arrange
        var manager = CreateManager(
            out var gameServer,
            out var screens,
            out var generationTool,
            out var outerSpace,
            out var screenResolution,
            out var scenarioService,
            out var localization);

        gameServer.Setup(s => s.LoadGame("slot2")).Returns(Task.CompletedTask).Verifiable();

        // Act
        var result = await manager.LoadGame("slot2");

        // Assert
        Assert.Equal(0, result);
        gameServer.Verify();
    }

    [Fact]
    public void SetGameSpeed_ShouldForwardToServer()
    {
        // Arrange
        var manager = CreateManager(
            out var gameServer,
            out var screens,
            out var generationTool,
            out var outerSpace,
            out var screenResolution,
            out var scenarioService,
            out var localization);

        gameServer.Setup(s => s.SetGameSpeed(3));

        // Act
        manager.SetGameSpeed(3);

        // Assert
        gameServer.Verify(s => s.SetGameSpeed(3), Times.Once);
    }

    [Fact]
    public void CommandExecute_ShouldCallAddCommand()
    {
        // Arrange
        var manager = CreateManager(
            out var gameServer,
            out var screens,
            out var generationTool,
            out var outerSpace,
            out var screenResolution,
            out var scenarioService,
            out var localization);

        var cmd = new Mock<ICommand>().Object;
        gameServer.Setup(s => s.AddCommand(cmd)).Returns(Task.CompletedTask).Verifiable();

        // Act
        manager.CommandExecute(cmd);

        // Assert
        gameServer.Verify();
    }

    [Fact]
    public void SessionStart_ShouldStartServer_SetOuterSpace_AndShowTacticalMap()
    {
        // Arrange
        var manager = CreateManager(
            out var gameServer,
            out var screens,
            out var generationTool,
            out var outerSpace,
            out var screenResolution,
            out var scenarioService,
            out var localization);

        var scenario = new GameSession();
        scenarioService.Setup(s => s.GetScenario()).Returns(scenario);
        gameServer.Setup(s => s.SessionStart(scenario));
        screens.Setup(s => s.ShowTacticalMapScreen());

        var initialOuterSpace = manager.OuterSpace;

        // Act
        manager.SessionStart();

        // Assert
        gameServer.Verify(s => s.SessionStart(scenario), Times.Once);
        screens.Verify(s => s.ShowTacticalMapScreen(), Times.Once);
        Assert.NotSame(initialOuterSpace, manager.OuterSpace);
        Assert.IsType<OuterSpaceService>(manager.OuterSpace);
    }

    [Fact]
    public void SessionPause_And_Resume_ShouldForwardToServer()
    {
        // Arrange
        var manager = CreateManager(
            out var gameServer,
            out var screens,
            out var generationTool,
            out var outerSpace,
            out var screenResolution,
            out var scenarioService,
            out var localization);

        gameServer.Setup(s => s.SessionPause());
        gameServer.Setup(s => s.SessionResume());

        // Act
        manager.SessionPause();
        manager.SessionResume();

        // Assert
        gameServer.Verify(s => s.SessionPause(), Times.Once);
        gameServer.Verify(s => s.SessionResume(), Times.Once);
    }

    [Fact]
    public void SessionStop_ShouldForwardToServer_AndCloseTacticalMap()
    {
        // Arrange
        var manager = CreateManager(
            out var gameServer,
            out var screens,
            out var generationTool,
            out var outerSpace,
            out var screenResolution,
            out var scenarioService,
            out var localization);

        gameServer.Setup(s => s.SessionStop());
        screens.Setup(s => s.CloseTacticalMapScreen());

        // Act
        manager.SessionStop();

        // Assert
        gameServer.Verify(s => s.SessionStop(), Times.Once);
        screens.Verify(s => s.CloseTacticalMapScreen(), Times.Once);
    }

    [Fact]
    public void ShowScreens_ShouldCallScreensService()
    {
        // Arrange
        var manager = CreateManager(
            out var gameServer,
            out var screens,
            out var generationTool,
            out var outerSpace,
            out var screenResolution,
            out var scenarioService,
            out var localization);

        screens.Setup(s => s.ShowTacticalMapScreen());
        screens.Setup(s => s.ShowGameMenuScreen());

        // Act
        manager.ShowTacticalMapScreen();
        manager.ShowMainMenuScreen();

        // Assert
        screens.Verify(s => s.ShowTacticalMapScreen(), Times.Once);
        screens.Verify(s => s.ShowGameMenuScreen(), Times.Once);
    }

    [Fact]
    public void GameEventInvoke_ShouldPauseSession_AndStartDialogOnTacticalMap()
    {
        // Arrange
        var manager = CreateManager(
            out var gameServer,
            out var screens,
            out var generationTool,
            out var outerSpace,
            out var screenResolution,
            out var scenarioService,
            out var localization);

        var tacticalMap = new Mock<IScreenTacticalMap>(MockBehavior.Strict);
        screens.Object.TacticalMap = tacticalMap.Object;

        gameServer.Setup(s => s.SessionPause());
        tacticalMap.Setup(t => t.StartDialog(It.IsAny<GameActionEventDto>()));

        var gameEvent = new GameActionEventDto { Key = "TEST_EVENT" };

        // Act
        manager.GameEventInvoke(gameEvent);

        // Assert
        gameServer.Verify(s => s.SessionPause(), Times.Once);
        tacticalMap.Verify(t => t.StartDialog(gameEvent), Times.Once);
    }
}



