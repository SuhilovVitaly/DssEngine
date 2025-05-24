namespace DeepSpaceSaga.Tests.CommonTests.Mappers;

public class CelestialObjectMapperTests
{
    [Fact]
    public void ToDto_Should_Map_All_Properties_Correctly()
    {
        // Arrange
        var celestialObject = new BaseAsteroid(1)
        {
            Name = "Asteroid 001",
            Type = CelestialObjectType.Spacecraft,
            IsPreScanned = true,
            X = 123,
            Y = 678,
            Id = new GenerationTool().GetId()
        };

        // Act
        var dto = CelestialObjectMapper.ToDto(celestialObject);

        // Assert
        dto.Should().NotBeNull();
        dto.Type.Should().Be(celestialObject.Type);
        dto.IsPreScanned.Should().Be(celestialObject.IsPreScanned);
        dto.X.Should().Be(celestialObject.X);
        dto.Y.Should().Be(celestialObject.Y);
        dto.Id.Should().Be(celestialObject.Id);
    }

    [Fact]
    public void ToDto_Should_Not_Return_Null()
    {
        // Arrange
        var celestialObject = new BaseAsteroid(1)
        {
            Name = "Asteroid 001",
            Type = CelestialObjectType.Spacecraft,
            IsPreScanned = true,
            X = 123,
            Y = 678,
            Id = new GenerationTool().GetId()
        };

        // Act
        var dto = CelestialObjectMapper.ToDto(celestialObject);

        // Assert
        dto.Should().NotBeNull();
    }

    [Theory]
    [InlineData(CelestialObjectType.Unknown)]
    [InlineData(CelestialObjectType.Asteroid)]
    [InlineData(CelestialObjectType.Spacecraft)]
    public void ToDto_Should_Map_Different_CelestialObjectTypes_Correctly(CelestialObjectType type)
    {
        // Arrange
        var celestialObject = new BaseAsteroid(1)
        {
            Name = "Asteroid 001",
            Type = CelestialObjectType.Spacecraft,
            IsPreScanned = true,
            X = 123,
            Y = 678,
            Id = new GenerationTool().GetId()
        };

        celestialObject.Type = type;

        // Act
        var dto = CelestialObjectMapper.ToDto(celestialObject);

        // Assert
        dto.Type.Should().Be(type);
    }

    [Fact]
    public void ToDto_Should_Map_Default_Values_Correctly()
    {
        // Arrange
        var celestialObject = new BaseAsteroid(1)
        {
            Name = "Asteroid 001",
            Type = CelestialObjectType.Spacecraft,
            IsPreScanned = true,
            X = 123,
            Y = 678,
            Id = new GenerationTool().GetId()
        };

        // Act
        var dto = CelestialObjectMapper.ToDto(celestialObject);

        // Assert
        dto.Type.Should().Be(CelestialObjectType.Spacecraft);
        dto.IsPreScanned.Should().Be(true);
        dto.X.Should().Be(123);
        dto.Y.Should().Be(678);
        dto.Id.Should().Be(celestialObject.Id);
    }
} 