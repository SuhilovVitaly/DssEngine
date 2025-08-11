using DeepSpaceSaga.Common.Extensions.Entities.CelestialObjects;
using DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects.Spacecrafts;

namespace DeepSpaceSaga.Common.Tests.Extensions;

public class CelestialObjectExtensionsTests
{
    #region GetLocation Tests

    [Fact]
    public void GetLocation_Should_Return_SpaceMapPoint_With_Correct_Coordinates()
    {
        // Arrange
        var asteroid = new BaseAsteroid(5)
        {
            X = 123.45,
            Y = 678.90
        };

        // Act
        var result = asteroid.GetLocation();

        // Assert
        result.Should().NotBeNull();
        result.X.Should().Be(123.45f);
        result.Y.Should().Be(678.90f);
    }

    [Fact]
    public void GetLocation_Should_Handle_Zero_Coordinates()
    {
        // Arrange
        var asteroid = new BaseAsteroid(3)
        {
            X = 0.0,
            Y = 0.0
        };

        // Act
        var result = asteroid.GetLocation();

        // Assert
        result.X.Should().Be(0f);
        result.Y.Should().Be(0f);
    }

    [Fact]
    public void GetLocation_Should_Handle_Negative_Coordinates()
    {
        // Arrange
        var spaceship = new BaseSpaceship()
        {
            X = -100.25,
            Y = -200.75
        };

        // Act
        var result = spaceship.GetLocation();

        // Assert
        result.X.Should().Be(-100.25f);
        result.Y.Should().Be(-200.75f);
    }

    [Fact]
    public void GetLocation_Should_Handle_Large_Coordinates()
    {
        // Arrange
        var asteroid = new BaseAsteroid(1)
        {
            X = 999999.999,
            Y = -999999.999
        };

        // Act
        var result = asteroid.GetLocation();

        // Assert
        result.X.Should().Be(999999.999f);
        result.Y.Should().Be(-999999.999f);
    }

    [Theory]
    [InlineData(12.34, 56.78)]
    [InlineData(0.0, 0.0)]
    [InlineData(-1.0, -2.0)]
    [InlineData(1000.0, 2000.0)]
    [InlineData(double.MaxValue, double.MinValue)]
    public void GetLocation_Should_Convert_Double_To_Float_Correctly(double x, double y)
    {
        // Arrange
        var celestialObject = new BaseAsteroid(1)
        {
            X = x,
            Y = y
        };

        // Act
        var result = celestialObject.GetLocation();

        // Assert
        result.X.Should().Be((float)x);
        result.Y.Should().Be((float)y);
    }

    #endregion

    #region ToSpaceship Tests

    [Fact]
    public void ToSpaceship_Should_Return_ISpacecraft_When_Object_Is_Spaceship()
    {
        // Arrange
        ICelestialObject spaceship = new BaseSpaceship()
        {
            Id = 1,
            Name = "Test Ship",
            X = 100.0,
            Y = 200.0,
            Type = CelestialObjectType.SpaceshipPlayer
        };

        // Act
        var result = spaceship.ToSpaceship();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<BaseSpaceship>();
        result.Should().BeAssignableTo<ISpacecraft>();
        result.Id.Should().Be(1);
        result.Name.Should().Be("Test Ship");
    }

    [Fact]
    public void ToSpaceship_Should_Return_Null_When_Object_Is_Not_Spaceship()
    {
        // Arrange
        ICelestialObject asteroid = new BaseAsteroid(5)
        {
            Id = 2,
            Name = "Test Asteroid",
            X = 300.0,
            Y = 400.0,
            Type = CelestialObjectType.Asteroid
        };

        // Act
        var result = asteroid.ToSpaceship();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void ToSpaceship_Should_Preserve_Spaceship_Properties()
    {
        // Arrange
        var originalSpaceship = new BaseSpaceship()
        {
            Id = 123,
            Name = "USS Enterprise",
            X = 1000.0,
            Y = 2000.0,
            MaxSpeed = 250.5f,
            Agility = 85.2f,
            Direction = 45.0,
            Speed = 100.0,
            Type = CelestialObjectType.SpaceshipPlayer
        };

        ICelestialObject celestialObject = originalSpaceship;

        // Act
        var result = celestialObject.ToSpaceship();

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(123);
        result.Name.Should().Be("USS Enterprise");
        result.X.Should().Be(1000.0);
        result.Y.Should().Be(2000.0);
        result.MaxSpeed.Should().Be(250.5f);
        result.Agility.Should().Be(85.2f);
        result.Direction.Should().Be(45.0);
        result.Speed.Should().Be(100.0);
        result.Type.Should().Be(CelestialObjectType.SpaceshipPlayer);
    }

    [Fact]
    public void ToSpaceship_Should_Allow_Access_To_Spaceship_Specific_Methods()
    {
        // Arrange
        ICelestialObject spaceship = new BaseSpaceship()
        {
            MaxSpeed = 200f
        };

        // Act
        var result = spaceship.ToSpaceship();

        // Assert
        result.Should().NotBeNull();
        
        // Should be able to call spaceship-specific methods
        result.Should().BeAssignableTo<ISpacecraft>();
        
        // Test spaceship-specific functionality
        var initialSpeed = result.Speed;
        result.ChangeVelocity(50.0);
        result.Speed.Should().Be(initialSpeed + 50.0);
    }

    [Theory]
    [InlineData(CelestialObjectType.SpaceshipPlayer)]
    [InlineData(CelestialObjectType.SpaceshipNpcNeutral)]
    [InlineData(CelestialObjectType.SpaceshipNpcEnemy)]
    [InlineData(CelestialObjectType.SpaceshipNpcFriend)]
    public void ToSpaceship_Should_Work_For_All_Spaceship_Types(CelestialObjectType spaceshipType)
    {
        // Arrange
        ICelestialObject spaceship = new BaseSpaceship()
        {
            Type = spaceshipType,
            Name = $"Test Ship {spaceshipType}"
        };

        // Act
        var result = spaceship.ToSpaceship();

        // Assert
        result.Should().NotBeNull();
        result.Type.Should().Be(spaceshipType);
        result.Name.Should().Be($"Test Ship {spaceshipType}");
    }

    [Theory]
    [InlineData(CelestialObjectType.Asteroid)]
    [InlineData(CelestialObjectType.Station)]
    [InlineData(CelestialObjectType.Missile)]
    [InlineData(CelestialObjectType.Explosion)]
    [InlineData(CelestialObjectType.Container)]
    [InlineData(CelestialObjectType.Unknown)]
    [InlineData(CelestialObjectType.PointInMap)]
    public void ToSpaceship_Should_Return_Null_For_Non_Spaceship_Types(CelestialObjectType nonSpaceshipType)
    {
        // Arrange
        ICelestialObject nonSpaceship = new BaseAsteroid(1)
        {
            Type = nonSpaceshipType,
            Name = $"Test Object {nonSpaceshipType}"
        };

        // Act
        var result = nonSpaceship.ToSpaceship();

        // Assert
        result.Should().BeNull();
    }

    #endregion

    #region Integration Tests

    [Fact]
    public void Extensions_Should_Work_Together_For_Spaceship()
    {
        // Arrange
        ICelestialObject spaceship = new BaseSpaceship()
        {
            X = 12.34,
            Y = 56.78,
            Type = CelestialObjectType.SpaceshipPlayer
        };

        // Act
        var location = spaceship.GetLocation();
        var asSpaceship = spaceship.ToSpaceship();

        // Assert
        location.X.Should().Be(12.34f);
        location.Y.Should().Be(56.78f);
        asSpaceship.Should().NotBeNull();
        asSpaceship.X.Should().Be(12.34);
        asSpaceship.Y.Should().Be(56.78);
    }

    [Fact]
    public void Extensions_Should_Work_For_Asteroid()
    {
        // Arrange
        ICelestialObject asteroid = new BaseAsteroid(3)
        {
            X = 98.76,
            Y = 54.32,
            Type = CelestialObjectType.Asteroid
        };

        // Act
        var location = asteroid.GetLocation();
        var asSpaceship = asteroid.ToSpaceship();

        // Assert
        location.X.Should().Be(98.76f);
        location.Y.Should().Be(54.32f);
        asSpaceship.Should().BeNull();
    }

    [Fact]
    public void GetLocation_And_ToSpaceship_Should_Be_Null_Safe()
    {
        // Arrange
        ICelestialObject? nullObject = null;

        // Act & Assert
        Action getLocationAction = () => nullObject!.GetLocation();
        
        // GetLocation should throw on null input
        getLocationAction.Should().Throw<NullReferenceException>();
        
        // ToSpaceship should return null for null input
        var result = nullObject.ToSpaceship();
        result.Should().BeNull();
    }

    #endregion
}
