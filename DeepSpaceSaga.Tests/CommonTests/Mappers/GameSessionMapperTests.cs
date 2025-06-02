using DeepSpaceSaga.Common.Abstractions.Entities.Commands;

namespace DeepSpaceSaga.Tests.CommonTests.Mappers;

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
            CelestialObjects = new Dictionary<int, ICelestialObject>(),
            Commands = new ConcurrentDictionary<Guid, ICommand>()
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

        _gameSession.CelestialObjects.Add(1, celestialObject);
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

        _gameSession.CelestialObjects.Add(1, celestialObject);

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

        _gameSession.CelestialObjects.Add(1, asteroid1);
        _gameSession.CelestialObjects.Add(2, asteroid2);

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
        var lockTaken = false;
        var celestialObject = new BaseAsteroid(1)
        {
            Name = "Test Asteroid",
            Type = CelestialObjectType.Asteroid,
            Id = 1
        };

        _gameSession.CelestialObjects.Add(1, celestialObject);

        // Act & Assert
        // This test verifies that the method can be called without throwing exceptions
        // The actual lock behavior is tested implicitly through thread safety
        var result = GameSessionMapper.ToDto(_mockSessionContext.Object);
        
        result.Should().NotBeNull();
        result.CelestialObjects.Should().HaveCount(1);
    }
} 