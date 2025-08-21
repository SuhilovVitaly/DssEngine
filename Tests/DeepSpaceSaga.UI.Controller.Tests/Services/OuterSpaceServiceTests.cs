using DeepSpaceSaga.Common.Abstractions.Dto.Ui;
using DeepSpaceSaga.Common.Abstractions.Entities;
using DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects;
using DeepSpaceSaga.Common.Geometry;
using DeepSpaceSaga.UI.Controller.Services;

namespace DeepSpaceSaga.UI.Controller.Tests.Services;

public class OuterSpaceServiceTests
{
    private readonly OuterSpaceService _sut;
    private readonly CelestialObjectSaveFormatDto _playerSpaceship;
    private readonly CelestialObjectSaveFormatDto _asteroid1;
    private readonly CelestialObjectSaveFormatDto _asteroid2;

    public OuterSpaceServiceTests()
    {
        _sut = new OuterSpaceService();
        
        _playerSpaceship = new CelestialObjectSaveFormatDto
        {
            Id = 1,
            Name = "Player Ship",
            Type = CelestialObjectType.SpaceshipPlayer,
            X = 100,
            Y = 100
        };

        _asteroid1 = new CelestialObjectSaveFormatDto
        {
            Id = 2,
            Name = "Asteroid 1",
            Type = CelestialObjectType.Asteroid,
            X = 120,
            Y = 100
        };

        _asteroid2 = new CelestialObjectSaveFormatDto
        {
            Id = 3,
            Name = "Asteroid 2",
            Type = CelestialObjectType.Asteroid,
            X = 140,
            Y = 100
        };
    }

    [Fact]
    public void Constructor_Should_Initialize_Service()
    {
        // Assert
        Assert.NotNull(_sut);
    }

    [Fact]
    public void HandleMouseMove_Should_Update_ActiveObjectId()
    {
        // Arrange
        var gameSession = new GameSessionDto
        {
            CelestialObjects = new Dictionary<int, CelestialObjectSaveFormatDto>
            {
                { _playerSpaceship.Id, _playerSpaceship },
                { _asteroid1.Id, _asteroid1 }
            }
        };

        var coordinates = new SpaceMapPoint(120, 100);
        var screenCoordinates = new SpaceMapPoint(120, 100);

        // Act
        _sut.HandleMouseMove(gameSession, coordinates, screenCoordinates);

        // Assert
        Assert.Equal(_asteroid1.Id, _sut.ActiveObjectId);
    }

    [Fact]
    public void HandleMouseClick_Should_Update_SelectedObjectId()
    {
        // Arrange
        var gameSession = new GameSessionDto
        {
            CelestialObjects = new Dictionary<int, CelestialObjectSaveFormatDto>
            {
                { _playerSpaceship.Id, _playerSpaceship },
                { _asteroid1.Id, _asteroid1 }
            }
        };

        var coordinates = new SpaceMapPoint(120, 100);

        // Act
        _sut.HandleMouseClick(gameSession, coordinates);

        // Assert
        Assert.Equal(_asteroid1.Id, _sut.SelectedObjectId);
    }

    [Fact]
    public void CleanActiveObject_Should_Reset_ActiveObjectId()
    {
        // Arrange
        // Use reflection to set private property for testing
        var property = typeof(OuterSpaceService).GetProperty("ActiveObjectId");
        property?.SetValue(_sut, 123);

        // Act
        _sut.CleanActiveObject();

        // Assert
        Assert.Equal(0, _sut.ActiveObjectId);
    }

    [Fact]
    public void CleanSelectedObject_Should_Reset_SelectedObjectId()
    {
        // Arrange
        // Use reflection to set private property for testing
        var property = typeof(OuterSpaceService).GetProperty("SelectedObjectId");
        property?.SetValue(_sut, 456);

        // Act
        _sut.CleanSelectedObject();

        // Assert
        Assert.Equal(0, _sut.SelectedObjectId);
    }

    [Fact]
    public void HandleMouseMove_Should_Not_Update_ActiveObjectId_When_Same_Object()
    {
        // Arrange
        var gameSession = new GameSessionDto
        {
            CelestialObjects = new Dictionary<int, CelestialObjectSaveFormatDto>
            {
                { _playerSpaceship.Id, _playerSpaceship },
                { _asteroid1.Id, _asteroid1 }
            }
        };

        var coordinates = new SpaceMapPoint(120, 100);
        var screenCoordinates = new SpaceMapPoint(120, 100);

        // Act - First call
        _sut.HandleMouseMove(gameSession, coordinates, screenCoordinates);
        var firstActiveId = _sut.ActiveObjectId;

        // Act - Second call with same object
        _sut.HandleMouseMove(gameSession, coordinates, screenCoordinates);
        var secondActiveId = _sut.ActiveObjectId;

        // Assert
        Assert.Equal(firstActiveId, secondActiveId);
    }

    [Fact]
    public void HandleMouseMove_Should_Reset_ActiveObjectId_When_Leaving_Object_Range()
    {
        // Arrange
        var gameSession = new GameSessionDto
        {
            CelestialObjects = new Dictionary<int, CelestialObjectSaveFormatDto>
            {
                { _playerSpaceship.Id, _playerSpaceship },
                { _asteroid1.Id, _asteroid1 }
            }
        };

        // First move to object
        var objectCoordinates = new SpaceMapPoint(120, 100);
        var screenCoordinates = new SpaceMapPoint(120, 100);
        _sut.HandleMouseMove(gameSession, objectCoordinates, screenCoordinates);

        // Then move away
        var awayCoordinates = new SpaceMapPoint(200, 200);

        // Act
        _sut.HandleMouseMove(gameSession, awayCoordinates, screenCoordinates);

        // Assert
        Assert.Equal(0, _sut.ActiveObjectId);
    }

    [Fact]
    public void HandleMouseMove_Should_Handle_Empty_CelestialObjects()
    {
        // Arrange
        var gameSession = new GameSessionDto
        {
            CelestialObjects = new Dictionary<int, CelestialObjectSaveFormatDto>()
        };

        var coordinates = new SpaceMapPoint(100, 100);
        var screenCoordinates = new SpaceMapPoint(100, 100);

        // Act
        _sut.HandleMouseMove(gameSession, coordinates, screenCoordinates);

        // Assert
        Assert.Equal(0, _sut.ActiveObjectId);
    }

    [Fact]
    public void HandleMouseClick_Should_Handle_Empty_CelestialObjects()
    {
        // Arrange
        var gameSession = new GameSessionDto
        {
            CelestialObjects = new Dictionary<int, CelestialObjectSaveFormatDto>()
        };

        var coordinates = new SpaceMapPoint(100, 100);

        // Act
        _sut.HandleMouseClick(gameSession, coordinates);

        // Assert
        Assert.Equal(0, _sut.SelectedObjectId);
    }
} 