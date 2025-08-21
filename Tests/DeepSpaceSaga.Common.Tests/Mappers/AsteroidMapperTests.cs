using DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects;
using DeepSpaceSaga.Common.Abstractions.Mappers.CelestialObjects;
using DeepSpaceSaga.Common.Abstractions.Dto.Ui;
using FluentAssertions;

namespace DeepSpaceSaga.Common.Tests.Mappers;

public class AsteroidMapperTests
{
    [Fact]
    public void ToGameObject_Should_Map_All_Properties_Correctly()
    {
        // Arrange
        var celestialObjectDto = new CelestialObjectSaveFormatDto
        {
            Id = 1,
            OwnerId = 2,
            Name = "Test Asteroid",
            Direction = 45.0,
            X = 100.0,
            Y = 200.0,
            Speed = 10.0,
            Type = CelestialObjectType.Asteroid,
            IsPreScanned = true,
            Size = 5.0f
        };

        // Act
        var result = AsteroidMapper.ToGameObject(celestialObjectDto);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(celestialObjectDto.Id);
        result.Name.Should().Be(celestialObjectDto.Name);
        result.Direction.Should().Be(celestialObjectDto.Direction);
        result.Speed.Should().Be(celestialObjectDto.Speed);
        result.X.Should().Be(celestialObjectDto.X);
        result.Y.Should().Be(celestialObjectDto.Y);
        result.Type.Should().Be(celestialObjectDto.Type);
        result.IsPreScanned.Should().Be(celestialObjectDto.IsPreScanned);
        result.Size.Should().Be(celestialObjectDto.Size);
    }

    [Fact]
    public void ToGameObject_Should_Return_BaseAsteroid_Type()
    {
        // Arrange
        var celestialObjectDto = new CelestialObjectSaveFormatDto
        {
            Id = 1,
            Name = "Test Asteroid",
            Type = CelestialObjectType.Asteroid
        };

        // Act
        var result = AsteroidMapper.ToGameObject(celestialObjectDto);

        // Assert
        result.Should().BeOfType<BaseAsteroid>();
    }

    [Fact]
    public void ToGameObject_Should_Handle_Different_Asteroid_Types()
    {
        // Arrange
        var celestialObjectDto = new CelestialObjectSaveFormatDto
        {
            Id = 1,
            Name = "Test Asteroid",
            Type = CelestialObjectType.Asteroid
        };

        // Act
        var result = AsteroidMapper.ToGameObject(celestialObjectDto);

        // Assert
        result.Type.Should().Be(CelestialObjectType.Asteroid);
    }

    [Theory]
    [InlineData(0, 0, 0.0, 0.0)]
    [InlineData(1, 1, 1.0, 1.0)]
    [InlineData(-1, -1, -1.0, -1.0)]
    [InlineData(100.5, 200.7, 100.5, 200.7)]
    public void ToGameObject_Should_Map_Coordinates_Correctly(int id, int ownerId, double x, double y)
    {
        // Arrange
        var celestialObjectDto = new CelestialObjectSaveFormatDto
        {
            Id = id,
            OwnerId = ownerId,
            Name = "Test Asteroid",
            X = x,
            Y = y,
            Type = CelestialObjectType.Asteroid
        };

        // Act
        var result = AsteroidMapper.ToGameObject(celestialObjectDto);

        // Assert
        result.X.Should().Be(x);
        result.Y.Should().Be(y);
    }

    [Theory]
    [InlineData("Asteroid Alpha", 45.0, 10.0, 5.0f)]
    [InlineData("Asteroid Beta", 90.0, 20.0, 10.0f)]
    [InlineData("Asteroid Gamma", 180.0, 5.0, 2.5f)]
    public void ToGameObject_Should_Map_Properties_With_Different_Values(string name, double direction, double speed, float size)
    {
        // Arrange
        var celestialObjectDto = new CelestialObjectSaveFormatDto
        {
            Id = 1,
            Name = name,
            Direction = direction,
            Speed = speed,
            Size = size,
            Type = CelestialObjectType.Asteroid
        };

        // Act
        var result = AsteroidMapper.ToGameObject(celestialObjectDto);

        // Assert
        result.Name.Should().Be(name);
        result.Direction.Should().Be(direction);
        result.Speed.Should().Be(speed);
        result.Size.Should().Be(size);
    }

    [Fact]
    public void ToGameObject_Should_Map_Boolean_Properties_Correctly()
    {
        // Arrange
        var celestialObjectDto = new CelestialObjectSaveFormatDto
        {
            Id = 1,
            Name = "Test Asteroid",
            IsPreScanned = true,
            Type = CelestialObjectType.Asteroid
        };

        // Act
        var result = AsteroidMapper.ToGameObject(celestialObjectDto);

        // Assert
        result.IsPreScanned.Should().Be(true);
    }

    [Fact]
    public void ToGameObject_Should_Handle_Zero_Values()
    {
        // Arrange
        var celestialObjectDto = new CelestialObjectSaveFormatDto
        {
            Id = 0,
            OwnerId = 0,
            Name = "Test Asteroid",
            Direction = 0.0,
            X = 0.0,
            Y = 0.0,
            Speed = 0.0,
            Type = CelestialObjectType.Asteroid,
            IsPreScanned = false,
            Size = 0.0f
        };

        // Act
        var result = AsteroidMapper.ToGameObject(celestialObjectDto);

        // Assert
        result.Id.Should().Be(0);
        result.OwnerId.Should().Be(0);
        result.Direction.Should().Be(0.0);
        result.X.Should().Be(0.0);
        result.Y.Should().Be(0.0);
        result.Speed.Should().Be(0.0);
        result.IsPreScanned.Should().Be(false);
        result.Size.Should().Be(0.0f);
    }
} 