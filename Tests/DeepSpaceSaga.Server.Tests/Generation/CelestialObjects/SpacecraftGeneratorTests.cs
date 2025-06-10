using DeepSpaceSaga.Common.Abstractions.Entities;
using DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects;
using DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects.Spacecrafts;
using DeepSpaceSaga.Server.Generation.CelestialObjects;
using FluentAssertions;

namespace DeepSpaceSaga.Server.Tests.Generation.CelestialObjects;

public class SpacecraftGeneratorTests
{
    [Fact]
    public void BuildPlayerSpacecraft_Should_Create_BaseSpaceship_With_Correct_Properties()
    {
        // Arrange
        var id = 100;
        var size = 10.5f;
        var direction = 90.0;
        var x = 1000.0;
        var y = 2000.0;
        var speed = 15.0;
        var name = "Test Spaceship";

        // Act
        var result = SpacecraftGenerator.BuildPlayerSpacecraft(id, size, direction, x, y, speed, name);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<BaseSpaceship>();
        result.Id.Should().Be(id);
        result.Size.Should().Be(size);
        result.Direction.Should().Be(direction);
        result.X.Should().Be(x);
        result.Y.Should().Be(y);
        result.Speed.Should().Be(speed);
        result.Name.Should().Be(name);
        result.OwnerId.Should().Be(id);
        result.Type.Should().Be(CelestialObjectType.SpaceshipPlayer);
        result.IsPreScanned.Should().BeTrue();
    }

    [Fact]
    public void BuildPlayerSpacecraft_Should_Set_Default_Spaceship_Properties()
    {
        // Arrange
        var id = 123;
        var size = 5.0f;
        var direction = 45.0;
        var x = 500.0;
        var y = 750.0;
        var speed = 10.0;
        var name = "Default Ship";

        // Act
        var result = SpacecraftGenerator.BuildPlayerSpacecraft(id, size, direction, x, y, speed, name);
        var spaceship = result as ISpacecraft;

        // Assert
        spaceship.Should().NotBeNull();
        spaceship.MaxSpeed.Should().Be(20);
        spaceship.Agility.Should().Be(0);
        spaceship.Crew.Should().NotBeNull();
        spaceship.Crew.Should().BeEmpty();
    }

    [Theory]
    [InlineData(0, 0f, 0.0, 0.0, 0.0, 0.0, "")]
    [InlineData(999, 100f, 359.0, -1000.0, -2000.0, 50.0, "Edge Case Ship")]
    [InlineData(-1, -5f, -90.0, 999999.0, 999999.0, -10.0, null)]
    public void BuildPlayerSpacecraft_Should_Handle_Edge_Cases(int id, float size, double direction, 
        double x, double y, double speed, string name)
    {
        // Act
        var result = SpacecraftGenerator.BuildPlayerSpacecraft(id, size, direction, x, y, speed, name);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(id);
        result.Size.Should().Be(size);
        result.Direction.Should().Be(direction);
        result.X.Should().Be(x);
        result.Y.Should().Be(y);
        result.Speed.Should().Be(speed);
        result.Name.Should().Be(name);
    }

    [Fact]
    public void BuildPlayerSpacecraft_Should_Return_ICelestialObject()
    {
        // Arrange
        var id = 1;
        var size = 1f;
        var direction = 0.0;
        var x = 0.0;
        var y = 0.0;
        var speed = 0.0;
        var name = "Test";

        // Act
        var result = SpacecraftGenerator.BuildPlayerSpacecraft(id, size, direction, x, y, speed, name);

        // Assert
        result.Should().BeAssignableTo<ICelestialObject>();
        result.Should().BeAssignableTo<ISpacecraft>();
    }

    [Fact]
    public void BuildPlayerSpacecraft_Should_Set_OwnerId_Same_As_Id()
    {
        // Arrange
        var id = 555;
        var size = 20f;
        var direction = 180.0;
        var x = 3000.0;
        var y = 4000.0;
        var speed = 25.0;
        var name = "Owner Test Ship";

        // Act
        var result = SpacecraftGenerator.BuildPlayerSpacecraft(id, size, direction, x, y, speed, name);

        // Assert
        result.OwnerId.Should().Be(id);
        result.Id.Should().Be(id);
    }

    [Fact]
    public void BuildPlayerSpacecraft_Should_Set_Correct_Spaceship_Type()
    {
        // Arrange
        var id = 777;
        var size = 15f;
        var direction = 270.0;
        var x = 5000.0;
        var y = 6000.0;
        var speed = 30.0;
        var name = "Type Test Ship";

        // Act
        var result = SpacecraftGenerator.BuildPlayerSpacecraft(id, size, direction, x, y, speed, name);

        // Assert
        result.Type.Should().Be(CelestialObjectType.SpaceshipPlayer);
    }

    [Fact]
    public void BuildPlayerSpacecraft_Should_Set_IsPreScanned_True()
    {
        // Arrange
        var id = 888;
        var size = 12f;
        var direction = 135.0;
        var x = 7000.0;
        var y = 8000.0;
        var speed = 18.0;
        var name = "PreScanned Ship";

        // Act
        var result = SpacecraftGenerator.BuildPlayerSpacecraft(id, size, direction, x, y, speed, name);

        // Assert
        result.IsPreScanned.Should().BeTrue();
    }
} 