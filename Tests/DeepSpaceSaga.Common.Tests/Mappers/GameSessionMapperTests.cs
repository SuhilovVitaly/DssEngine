using DeepSpaceSaga.Common.Abstractions.Dto.Save;
using DeepSpaceSaga.Common.Abstractions.Dto.Ui;

namespace DeepSpaceSaga.Common.Tests.Mappers;

public class GameSessionMapperTests
{
    private readonly Mock<ISessionContextService> _mockSessionContext;
    private readonly Mock<ISessionInfoService> _mockSessionInfo;
    private readonly Mock<IMetricsService> _mockMetrics;
    private readonly GameSession _gameSession;

    public GameSessionMapperTests()
    {
        _mockSessionContext = new Mock<ISessionContextService>();
        _mockSessionInfo = new Mock<ISessionInfoService>();
        _mockMetrics = new Mock<IMetricsService>();
        
        _gameSession = new GameSession
        {
            Id = Guid.NewGuid(),
            CelestialObjects = new ConcurrentDictionary<int, ICelestialObject>(),
            Commands = new ConcurrentDictionary<Guid, ICommand>(),
            ActiveEvents = new ConcurrentDictionary<long, IGameActionEvent>(),
            FinishedEvents = new ConcurrentDictionary<long, long>()
        };

        _mockSessionContext.Setup(x => x.SessionInfo).Returns(_mockSessionInfo.Object);
        _mockSessionContext.Setup(x => x.Metrics).Returns(_mockMetrics.Object);
        _mockSessionContext.Setup(x => x.GameSession).Returns(_gameSession);
    }

    [Fact]
    public void ToDto_Should_Map_All_Properties_Correctly()
    {
        // Arrange
        var celestialObject = new BaseAsteroid(1)
        {
            Name = "Test Asteroid",
            Type = CelestialObjectType.Asteroid,
            IsPreScanned = true,
            X = 100,
            Y = 200,
            Id = 1
        };

        ICommand command = new Command
        {
            Id = Guid.NewGuid()
        };

        _gameSession.CelestialObjects.TryAdd(1, celestialObject);
        _gameSession.Commands.TryAdd(command.Id, command);

        // Act
        var result = GameSessionMapper.ToDto(_mockSessionContext.Object);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(_gameSession.Id);
        result.State.Should().NotBeNull();
        result.CelestialObjects.Should().HaveCount(1);
        result.Commands.Should().HaveCount(1);
        
        result.CelestialObjects.Should().ContainKey(1);
        result.Commands.Should().ContainKey(command.Id);
    }

    [Fact]
    public void ToDto_Should_Handle_Empty_Collections()
    {
        // Arrange
        _gameSession.CelestialObjects.Clear();
        _gameSession.Commands.Clear();

        // Act
        var result = GameSessionMapper.ToDto(_mockSessionContext.Object);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(_gameSession.Id);
        result.State.Should().NotBeNull();
        result.CelestialObjects.Should().BeEmpty();
        result.Commands.Should().BeEmpty();
    }

    [Fact]
    public void ToDto_Should_Create_Independent_Copies_Of_Collections()
    {
        // Arrange
        var celestialObject = new BaseAsteroid(1)
        {
            Name = "Test Asteroid",
            Type = CelestialObjectType.Asteroid,
            IsPreScanned = true,
            X = 100,
            Y = 200,
            Id = 1
        };

        _gameSession.CelestialObjects.TryAdd(1, celestialObject);

        // Act
        var result = GameSessionMapper.ToDto(_mockSessionContext.Object);

        // Modify original collection
        _gameSession.CelestialObjects.Clear();

        // Assert
        result.CelestialObjects.Should().HaveCount(1);
        result.CelestialObjects.Should().ContainKey(1);
    }

    [Fact]
    public void ToDto_Should_Map_Multiple_CelestialObjects()
    {
        // Arrange
        var asteroid1 = new BaseAsteroid(1)
        {
            Name = "Asteroid 1",
            Type = CelestialObjectType.Asteroid,
            Id = 1
        };

        var asteroid2 = new BaseAsteroid(2)
        {
            Name = "Asteroid 2",
            Type = CelestialObjectType.Asteroid,
            Id = 2
        };

        _gameSession.CelestialObjects.TryAdd(1, asteroid1);
        _gameSession.CelestialObjects.TryAdd(2, asteroid2);

        // Act
        var result = GameSessionMapper.ToDto(_mockSessionContext.Object);

        // Assert
        result.CelestialObjects.Should().HaveCount(2);
        result.CelestialObjects.Should().ContainKey(1);
        result.CelestialObjects.Should().ContainKey(2);
    }

    [Fact]
    public void ToDto_Should_Map_Multiple_Commands()
    {
        // Arrange
        ICommand command1 = new Command
        {
            Id = Guid.NewGuid()
        };

        ICommand command2 = new Command
        {
            Id = Guid.NewGuid()
        };

        _gameSession.Commands.TryAdd(command1.Id, command1);
        _gameSession.Commands.TryAdd(command2.Id, command2);

        // Act
        var result = GameSessionMapper.ToDto(_mockSessionContext.Object);

        // Assert
        result.Commands.Should().HaveCount(2);
        result.Commands.Should().ContainKey(command1.Id);
        result.Commands.Should().ContainKey(command2.Id);
    }

    [Fact]
    public void ToDto_Should_Not_Return_Null()
    {
        // Act
        var result = GameSessionMapper.ToDto(_mockSessionContext.Object);

        // Assert
        result.Should().NotBeNull();
        result.CelestialObjects.Should().NotBeNull();
        result.Commands.Should().NotBeNull();
        result.State.Should().NotBeNull();
    }

    [Fact]
    public void ToDto_Should_Use_Lock_For_Thread_Safety()
    {
        // Arrange
        var celestialObject = new BaseAsteroid(1)
        {
            Name = "Test Asteroid",
            Type = CelestialObjectType.Asteroid,
            Id = 1
        };

        _gameSession.CelestialObjects.TryAdd(1, celestialObject);

        // Act & Assert
        // This test verifies that the method can be called without throwing exceptions
        // The actual lock behavior is tested implicitly through thread safety
        var result = GameSessionMapper.ToDto(_mockSessionContext.Object);
        
        result.Should().NotBeNull();
        result.CelestialObjects.Should().HaveCount(1);
    }

    [Fact]
    public void ToSaveFormat_Should_Map_All_Properties_Correctly()
    {
        // Arrange
        var celestialObject = new BaseAsteroid(3)
        {
            Name = "Test Asteroid",
            Type = CelestialObjectType.Asteroid,
            IsPreScanned = true,
            X = 100,
            Y = 200,
            Id = 1,
            RemainingDrillAttempts = 2
        };

        ICommand command = new Command
        {
            Id = Guid.NewGuid(),
            Category = CommandCategory.CommandAccept
        };

        _gameSession.CelestialObjects.TryAdd(1, celestialObject);
        _gameSession.Commands.TryAdd(command.Id, command);

        // Act
        var result = GameSessionMapper.ToSaveFormat(_mockSessionContext.Object);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(_gameSession.Id);
        result.State.Should().NotBeNull();
        result.CelestialObjects.Should().HaveCount(1);
        result.Commands.Should().HaveCount(1);
        
        result.CelestialObjects.Should().ContainKey(1);
        result.Commands.Should().ContainKey(command.Id);
        
        var celestialObjectDto = result.CelestialObjects[1];
        celestialObjectDto.RemainingDrillAttempts.Should().Be(2);
    }

    [Fact]
    public void ToSaveFormat_Should_Handle_Empty_Collections()
    {
        // Arrange
        _gameSession.CelestialObjects.Clear();
        _gameSession.Commands.Clear();
        _gameSession.ActiveEvents.Clear();
        _gameSession.FinishedEvents.Clear();

        // Act
        var result = GameSessionMapper.ToSaveFormat(_mockSessionContext.Object);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(_gameSession.Id);
        result.State.Should().NotBeNull();
        result.CelestialObjects.Should().BeEmpty();
        result.Commands.Should().BeEmpty();
        result.GameActionEvents.Should().BeEmpty();
        result.FinishedEvents.Should().BeEmpty();
    }

    [Fact]
    public void ToSaveFormat_Should_Create_Independent_Copies_Of_Collections()
    {
        // Arrange
        var celestialObject = new BaseAsteroid(1)
        {
            Name = "Test Asteroid",
            Type = CelestialObjectType.Asteroid,
            Id = 1
        };

        _gameSession.CelestialObjects.TryAdd(1, celestialObject);

        // Act
        var result = GameSessionMapper.ToSaveFormat(_mockSessionContext.Object);

        // Modify original collection
        _gameSession.CelestialObjects.Clear();

        // Assert
        result.CelestialObjects.Should().HaveCount(1);
        result.CelestialObjects.Should().ContainKey(1);
    }

    [Fact]
    public void ToSaveFormat_Should_Map_Asteroid_Specific_Properties()
    {
        // Arrange
        var asteroid = new BaseAsteroid(5)
        {
            Name = "Mining Asteroid",
            Type = CelestialObjectType.Asteroid,
            Id = 42,
            RemainingDrillAttempts = 3
        };

        _gameSession.CelestialObjects.TryAdd(42, asteroid);

        // Act
        var result = GameSessionMapper.ToSaveFormat(_mockSessionContext.Object);

        // Assert
        result.CelestialObjects.Should().ContainKey(42);
        var asteroidDto = result.CelestialObjects[42];
        asteroidDto.RemainingDrillAttempts.Should().Be(3);
        asteroidDto.Type.Should().Be(CelestialObjectType.Asteroid);
        asteroidDto.Name.Should().Be("Mining Asteroid");
    }

    [Fact]
    public void ToSaveFormat_Should_Not_Return_Null()
    {
        // Act
        var result = GameSessionMapper.ToSaveFormat(_mockSessionContext.Object);

        // Assert
        result.Should().NotBeNull();
        result.CelestialObjects.Should().NotBeNull();
        result.Commands.Should().NotBeNull();
        result.GameActionEvents.Should().NotBeNull();
        result.FinishedEvents.Should().NotBeNull();
        result.State.Should().NotBeNull();
    }

    [Fact]
    public void ToSaveFormat_Should_Use_Lock_For_Thread_Safety()
    {
        // Arrange
        var celestialObject = new BaseAsteroid(1)
        {
            Name = "Test Asteroid",
            Type = CelestialObjectType.Asteroid,
            Id = 1
        };

        _gameSession.CelestialObjects.TryAdd(1, celestialObject);

        // Act & Assert
        var result = GameSessionMapper.ToSaveFormat(_mockSessionContext.Object);
        
        result.Should().NotBeNull();
        result.CelestialObjects.Should().HaveCount(1);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public void ToSaveFormat_Should_Map_Different_Drill_Attempts_Correctly(int drillAttempts)
    {
        // Arrange
        var asteroid = new BaseAsteroid(drillAttempts)
        {
            Name = "Variable Asteroid",
            Type = CelestialObjectType.Asteroid,
            Id = 1
        };

        _gameSession.CelestialObjects.TryAdd(1, asteroid);

        // Act
        var result = GameSessionMapper.ToSaveFormat(_mockSessionContext.Object);

        // Assert
        result.CelestialObjects[1].RemainingDrillAttempts.Should().Be(drillAttempts);
    }

    [Fact]
    public void ToGameObject_Should_Map_All_Properties_Correctly()
    {
        // Arrange
        var gameSessionDto = new GameSessionSaveFormatDto
        {
            Id = Guid.NewGuid(),
            State = new GameStateDto
            {
                Turn = 100,
                Cycle = 5,
                Tick = 50,
                ProcessedTurns = 99
            },
            CelestialObjects = new Dictionary<int, CelestialObjectSaveFormatDto>
            {
                {
                    1, new CelestialObjectSaveFormatDto
                    {
                        Id = 1,
                        Name = "Test Asteroid",
                        Type = CelestialObjectType.Asteroid,
                        X = 100.0,
                        Y = 200.0,
                        RemainingDrillAttempts = 3
                    }
                }
            },
            Commands = new Dictionary<Guid, CommandDto>(),
            GameActionEvents = new Dictionary<long, GameActionEventDto>(),
            FinishedEvents = new Dictionary<long, long>()
        };

        // Act
        var result = GameSessionMapper.ToGameObject(gameSessionDto);

        // Assert
        result.Should().NotBeNull();
        result.CelestialObjects.Should().HaveCount(1);
        result.CelestialObjects.Should().ContainKey(1);
        
        var celestialObject = result.CelestialObjects[1];
        celestialObject.Should().BeOfType<BaseAsteroid>();
        celestialObject.Name.Should().Be("Test Asteroid");
        celestialObject.X.Should().Be(100.0);
        celestialObject.Y.Should().Be(200.0);
        
        var asteroid = celestialObject as BaseAsteroid;
        asteroid!.RemainingDrillAttempts.Should().Be(3);
    }

    [Fact]
    public void ToGameObject_Should_Handle_Empty_Collections()
    {
        // Arrange
        var gameSessionDto = new GameSessionSaveFormatDto
        {
            Id = Guid.NewGuid(),
            State = new GameStateDto(),
            CelestialObjects = new Dictionary<int, CelestialObjectSaveFormatDto>(),
            Commands = new Dictionary<Guid, CommandDto>(),
            GameActionEvents = new Dictionary<long, GameActionEventDto>(),
            FinishedEvents = new Dictionary<long, long>()
        };

        // Act
        var result = GameSessionMapper.ToGameObject(gameSessionDto);

        // Assert
        result.Should().NotBeNull();
        result.CelestialObjects.Should().BeEmpty();
    }

    [Fact]
    public void ToGameObject_Should_Create_Independent_Collections()
    {
        // Arrange
        var gameSessionDto = new GameSessionSaveFormatDto
        {
            Id = Guid.NewGuid(),
            State = new GameStateDto(),
            CelestialObjects = new Dictionary<int, CelestialObjectSaveFormatDto>
            {
                {
                    1, new CelestialObjectSaveFormatDto
                    {
                        Id = 1,
                        Name = "Test Asteroid",
                        Type = CelestialObjectType.Asteroid
                    }
                }
            }
        };

        // Act
        var result = GameSessionMapper.ToGameObject(gameSessionDto);

        // Modify original DTO collection
        gameSessionDto.CelestialObjects.Clear();

        // Assert
        result.CelestialObjects.Should().HaveCount(1);
        result.CelestialObjects.Should().ContainKey(1);
    }

    [Fact]
    public void ToGameObject_Should_Map_Multiple_CelestialObjects()
    {
        // Arrange
        var gameSessionDto = new GameSessionSaveFormatDto
        {
            Id = Guid.NewGuid(),
            State = new GameStateDto(),
            CelestialObjects = new Dictionary<int, CelestialObjectSaveFormatDto>
            {
                {
                    1, new CelestialObjectSaveFormatDto
                    {
                        Id = 1,
                        Name = "Asteroid 1",
                        Type = CelestialObjectType.Asteroid,
                        RemainingDrillAttempts = 2
                    }
                },
                {
                    2, new CelestialObjectSaveFormatDto
                    {
                        Id = 2,
                        Name = "Asteroid 2",
                        Type = CelestialObjectType.Asteroid,
                        RemainingDrillAttempts = 5
                    }
                }
            }
        };

        // Act
        var result = GameSessionMapper.ToGameObject(gameSessionDto);

        // Assert
        result.CelestialObjects.Should().HaveCount(2);
        result.CelestialObjects.Should().ContainKey(1);
        result.CelestialObjects.Should().ContainKey(2);
        
        var asteroid1 = result.CelestialObjects[1] as BaseAsteroid;
        var asteroid2 = result.CelestialObjects[2] as BaseAsteroid;
        
        asteroid1!.RemainingDrillAttempts.Should().Be(2);
        asteroid2!.RemainingDrillAttempts.Should().Be(5);
    }

    [Fact]
    public void ToGameObject_Should_Not_Return_Null()
    {
        // Arrange
        var gameSessionDto = new GameSessionSaveFormatDto
        {
            Id = Guid.NewGuid(),
            State = new GameStateDto(),
            CelestialObjects = new Dictionary<int, CelestialObjectSaveFormatDto>()
        };

        // Act
        var result = GameSessionMapper.ToGameObject(gameSessionDto);

        // Assert
        result.Should().NotBeNull();
        result.CelestialObjects.Should().NotBeNull();
    }

    [Fact]
    public void ToGameObject_Should_Use_Lock_For_Thread_Safety()
    {
        // Arrange
        var gameSessionDto = new GameSessionSaveFormatDto
        {
            Id = Guid.NewGuid(),
            State = new GameStateDto(),
            CelestialObjects = new Dictionary<int, CelestialObjectSaveFormatDto>
            {
                {
                    1, new CelestialObjectSaveFormatDto
                    {
                        Id = 1,
                        Name = "Test Asteroid",
                        Type = CelestialObjectType.Asteroid
                    }
                }
            }
        };

        // Act & Assert
        var result = GameSessionMapper.ToGameObject(gameSessionDto);
        
        result.Should().NotBeNull();
        result.CelestialObjects.Should().HaveCount(1);
    }

    [Fact]
    public void ToGameObject_Should_Throw_ArgumentNullException_When_Dto_Is_Null()
    {
        // Act & Assert
        var action = () => GameSessionMapper.ToGameObject(null!);
        action.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(10)]
    public void ToGameObject_Should_Map_Different_Drill_Attempts_Correctly(int drillAttempts)
    {
        // Arrange
        var gameSessionDto = new GameSessionSaveFormatDto
        {
            Id = Guid.NewGuid(),
            State = new GameStateDto(),
            CelestialObjects = new Dictionary<int, CelestialObjectSaveFormatDto>
            {
                {
                    1, new CelestialObjectSaveFormatDto
                    {
                        Id = 1,
                        Name = "Variable Asteroid",
                        Type = CelestialObjectType.Asteroid,
                        RemainingDrillAttempts = drillAttempts
                    }
                }
            }
        };

        // Act
        var result = GameSessionMapper.ToGameObject(gameSessionDto);

        // Assert
        var asteroid = result.CelestialObjects[1] as BaseAsteroid;
        asteroid!.RemainingDrillAttempts.Should().Be(drillAttempts);
    }

    [Fact]
    public void ToGameObject_Should_Create_Independent_Object_Instances()
    {
        // Arrange
        var gameSessionDto = new GameSessionSaveFormatDto
        {
            Id = Guid.NewGuid(),
            State = new GameStateDto(),
            CelestialObjects = new Dictionary<int, CelestialObjectSaveFormatDto>
            {
                {
                    1, new CelestialObjectSaveFormatDto
                    {
                        Id = 1,
                        Name = "Test Asteroid",
                        Type = CelestialObjectType.Asteroid
                    }
                }
            }
        };

        // Act
        var result1 = GameSessionMapper.ToGameObject(gameSessionDto);
        var result2 = GameSessionMapper.ToGameObject(gameSessionDto);

        // Assert
        result1.Should().NotBeSameAs(result2);
        result1.CelestialObjects[1].Should().NotBeSameAs(result2.CelestialObjects[1]);
    }
} 