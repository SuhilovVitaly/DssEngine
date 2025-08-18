using DeepSpaceSaga.Server.Processing.Handlers;
using DeepSpaceSaga.Common.Abstractions.Entities.Commands;
using DeepSpaceSaga.Common.Implementation.Entities.Commands;
using DeepSpaceSaga.Common.Implementation.Entities.Events;

namespace DeepSpaceSaga.Server.Tests.Processing.Handlers;

public class ProcessingEventAcknowledgeHandlerTests
{
    private readonly Mock<ISessionContextService> _mockSessionContext;
    private readonly Mock<IMetricsService> _mockMetrics;
    private readonly Mock<ISessionInfoService> _mockSessionInfo;
    private readonly GameSession _gameSession;
    private readonly ProcessingEventAcknowledgeHandler _handler;

    public ProcessingEventAcknowledgeHandlerTests()
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
            FinishedEvents = new ConcurrentDictionary<string, string>()
        };

        _mockSessionContext.Setup(x => x.SessionInfo).Returns(_mockSessionInfo.Object);
        _mockSessionContext.Setup(x => x.Metrics).Returns(_mockMetrics.Object);
        _mockSessionContext.Setup(x => x.GameSession).Returns(_gameSession);
        
        // Setup lock methods
        _mockSessionContext.Setup(x => x.EnterWriteLock()).Verifiable();
        _mockSessionContext.Setup(x => x.ExitWriteLock()).Verifiable();
        
        _handler = new ProcessingEventAcknowledgeHandler();
    }

    [Fact]
    public void Execute_WithCommandAcceptCommands_ShouldProcessAndRemoveThem()
    {
        // Arrange
        var command1 = new Command
        {
            Id = Guid.NewGuid(),
            Category = CommandCategory.CommandAccept
        };
        
        var command2 = new Command
        {
            Id = Guid.NewGuid(),
            Category = CommandCategory.CommandAccept
        };

        var activeEvent1 = new BaseGameEvent { Key = "event1" };
        var activeEvent2 = new BaseGameEvent { Key = "event2" };

        _gameSession.Commands.TryAdd(command1.Id, command1);
        _gameSession.Commands.TryAdd(command2.Id, command2);
        _gameSession.ActiveEvents.TryAdd("event1", activeEvent1);
        _gameSession.ActiveEvents.TryAdd("event2", activeEvent2);

        // Act
        _handler.Execute(_mockSessionContext.Object);

        // Assert
        _gameSession.Commands.Should().BeEmpty();
        _gameSession.ActiveEvents.Should().BeEmpty();
        _gameSession.FinishedEvents.Should().HaveCount(2);
        _gameSession.FinishedEvents.Should().ContainKey("event1");
        _gameSession.FinishedEvents.Should().ContainKey("event2");
        
        // Verify metrics were called
        _mockMetrics.Verify(x => x.Add(MetricsServer.ProcessingEventAcknowledgeReceived, 1), Times.Exactly(2));
        _mockMetrics.Verify(x => x.Add(MetricsServer.ProcessingEventAcknowledgeProcessed, 1), Times.Exactly(2));
        _mockMetrics.Verify(x => x.Add(MetricsServer.ProcessingEventAcknowledgeRemoved, 1), Times.Exactly(2));
        
        // Verify lock methods were called
        _mockSessionContext.Verify(x => x.EnterWriteLock(), Times.Exactly(1));
        _mockSessionContext.Verify(x => x.ExitWriteLock(), Times.Exactly(1));
    }

    [Fact]
    public void Execute_WithNonCommandAcceptCommands_ShouldNotProcessThem()
    {
        // Arrange
        var command1 = new Command
        {
            Id = Guid.NewGuid(),
            Category = CommandCategory.DialogExit
        };
        
        var command2 = new Command
        {
            Id = Guid.NewGuid(),
            Category = CommandCategory.Mining
        };

        _gameSession.Commands.TryAdd(command1.Id, command1);
        _gameSession.Commands.TryAdd(command2.Id, command2);

        // Act
        _handler.Execute(_mockSessionContext.Object);

        // Assert
        _gameSession.Commands.Should().HaveCount(2);
        _gameSession.FinishedEvents.Should().BeEmpty();
        
        // Verify metrics were not called
        _mockMetrics.Verify(x => x.Add(MetricsServer.ProcessingEventAcknowledgeReceived, 1), Times.Never);
        _mockMetrics.Verify(x => x.Add(MetricsServer.ProcessingEventAcknowledgeProcessed, 1), Times.Never);
        _mockMetrics.Verify(x => x.Add(MetricsServer.ProcessingEventAcknowledgeRemoved, 1), Times.Never);
        
        // Verify lock methods were called even with no processing
        _mockSessionContext.Verify(x => x.EnterWriteLock(), Times.Exactly(1));
        _mockSessionContext.Verify(x => x.ExitWriteLock(), Times.Exactly(1));
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
        _gameSession.FinishedEvents.Should().BeEmpty();
        
        // Verify metrics were not called
        _mockMetrics.Verify(x => x.Add(MetricsServer.ProcessingEventAcknowledgeReceived, 1), Times.Never);
        _mockMetrics.Verify(x => x.Add(MetricsServer.ProcessingEventAcknowledgeProcessed, 1), Times.Never);
        _mockMetrics.Verify(x => x.Add(MetricsServer.ProcessingEventAcknowledgeRemoved, 1), Times.Never);
        
        // Verify lock methods were called
        _mockSessionContext.Verify(x => x.EnterWriteLock(), Times.Exactly(1));
        _mockSessionContext.Verify(x => x.ExitWriteLock(), Times.Exactly(1));
    }

    [Fact]
    public void Execute_WithMixedCommands_ShouldOnlyProcessCommandAccept()
    {
        // Arrange
        var acceptCommand = new Command
        {
            Id = Guid.NewGuid(),
            Category = CommandCategory.CommandAccept
        };
        
        var otherCommand = new Command
        {
            Id = Guid.NewGuid(),
            Category = CommandCategory.DialogExit
        };

        var activeEvent = new BaseGameEvent { Key = "event1" };

        _gameSession.Commands.TryAdd(acceptCommand.Id, acceptCommand);
        _gameSession.Commands.TryAdd(otherCommand.Id, otherCommand);
        _gameSession.ActiveEvents.TryAdd("event1", activeEvent);

        // Act
        _handler.Execute(_mockSessionContext.Object);

        // Assert
        _gameSession.Commands.Should().HaveCount(1);
        _gameSession.Commands.Should().ContainKey(otherCommand.Id);
        _gameSession.FinishedEvents.Should().HaveCount(1);
        _gameSession.FinishedEvents.Should().ContainKey("event1");
        
        // Verify metrics were called only once
        _mockMetrics.Verify(x => x.Add(MetricsServer.ProcessingEventAcknowledgeReceived, 1), Times.Exactly(1));
        _mockMetrics.Verify(x => x.Add(MetricsServer.ProcessingEventAcknowledgeProcessed, 1), Times.Exactly(1));
        _mockMetrics.Verify(x => x.Add(MetricsServer.ProcessingEventAcknowledgeRemoved, 1), Times.Exactly(1));
        
        // Verify lock methods were called
        _mockSessionContext.Verify(x => x.EnterWriteLock(), Times.Exactly(1));
        _mockSessionContext.Verify(x => x.ExitWriteLock(), Times.Exactly(1));
    }

    [Fact]
    public void Execute_ShouldUseWriteLock_ForThreadSafety()
    {
        // Arrange
        var command = new Command
        {
            Id = Guid.NewGuid(),
            Category = CommandCategory.CommandAccept
        };

        _gameSession.Commands.TryAdd(command.Id, command);

        // Act
        _handler.Execute(_mockSessionContext.Object);

        // Assert
        // Verify write lock methods were called for thread safety
        _mockSessionContext.Verify(x => x.EnterWriteLock(), Times.Exactly(1));
        _mockSessionContext.Verify(x => x.ExitWriteLock(), Times.Exactly(1));
    }
}
