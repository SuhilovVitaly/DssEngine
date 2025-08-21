using DeepSpaceSaga.Common.Abstractions.Dto.Ui;
using DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects.Asteroids;
using DeepSpaceSaga.Common.Abstractions.Entities;
using FluentAssertions;

namespace DeepSpaceSaga.Common.Tests.Entities.CelestialObjects.Asteroids;

public class BaseAsteroidTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(100)]
    public void Constructor_With_MaxDrillAttempts_Should_Set_RemainingDrillAttempts(int maxDrillAttempts)
    {
        // Act
        var asteroid = new BaseAsteroid(maxDrillAttempts);

        // Assert
        asteroid.RemainingDrillAttempts.Should().Be(maxDrillAttempts);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-10)]
    [InlineData(int.MinValue)]
    public void Constructor_With_Negative_MaxDrillAttempts_Should_Set_Negative_RemainingDrillAttempts(int maxDrillAttempts)
    {
        // Act
        var asteroid = new BaseAsteroid(maxDrillAttempts);

        // Assert
        asteroid.RemainingDrillAttempts.Should().Be(maxDrillAttempts);
    }

    [Fact]
    public void Constructor_With_Dto_Should_Load_Object_Properties()
    {
        // Arrange
        var dto = new CelestialObjectDto
        {
            Id = 456,
            OwnerId = 0,
            Name = "Test Asteroid",
            Direction = 180.0,
            Speed = 5.0,
            X = 2000.0,
            Y = 3000.0,
            Type = CelestialObjectType.Asteroid,
            IsPreScanned = false,
            Size = 100.0f,
            RemainingDrillAttempts = 3
        };

        // Act
        var asteroid = new BaseAsteroid(dto);

        // Assert
        asteroid.Id.Should().Be(456);
        asteroid.OwnerId.Should().Be(0);
        asteroid.Name.Should().Be("Test Asteroid");
        asteroid.Direction.Should().Be(180.0);
        asteroid.Speed.Should().Be(5.0);
        asteroid.X.Should().Be(2000.0);
        asteroid.Y.Should().Be(3000.0);
        asteroid.Type.Should().Be(CelestialObjectType.Asteroid);
        asteroid.IsPreScanned.Should().BeFalse();
        asteroid.Size.Should().Be(100.0f);
        asteroid.RemainingDrillAttempts.Should().Be(3);
    }

    [Fact]
    public void Constructor_With_Dto_Should_Handle_Zero_RemainingDrillAttempts()
    {
        // Arrange
        var dto = new CelestialObjectDto
        {
            Name = "Zero Drill Asteroid",
            RemainingDrillAttempts = 0
        };

        // Act
        var asteroid = new BaseAsteroid(dto);

        // Assert
        asteroid.RemainingDrillAttempts.Should().Be(0);
    }

    [Fact]
    public void Constructor_With_Dto_Should_Handle_Negative_RemainingDrillAttempts()
    {
        // Arrange
        var dto = new CelestialObjectDto
        {
            Name = "Negative Drill Asteroid",
            RemainingDrillAttempts = -5
        };

        // Act
        var asteroid = new BaseAsteroid(dto);

        // Assert
        asteroid.RemainingDrillAttempts.Should().Be(-5);
    }

    [Theory]
    [InlineData(5, 4)]
    [InlineData(1, 0)]
    [InlineData(0, -1)]
    [InlineData(-1, -2)]
    [InlineData(100, 99)]
    public void Drill_Should_Decrease_RemainingDrillAttempts_By_One(int initialAttempts, int expectedAttempts)
    {
        // Arrange
        var asteroid = new BaseAsteroid(initialAttempts);

        // Act
        asteroid.Drill();

        // Assert
        asteroid.RemainingDrillAttempts.Should().Be(expectedAttempts);
    }

    [Fact]
    public void Drill_Should_Be_Callable_Multiple_Times()
    {
        // Arrange
        var asteroid = new BaseAsteroid(5);

        // Act
        asteroid.Drill();
        asteroid.Drill();
        asteroid.Drill();

        // Assert
        asteroid.RemainingDrillAttempts.Should().Be(2);
    }

    [Fact]
    public void Drill_Should_Continue_Decreasing_Below_Zero()
    {
        // Arrange
        var asteroid = new BaseAsteroid(1);

        // Act
        asteroid.Drill();
        asteroid.Drill();
        asteroid.Drill();

        // Assert
        asteroid.RemainingDrillAttempts.Should().Be(-2);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(-1)]
    public void RemainingDrillAttempts_Should_Set_And_Get_Correctly(int attempts)
    {
        // Arrange
        var asteroid = new BaseAsteroid(0);

        // Act
        asteroid.RemainingDrillAttempts = attempts;

        // Assert
        asteroid.RemainingDrillAttempts.Should().Be(attempts);
    }

    [Fact]
    public void BaseAsteroid_Should_Implement_IAsteroid_Interface()
    {
        // Arrange & Act
        var asteroid = new BaseAsteroid(5);

        // Assert
        asteroid.Should().BeAssignableTo<IAsteroid>();
    }

    [Fact]
    public void BaseAsteroid_Should_Inherit_From_BaseCelestialObject()
    {
        // Arrange & Act
        var asteroid = new BaseAsteroid(5);

        // Assert
        asteroid.Should().BeAssignableTo<BaseCelestialObject>();
    }

    [Fact]
    public void BaseAsteroid_Should_Have_Default_BaseCelestialObject_Properties()
    {
        // Arrange & Act
        var asteroid = new BaseAsteroid(5);

        // Assert
        asteroid.Id.Should().Be(0);
        asteroid.OwnerId.Should().Be(0);
        asteroid.Name.Should().BeNull();
        asteroid.Direction.Should().Be(0.0);
        asteroid.Speed.Should().Be(0.0);
        asteroid.X.Should().Be(0.0);
        asteroid.Y.Should().Be(0.0);
        asteroid.Type.Should().Be(CelestialObjectType.Unknown);
        asteroid.IsPreScanned.Should().BeFalse();
        asteroid.Size.Should().Be(0.0f);
    }

    [Fact]
    public void BaseAsteroid_Should_Allow_Setting_BaseCelestialObject_Properties()
    {
        // Arrange
        var asteroid = new BaseAsteroid(5);

        // Act
        asteroid.Id = 123;
        asteroid.OwnerId = 456;
        asteroid.Name = "Test Asteroid";
        asteroid.Direction = 90.0;
        asteroid.Speed = 10.0;
        asteroid.X = 1000.0;
        asteroid.Y = 2000.0;
        asteroid.Type = CelestialObjectType.Asteroid;
        asteroid.IsPreScanned = true;
        asteroid.Size = 50.0f;

        // Assert
        asteroid.Id.Should().Be(123);
        asteroid.OwnerId.Should().Be(456);
        asteroid.Name.Should().Be("Test Asteroid");
        asteroid.Direction.Should().Be(90.0);
        asteroid.Speed.Should().Be(10.0);
        asteroid.X.Should().Be(1000.0);
        asteroid.Y.Should().Be(2000.0);
        asteroid.Type.Should().Be(CelestialObjectType.Asteroid);
        asteroid.IsPreScanned.Should().BeTrue();
        asteroid.Size.Should().Be(50.0f);
    }
} 