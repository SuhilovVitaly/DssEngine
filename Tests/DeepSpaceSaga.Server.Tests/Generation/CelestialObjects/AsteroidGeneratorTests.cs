using DeepSpaceSaga.Common.Abstractions.Entities;
using DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects;
using DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects.Asteroids;
using DeepSpaceSaga.Common.Tools;
using DeepSpaceSaga.Server.Generation.CelestialObjects;
using FluentAssertions;
using Moq;

namespace DeepSpaceSaga.Server.Tests.Generation.CelestialObjects;

public class AsteroidGeneratorTests
{
    [Fact]
    public void BuildAsteroid_Should_Create_BaseAsteroid_With_Correct_Properties()
    {
        // Arrange
        var id = 200;
        var maxDrillAttempts = 3;
        var size = 75.5f;
        var direction = 270.0;
        var x = 5000.0;
        var y = 6000.0;
        var speed = 8.0;
        var name = "Test Asteroid";
        var isPreScanned = true;

        // Act
        var result = AsteroidGenerator.BuildAsteroid(id, maxDrillAttempts, size, direction, x, y, speed, name, isPreScanned);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<BaseAsteroid>();
        result.Id.Should().Be(id);
        result.Size.Should().Be(size);
        result.Direction.Should().Be(direction);
        result.X.Should().Be(x);
        result.Y.Should().Be(y);
        result.Speed.Should().Be(speed);
        result.Name.Should().Be(name);
        result.OwnerId.Should().Be(0);
        result.Type.Should().Be(CelestialObjectType.Asteroid);
        result.IsPreScanned.Should().Be(isPreScanned);
    }

    [Fact]
    public void BuildAsteroid_Should_Set_RemainingDrillAttempts()
    {
        // Arrange
        var id = 201;
        var maxDrillAttempts = 5;
        var size = 100f;
        var direction = 0.0;
        var x = 0.0;
        var y = 0.0;
        var speed = 0.0;
        var name = "Drill Test Asteroid";

        // Act
        var result = AsteroidGenerator.BuildAsteroid(id, maxDrillAttempts, size, direction, x, y, speed, name);
        var asteroid = result as IAsteroid;

        // Assert
        asteroid.Should().NotBeNull();
        asteroid.RemainingDrillAttempts.Should().Be(maxDrillAttempts);
    }

    [Fact]
    public void BuildAsteroid_Should_Set_Default_IsPreScanned_False()
    {
        // Arrange
        var id = 202;
        var maxDrillAttempts = 2;
        var size = 50f;
        var direction = 180.0;
        var x = 1000.0;
        var y = 2000.0;
        var speed = 5.0;
        var name = "Default PreScanned Asteroid";

        // Act
        var result = AsteroidGenerator.BuildAsteroid(id, maxDrillAttempts, size, direction, x, y, speed, name);

        // Assert
        result.IsPreScanned.Should().BeFalse();
    }

    [Theory]
    [InlineData(0, 0, 0f, 0.0, 0.0, 0.0, 0.0, "", false)]
    [InlineData(-1, -5, -10f, -90.0, -1000.0, -2000.0, -5.0, null, true)]
    [InlineData(999, 100, 500f, 360.0, 999999.0, 999999.0, 100.0, "Edge Case", true)]
    public void BuildAsteroid_Should_Handle_Edge_Cases(int id, int maxDrillAttempts, float size, 
        double direction, double x, double y, double speed, string name, bool isPreScanned)
    {
        // Act
        var result = AsteroidGenerator.BuildAsteroid(id, maxDrillAttempts, size, direction, x, y, speed, name, isPreScanned);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(id);
        result.Size.Should().Be(size);
        result.Direction.Should().Be(direction);
        result.X.Should().Be(x);
        result.Y.Should().Be(y);
        result.Speed.Should().Be(speed);
        result.Name.Should().Be(name);
        result.IsPreScanned.Should().Be(isPreScanned);
        ((IAsteroid)result).RemainingDrillAttempts.Should().Be(maxDrillAttempts);
    }

    [Fact]
    public void BuildAsteroid_Should_Always_Set_OwnerId_To_Zero()
    {
        // Arrange
        var id = 203;
        var maxDrillAttempts = 3;
        var size = 80f;
        var direction = 45.0;
        var x = 3000.0;
        var y = 4000.0;
        var speed = 12.0;
        var name = "Owner Test Asteroid";

        // Act
        var result = AsteroidGenerator.BuildAsteroid(id, maxDrillAttempts, size, direction, x, y, speed, name);

        // Assert
        result.OwnerId.Should().Be(0);
    }

    [Fact]
    public void BuildAsteroid_Should_Always_Set_Type_To_Asteroid()
    {
        // Arrange
        var id = 204;
        var maxDrillAttempts = 4;
        var size = 120f;
        var direction = 315.0;
        var x = 7000.0;
        var y = 8000.0;
        var speed = 3.0;
        var name = "Type Test Asteroid";

        // Act
        var result = AsteroidGenerator.BuildAsteroid(id, maxDrillAttempts, size, direction, x, y, speed, name);

        // Assert
        result.Type.Should().Be(CelestialObjectType.Asteroid);
    }

    [Fact]
    public void BuildAsteroid_Should_Return_ICelestialObject()
    {
        // Arrange
        var id = 205;
        var maxDrillAttempts = 1;
        var size = 25f;
        var direction = 135.0;
        var x = 500.0;
        var y = 750.0;
        var speed = 2.0;
        var name = "Interface Test";

        // Act
        var result = AsteroidGenerator.BuildAsteroid(id, maxDrillAttempts, size, direction, x, y, speed, name);

        // Assert
        result.Should().BeAssignableTo<ICelestialObject>();
        result.Should().BeAssignableTo<IAsteroid>();
    }

    [Fact]
    public void CreateAsteroid_Should_Use_GenerationTool_Values()
    {
        // Arrange
        var mockGenerationTool = new Mock<IGenerationTool>();
        var expectedId = 12345;
        var expectedDrillAttempts = 3;
        var expectedSize = 150.5f;
        var direction = 90.0;
        var x = 2000.0;
        var y = 3000.0;
        var speed = 7.5;
        var name = "Generated Asteroid";
        var isPreScanned = true;

        mockGenerationTool.Setup(g => g.GetId()).Returns(expectedId);
        mockGenerationTool.Setup(g => g.GetInteger(2, 4)).Returns(expectedDrillAttempts);
        mockGenerationTool.Setup(g => g.GetFloat(350)).Returns(expectedSize);

        // Act
        var result = AsteroidGenerator.CreateAsteroid(mockGenerationTool.Object, direction, x, y, speed, name, isPreScanned);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(expectedId);
        result.Direction.Should().Be(direction);
        result.X.Should().Be(x);
        result.Y.Should().Be(y);
        result.Speed.Should().Be(speed);
        result.Name.Should().Be(name);
        result.IsPreScanned.Should().Be(isPreScanned);
        result.Size.Should().Be(expectedSize);
        ((IAsteroid)result).RemainingDrillAttempts.Should().Be(expectedDrillAttempts);
        
        mockGenerationTool.Verify(g => g.GetId(), Times.Once);
        mockGenerationTool.Verify(g => g.GetInteger(2, 4), Times.Once);
        mockGenerationTool.Verify(g => g.GetFloat(350), Times.Once);
    }

    [Fact]
    public void CreateAsteroid_Should_Set_Default_IsPreScanned_False()
    {
        // Arrange
        var mockGenerationTool = new Mock<IGenerationTool>();
        mockGenerationTool.Setup(g => g.GetId()).Returns(999);
        mockGenerationTool.Setup(g => g.GetInteger(2, 4)).Returns(2);
        mockGenerationTool.Setup(g => g.GetFloat(350)).Returns(100f);

        var direction = 45.0;
        var x = 1000.0;
        var y = 1500.0;
        var speed = 4.0;
        var name = "Default PreScanned Test";

        // Act
        var result = AsteroidGenerator.CreateAsteroid(mockGenerationTool.Object, direction, x, y, speed, name);

        // Assert
        result.IsPreScanned.Should().BeFalse();
    }

    [Fact]
    public void CreateAsteroid_Should_Always_Set_Fixed_Properties()
    {
        // Arrange
        var mockGenerationTool = new Mock<IGenerationTool>();
        mockGenerationTool.Setup(g => g.GetId()).Returns(777);
        mockGenerationTool.Setup(g => g.GetInteger(2, 4)).Returns(3);
        mockGenerationTool.Setup(g => g.GetFloat(350)).Returns(200f);

        var direction = 225.0;
        var x = 4000.0;
        var y = 5000.0;
        var speed = 6.0;
        var name = "Fixed Properties Test";

        // Act
        var result = AsteroidGenerator.CreateAsteroid(mockGenerationTool.Object, direction, x, y, speed, name);

        // Assert
        result.OwnerId.Should().Be(0);
        result.Type.Should().Be(CelestialObjectType.Asteroid);
    }

    [Fact]
    public void CreateAsteroid_Should_Use_Correct_Generation_Tool_Parameters()
    {
        // Arrange
        var mockGenerationTool = new Mock<IGenerationTool>();
        mockGenerationTool.Setup(g => g.GetId()).Returns(1);
        mockGenerationTool.Setup(g => g.GetInteger(It.IsAny<int>(), It.IsAny<int>())).Returns(1);
        mockGenerationTool.Setup(g => g.GetFloat(It.IsAny<double>())).Returns(1f);

        // Act
        AsteroidGenerator.CreateAsteroid(mockGenerationTool.Object, 0, 0, 0, 0, "Test");

        // Assert
        mockGenerationTool.Verify(g => g.GetInteger(2, 4), Times.Once, "Should call GetInteger with min=2, max=4 for drill attempts");
        mockGenerationTool.Verify(g => g.GetFloat(350), Times.Once, "Should call GetFloat with max=350 for size");
    }
} 