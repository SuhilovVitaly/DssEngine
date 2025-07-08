using DeepSpaceSaga.Common.Abstractions.Dto.Ui;
using DeepSpaceSaga.Common.Abstractions.Entities;
using DeepSpaceSaga.Common.Geometry;
using DeepSpaceSaga.UI.Controller.Tools;
using System.Reflection;

namespace DeepSpaceSaga.UI.Controller.Tests.Services;

public class OuterSpaceServiceTests
{
    private readonly OuterSpaceService _outerSpaceService;
    private readonly GameSessionDto _gameSessionDto;
    private readonly CelestialObjectDto _playerSpaceship;
    private readonly CelestialObjectDto _asteroid1;
    private readonly CelestialObjectDto _asteroid2;
    private readonly SpaceMapPoint _testCoordinates;

    public OuterSpaceServiceTests()
    {
        _outerSpaceService = new OuterSpaceService();
        
        _playerSpaceship = new CelestialObjectDto
        {
            Id = 1,
            Name = "Player Ship",
            Type = CelestialObjectType.SpaceshipPlayer,
            X = 50,
            Y = 50
        };

        _asteroid1 = new CelestialObjectDto
        {
            Id = 2,
            Name = "Asteroid 1",
            Type = CelestialObjectType.Asteroid,
            X = 110,
            Y = 110
        };

        _asteroid2 = new CelestialObjectDto
        {
            Id = 3,
            Name = "Asteroid 2", 
            Type = CelestialObjectType.Asteroid,
            X = 120,
            Y = 120
        };

        _gameSessionDto = new GameSessionDto
        {
            CelestialObjects = new Dictionary<int, CelestialObjectDto>
            {
                { 1, _playerSpaceship },
                { 2, _asteroid1 },
                { 3, _asteroid2 }
            }
        };

        _testCoordinates = new SpaceMapPoint(115, 115);
    }

    [Fact]
    public void Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var service = new OuterSpaceService();

        // Assert
        Assert.Equal(0, service.ActiveObjectId);
        Assert.Equal(0, service.SelectedObjectId);
    }

    [Fact]
    public void CleanActiveObject_ShouldSetActiveObjectIdToZero()
    {
        // Arrange
        _outerSpaceService.HandleMouseMove(_gameSessionDto, _testCoordinates, _testCoordinates);
        Assert.NotEqual(0, _outerSpaceService.ActiveObjectId);

        // Act
        _outerSpaceService.CleanActiveObject();

        // Assert
        Assert.Equal(0, _outerSpaceService.ActiveObjectId);
    }

    [Fact]
    public void CleanSelectedObject_ShouldSetSelectedObjectIdToZero()
    {
        // Arrange
        _outerSpaceService.HandleMouseClick(_gameSessionDto, _testCoordinates);
        Assert.NotEqual(0, _outerSpaceService.SelectedObjectId);

        // Act
        _outerSpaceService.CleanSelectedObject();

        // Assert
        Assert.Equal(0, _outerSpaceService.SelectedObjectId);
    }

    [Fact]
    public void HandleMouseMove_ShouldSetActiveObjectAndTriggerEvent_WhenObjectFound()
    {
        // Arrange
        CelestialObjectDto? eventObject = null;
        _outerSpaceService.OnShowCelestialObject += obj => eventObject = obj;

        // Act
        _outerSpaceService.HandleMouseMove(_gameSessionDto, _testCoordinates, _testCoordinates);

        // Assert
        Assert.Equal(_asteroid1.Id, _outerSpaceService.ActiveObjectId);
        Assert.NotNull(eventObject);
        Assert.Equal(_asteroid1.Id, eventObject.Id);
    }

    [Fact]
    public void HandleMouseMove_ShouldTriggerHideEvent_WhenNoObjectFoundAndActiveObjectExists()
    {
        // Arrange
        _outerSpaceService.HandleMouseMove(_gameSessionDto, _testCoordinates, _testCoordinates);
        Assert.NotEqual(0, _outerSpaceService.ActiveObjectId);

        CelestialObjectDto? hideEventObject = null;
        _outerSpaceService.OnHideCelestialObject += obj => hideEventObject = obj;

        var farCoordinates = new SpaceMapPoint(500, 500);

        // Act
        _outerSpaceService.HandleMouseMove(_gameSessionDto, farCoordinates, farCoordinates);

        // Assert
        Assert.Equal(0, _outerSpaceService.ActiveObjectId);
        Assert.Null(hideEventObject);
    }

    [Fact]
    public void HandleMouseMove_ShouldNotTriggerEvent_WhenNoObjectFoundAndNoActiveObject()
    {
        // Arrange
        var eventTriggered = false;
        _outerSpaceService.OnHideCelestialObject += obj => eventTriggered = true;

        var farCoordinates = new SpaceMapPoint(500, 500);

        // Act
        _outerSpaceService.HandleMouseMove(_gameSessionDto, farCoordinates, farCoordinates);

        // Assert
        Assert.Equal(0, _outerSpaceService.ActiveObjectId);
        Assert.False(eventTriggered);
    }

    [Fact]
    public void HandleMouseMove_ShouldNotTriggerEvent_WhenSameObjectFound()
    {
        // Arrange
        _outerSpaceService.HandleMouseMove(_gameSessionDto, _testCoordinates, _testCoordinates);
        var initialActiveId = _outerSpaceService.ActiveObjectId;

        var eventTriggered = false;
        _outerSpaceService.OnShowCelestialObject += obj => eventTriggered = true;

        // Act
        _outerSpaceService.HandleMouseMove(_gameSessionDto, _testCoordinates, _testCoordinates);

        // Assert
        Assert.Equal(initialActiveId, _outerSpaceService.ActiveObjectId);
        Assert.False(eventTriggered);
    }

    [Fact]
    public void HandleMouseMove_ShouldExcludePlayerSpaceship()
    {
        // Arrange
        var playerCoordinates = new SpaceMapPoint(50, 50);
        
        // Act
        _outerSpaceService.HandleMouseMove(_gameSessionDto, playerCoordinates, playerCoordinates);

        // Assert
        // Player spaceship should be excluded from selection, so ActiveObjectId should remain 0
        Assert.Equal(0, _outerSpaceService.ActiveObjectId);
    }

    [Fact]
    public void HandleMouseMove_ShouldSelectClosestObject_WhenMultipleObjectsInRange()
    {
        // Arrange
        var coordinates = new SpaceMapPoint(112, 112);

        // Act
        _outerSpaceService.HandleMouseMove(_gameSessionDto, coordinates, coordinates);

        // Assert
        Assert.Equal(_asteroid1.Id, _outerSpaceService.ActiveObjectId);
    }

    [Fact]
    public void HandleMouseClick_ShouldSetSelectedObjectAndTriggerEvent_WhenObjectFound()
    {
        // Arrange
        CelestialObjectDto? eventObject = null;
        _outerSpaceService.OnSelectCelestialObject += obj => eventObject = obj;

        // Act
        _outerSpaceService.HandleMouseClick(_gameSessionDto, _testCoordinates);

        // Assert
        Assert.Equal(_asteroid1.Id, _outerSpaceService.SelectedObjectId);
        Assert.NotNull(eventObject);
        Assert.Equal(_asteroid1.Id, eventObject.Id);
    }

    [Fact]
    public void HandleMouseClick_ShouldNotChangeSelectedObject_WhenNoObjectFound()
    {
        // Arrange
        var initialSelectedId = _outerSpaceService.SelectedObjectId;
        var farCoordinates = new SpaceMapPoint(500, 500);

        var eventTriggered = false;
        _outerSpaceService.OnSelectCelestialObject += obj => eventTriggered = true;

        // Act
        _outerSpaceService.HandleMouseClick(_gameSessionDto, farCoordinates);

        // Assert
        Assert.Equal(initialSelectedId, _outerSpaceService.SelectedObjectId);
        Assert.False(eventTriggered);
    }

    [Fact]
    public void HandleMouseClick_ShouldExcludePlayerSpaceship()
    {
        // Arrange
        var playerCoordinates = new SpaceMapPoint(50, 50);
        var eventTriggered = false;
        _outerSpaceService.OnSelectCelestialObject += obj => eventTriggered = true;

        // Act
        _outerSpaceService.HandleMouseClick(_gameSessionDto, playerCoordinates);

        // Assert
        Assert.Equal(0, _outerSpaceService.SelectedObjectId);
        Assert.False(eventTriggered);
    }

    [Fact]
    public void HandleMouseClick_ShouldSelectClosestObject_WhenMultipleObjectsInRange()
    {
        // Arrange
        var coordinates = new SpaceMapPoint(112, 112);

        // Act
        _outerSpaceService.HandleMouseClick(_gameSessionDto, coordinates);

        // Assert
        Assert.Equal(_asteroid1.Id, _outerSpaceService.SelectedObjectId);
    }

    [Fact]
    public void Events_ShouldNotThrow_WhenNoSubscribers()
    {
        // Arrange & Act & Assert
        var exception1 = Record.Exception(() => _outerSpaceService.HandleMouseMove(_gameSessionDto, _testCoordinates, _testCoordinates));
        var exception2 = Record.Exception(() => _outerSpaceService.HandleMouseClick(_gameSessionDto, _testCoordinates));

        Assert.Null(exception1);
        Assert.Null(exception2);
    }

    [Fact]
    public void Events_ShouldSupportMultipleSubscribers()
    {
        // Arrange
        var showEventCount = 0;
        var selectEventCount = 0;

        _outerSpaceService.OnShowCelestialObject += obj => showEventCount++;
        _outerSpaceService.OnShowCelestialObject += obj => showEventCount++;
        _outerSpaceService.OnSelectCelestialObject += obj => selectEventCount++;
        _outerSpaceService.OnSelectCelestialObject += obj => selectEventCount++;

        // Act
        _outerSpaceService.HandleMouseMove(_gameSessionDto, _testCoordinates, _testCoordinates);
        _outerSpaceService.HandleMouseClick(_gameSessionDto, _testCoordinates);

        // Assert
        Assert.Equal(2, showEventCount);
        Assert.Equal(2, selectEventCount);
    }

    [Fact]
    public void HandleMouseMove_ShouldUpdateActiveObject_WhenDifferentObjectFound()
    {
        // Arrange
        _outerSpaceService.HandleMouseMove(_gameSessionDto, _testCoordinates, _testCoordinates);
        var initialActiveId = _outerSpaceService.ActiveObjectId;

        var showEventCount = 0;
        _outerSpaceService.OnShowCelestialObject += obj => showEventCount++;

        var coordinates2 = new SpaceMapPoint(120, 120);

        // Act
        _outerSpaceService.HandleMouseMove(_gameSessionDto, coordinates2, coordinates2);

        // Assert
        Assert.NotEqual(initialActiveId, _outerSpaceService.ActiveObjectId);
        Assert.Equal(_asteroid2.Id, _outerSpaceService.ActiveObjectId);
        Assert.Equal(1, showEventCount);
    }

    [Fact]
    public void Properties_ShouldBeReadOnly()
    {
        // Arrange
        var type = typeof(OuterSpaceService);
        var activeObjectIdProperty = type.GetProperty(nameof(OuterSpaceService.ActiveObjectId));
        var selectedObjectIdProperty = type.GetProperty(nameof(OuterSpaceService.SelectedObjectId));

        // Assert
        Assert.NotNull(activeObjectIdProperty);
        Assert.NotNull(selectedObjectIdProperty);
        Assert.True(activeObjectIdProperty.CanRead);
        Assert.True(selectedObjectIdProperty.CanRead);
        // Properties should not have public setters
        Assert.True(activeObjectIdProperty.SetMethod == null || !activeObjectIdProperty.SetMethod.IsPublic);
        Assert.True(selectedObjectIdProperty.SetMethod == null || !selectedObjectIdProperty.SetMethod.IsPublic);
    }

    [Fact]
    public void HandleMouseMove_WithEmptyGameSession_ShouldNotThrow()
    {
        // Arrange
        var emptyGameSession = new GameSessionDto
        {
            CelestialObjects = new Dictionary<int, CelestialObjectDto>()
        };

        // Act & Assert
        var exception = Record.Exception(() => _outerSpaceService.HandleMouseMove(emptyGameSession, _testCoordinates, _testCoordinates));
        Assert.Null(exception);
        Assert.Equal(0, _outerSpaceService.ActiveObjectId);
    }

    [Fact]
    public void HandleMouseClick_WithEmptyGameSession_ShouldNotThrow()
    {
        // Arrange
        var emptyGameSession = new GameSessionDto
        {
            CelestialObjects = new Dictionary<int, CelestialObjectDto>()
        };

        // Act & Assert
        var exception = Record.Exception(() => _outerSpaceService.HandleMouseClick(emptyGameSession, _testCoordinates));
        Assert.Null(exception);
        Assert.Equal(0, _outerSpaceService.SelectedObjectId);
    }
} 