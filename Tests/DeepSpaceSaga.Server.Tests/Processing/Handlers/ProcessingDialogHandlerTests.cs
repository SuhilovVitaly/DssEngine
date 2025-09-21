using DeepSpaceSaga.Server.Processing.Handlers;
using DeepSpaceSaga.Common.Abstractions.Entities.Commands;
using DeepSpaceSaga.Common.Implementation.Entities.Commands;
using DeepSpaceSaga.Common.Implementation.Entities.Dialogs;

namespace DeepSpaceSaga.Server.Tests.Processing.Handlers;

public class ProcessingDialogHandlerTests
{
    private readonly Mock<ISessionContextService> _mockSessionContext;
    private readonly Mock<IMetricsService> _mockMetrics;
    private readonly Mock<ISessionInfoService> _mockSessionInfo;
    private readonly GameSession _gameSession;
    private readonly ProcessingDialogHandler _handler;

    public ProcessingDialogHandlerTests()
    {
        _mockSessionContext = new Mock<ISessionContextService>();
        _mockMetrics = new Mock<IMetricsService>();
        _mockSessionInfo = new Mock<ISessionInfoService>();
        
        _gameSession = new GameSession
        {
            Id = Guid.NewGuid(),
            CelestialObjects = new ConcurrentDictionary<int, ICelestialObject>(),
            Commands = new ConcurrentDictionary<Guid, ICommand>(),
            ActiveEvents = new ConcurrentDictionary<string, IGameActionEvent>(),
            FinishedEvents = new ConcurrentDictionary<string, string>(),
            DialogsExits = new ConcurrentDictionary<string, string>()
        };

        _mockSessionContext.Setup(x => x.SessionInfo).Returns(_mockSessionInfo.Object);
        _mockSessionContext.Setup(x => x.Metrics).Returns(_mockMetrics.Object);
        _mockSessionContext.Setup(x => x.GameSession).Returns(_gameSession);
        
        // Setup lock methods
        _mockSessionContext.Setup(x => x.EnterWriteLock()).Verifiable();
        _mockSessionContext.Setup(x => x.ExitWriteLock()).Verifiable();
        
        _handler = new ProcessingDialogHandler();
    }

    [Fact]
    public void Execute_WithDialogExitCommands_ShouldProcessAndRemoveThem()
    {
        // Arrange
        var dialogExitCommand1 = new DialogExitCommand
        {
            Id = Guid.NewGuid(),
            Category = CommandCategory.DialogExit,
            DialogKey = "dialog1",
            DialogExitKey = "exit1",
            DialogCommand = new DialogCommand { Name = "command1" }
        };
        
        var dialogExitCommand2 = new DialogExitCommand
        {
            Id = Guid.NewGuid(),
            Category = CommandCategory.DialogExit,
            DialogKey = "dialog2",
            DialogExitKey = "exit2",
            DialogCommand = new DialogCommand { Name = "command2" }
        };

        _gameSession.Commands.TryAdd(dialogExitCommand1.Id, dialogExitCommand1);
        _gameSession.Commands.TryAdd(dialogExitCommand2.Id, dialogExitCommand2);

        // Act
        _handler.Execute(_mockSessionContext.Object);

        // Assert
        _gameSession.Commands.Should().BeEmpty();
        _gameSession.DialogsExits.Should().HaveCount(2);
        _gameSession.DialogsExits.Should().ContainKey("dialog1");
        _gameSession.DialogsExits.Should().ContainKey("dialog2");
        _gameSession.DialogsExits["dialog1"].Should().Be("exit1");
        _gameSession.DialogsExits["dialog2"].Should().Be("exit2");
        
        // Verify lock methods were called
        _mockSessionContext.Verify(x => x.EnterWriteLock(), Times.Once);
        _mockSessionContext.Verify(x => x.ExitWriteLock(), Times.Once);
    }

    [Fact]
    public void Execute_WithNonDialogExitCommands_ShouldNotProcessThem()
    {
        // Arrange
        var otherCommand1 = new Command
        {
            Id = Guid.NewGuid(),
            Category = CommandCategory.Mining
        };
        
        var otherCommand2 = new Command
        {
            Id = Guid.NewGuid(),
            Category = CommandCategory.CommandAccept
        };

        _gameSession.Commands.TryAdd(otherCommand1.Id, otherCommand1);
        _gameSession.Commands.TryAdd(otherCommand2.Id, otherCommand2);

        // Act
        _handler.Execute(_mockSessionContext.Object);

        // Assert
        _gameSession.Commands.Should().HaveCount(2);
        _gameSession.DialogsExits.Should().BeEmpty();
        
        // Verify lock methods were called even with no processing
        _mockSessionContext.Verify(x => x.EnterWriteLock(), Times.Once);
        _mockSessionContext.Verify(x => x.ExitWriteLock(), Times.Once);
    }

    [Fact]
    public void Execute_WithEmptyCommands_ShouldNotProcessAnything()
    {
        // Arrange
        // No commands added

        // Act
        _handler.Execute(_mockSessionContext.Object);

        // Assert
        _gameSession.Commands.Should().BeEmpty();
        _gameSession.DialogsExits.Should().BeEmpty();
        
        // Verify lock methods were called
        _mockSessionContext.Verify(x => x.EnterWriteLock(), Times.Once);
        _mockSessionContext.Verify(x => x.ExitWriteLock(), Times.Once);
    }

    [Fact]
    public void Execute_WithMixedCommands_ShouldOnlyProcessDialogExit()
    {
        // Arrange
        var dialogExitCommand = new DialogExitCommand
        {
            Id = Guid.NewGuid(),
            Category = CommandCategory.DialogExit,
            DialogKey = "dialog1",
            DialogExitKey = "exit1",
            DialogCommand = new DialogCommand { Name = "command1" }
        };
        
        var otherCommand = new Command
        {
            Id = Guid.NewGuid(),
            Category = CommandCategory.Mining
        };

        _gameSession.Commands.TryAdd(dialogExitCommand.Id, dialogExitCommand);
        _gameSession.Commands.TryAdd(otherCommand.Id, otherCommand);

        // Act
        _handler.Execute(_mockSessionContext.Object);

        // Assert
        _gameSession.Commands.Should().HaveCount(1);
        _gameSession.Commands.Should().ContainKey(otherCommand.Id);
        _gameSession.DialogsExits.Should().HaveCount(1);
        _gameSession.DialogsExits.Should().ContainKey("dialog1");
        _gameSession.DialogsExits["dialog1"].Should().Be("exit1");
        
        // Verify lock methods were called
        _mockSessionContext.Verify(x => x.EnterWriteLock(), Times.Once);
        _mockSessionContext.Verify(x => x.ExitWriteLock(), Times.Once);
    }

    [Fact]
    public void Execute_WithNullDialogExitCommand_ShouldSkipIt()
    {
        // Arrange
        var nullCommand = new Command
        {
            Id = Guid.NewGuid(),
            Category = CommandCategory.DialogExit
        };
        // Note: This is not a DialogExitCommand, so it will be skipped

        _gameSession.Commands.TryAdd(nullCommand.Id, nullCommand);

        // Act
        _handler.Execute(_mockSessionContext.Object);

        // Assert
        _gameSession.Commands.Should().HaveCount(1); // Command not removed because it's not DialogExitCommand
        _gameSession.DialogsExits.Should().BeEmpty();
        
        // Verify lock methods were called
        _mockSessionContext.Verify(x => x.EnterWriteLock(), Times.Once);
        _mockSessionContext.Verify(x => x.ExitWriteLock(), Times.Once);
    }

    [Fact]
    public void Execute_ShouldUseWriteLock_ForThreadSafety()
    {
        // Arrange
        var dialogExitCommand = new DialogExitCommand
        {
            Id = Guid.NewGuid(),
            Category = CommandCategory.DialogExit,
            DialogKey = "dialog1",
            DialogExitKey = "exit1",
            DialogCommand = new DialogCommand { Name = "command1" }
        };

        _gameSession.Commands.TryAdd(dialogExitCommand.Id, dialogExitCommand);

        // Act
        _handler.Execute(_mockSessionContext.Object);

        // Assert
        // Verify write lock methods were called for thread safety
        _mockSessionContext.Verify(x => x.EnterWriteLock(), Times.Once);
        _mockSessionContext.Verify(x => x.ExitWriteLock(), Times.Once);
    }
}
