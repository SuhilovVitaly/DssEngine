using DeepSpaceSaga.Common.Abstractions.Dto.Ui;
using DeepSpaceSaga.Common.Abstractions.Mappers.CelestialObjects;

namespace DeepSpaceSaga.Common.Tests.Mappers;

public class AsteroidMapperTests
{
    [Fact]
    public void ToGameObject_Should_Map_All_Properties_Correctly()
    {
        // Arrange
        var celestialObjectDto = new CelestialObjectDto
        {
            Id = 123,
            Name = "Test Asteroid",
            Direction = 45.5,
            Speed = 10.5,
            X = 100.0,
            Y = 200.0,
            IsPreScanned = true,
            Size = 150.5f,
            Type = CelestialObjectType.Station, // This should be overridden
            OwnerId = 999 // This should be overridden
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
        result.IsPreScanned.Should().Be(celestialObjectDto.IsPreScanned);
        result.Size.Should().Be(celestialObjectDto.Size);
    }

    [Fact]
    public void ToGameObject_Should_Set_Fixed_Properties_Correctly()
    {
        // Arrange
        var celestialObjectDto = new CelestialObjectDto
        {
            Id = 123,
            Name = "Test Asteroid",
            Type = CelestialObjectType.Station, // This should be overridden
            OwnerId = 999 // This should be overridden
        };

        // Act
        var result = AsteroidMapper.ToGameObject(celestialObjectDto);

        // Assert
        result.OwnerId.Should().Be(0);
        result.Type.Should().Be(CelestialObjectType.Asteroid);
    }

    [Fact]
    public void ToGameObject_Should_Return_BaseAsteroid_Instance()
    {
        // Arrange
        var celestialObjectDto = new CelestialObjectDto
        {
            Id = 123,
            Name = "Test Asteroid"
        };

        // Act
        var result = AsteroidMapper.ToGameObject(celestialObjectDto);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<BaseAsteroid>();
        result.Should().BeAssignableTo<ICelestialObject>();
        result.Should().BeAssignableTo<IAsteroid>();
    }

    [Fact]
    public void ToGameObject_Should_Set_RemainingDrillAttempts_To_One()
    {
        // Arrange
        var celestialObjectDto = new CelestialObjectDto
        {
            Id = 123,
            Name = "Test Asteroid"
        };

        // Act
        var result = AsteroidMapper.ToGameObject(celestialObjectDto);

        // Assert
        var asteroid = result.Should().BeOfType<BaseAsteroid>().Subject;
        asteroid.RemainingDrillAttempts.Should().Be(1);
    }

    [Fact]
    public void ToGameObject_Should_Handle_Zero_Values()
    {
        // Arrange
        var celestialObjectDto = new CelestialObjectDto
        {
            Id = 0,
            Name = "Zero Asteroid",
            Direction = 0.0,
            Speed = 0.0,
            X = 0.0,
            Y = 0.0,
            Size = 0.0f
        };

        // Act
        var result = AsteroidMapper.ToGameObject(celestialObjectDto);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(0);
        result.Direction.Should().Be(0.0);
        result.Speed.Should().Be(0.0);
        result.X.Should().Be(0.0);
        result.Y.Should().Be(0.0);
        result.Size.Should().Be(0.0f);
    }

    [Fact]
    public void ToGameObject_Should_Handle_Negative_Values()
    {
        // Arrange
        var celestialObjectDto = new CelestialObjectDto
        {
            Id = -100,
            Name = "Negative Asteroid",
            Direction = -45.5,
            Speed = -10.0,
            X = -200.0,
            Y = -300.0,
            Size = -50.0f
        };

        // Act
        var result = AsteroidMapper.ToGameObject(celestialObjectDto);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(-100);
        result.Direction.Should().Be(-45.5);
        result.Speed.Should().Be(-10.0);
        result.X.Should().Be(-200.0);
        result.Y.Should().Be(-300.0);
        result.Size.Should().Be(-50.0f);
    }

    [Fact]
    public void ToGameObject_Should_Handle_Large_Values()
    {
        // Arrange
        var celestialObjectDto = new CelestialObjectDto
        {
            Id = int.MaxValue,
            Name = "Large Asteroid",
            Direction = double.MaxValue,
            Speed = double.MaxValue,
            X = double.MaxValue,
            Y = double.MaxValue,
            Size = float.MaxValue
        };

        // Act
        var result = AsteroidMapper.ToGameObject(celestialObjectDto);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(int.MaxValue);
        result.Direction.Should().Be(double.MaxValue);
        result.Speed.Should().Be(double.MaxValue);
        result.X.Should().Be(double.MaxValue);
        result.Y.Should().Be(double.MaxValue);
        result.Size.Should().Be(float.MaxValue);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ToGameObject_Should_Map_IsPreScanned_Correctly(bool isPreScanned)
    {
        // Arrange
        var celestialObjectDto = new CelestialObjectDto
        {
            Id = 123,
            Name = "Test Asteroid",
            IsPreScanned = isPreScanned
        };

        // Act
        var result = AsteroidMapper.ToGameObject(celestialObjectDto);

        // Assert
        result.IsPreScanned.Should().Be(isPreScanned);
    }

    [Theory]
    [InlineData("")]
    [InlineData("A")]
    [InlineData("Very Long Asteroid Name With Special Characters 123!@#")]
    [InlineData("Астероид с русскими символами")]
    public void ToGameObject_Should_Map_Name_Correctly(string name)
    {
        // Arrange
        var celestialObjectDto = new CelestialObjectDto
        {
            Id = 123,
            Name = name
        };

        // Act
        var result = AsteroidMapper.ToGameObject(celestialObjectDto);

        // Assert
        result.Name.Should().Be(name);
    }

    [Theory]
    [InlineData(CelestialObjectType.Unknown)]
    [InlineData(CelestialObjectType.Station)]
    [InlineData(CelestialObjectType.SpaceshipPlayer)]
    [InlineData(CelestialObjectType.Missile)]
    public void ToGameObject_Should_Always_Set_Type_To_Asteroid_Regardless_Of_Input(CelestialObjectType inputType)
    {
        // Arrange
        var celestialObjectDto = new CelestialObjectDto
        {
            Id = 123,
            Name = "Test Asteroid",
            Type = inputType
        };

        // Act
        var result = AsteroidMapper.ToGameObject(celestialObjectDto);

        // Assert
        result.Type.Should().Be(CelestialObjectType.Asteroid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(999)]
    [InlineData(-1)]
    [InlineData(int.MaxValue)]
    public void ToGameObject_Should_Always_Set_OwnerId_To_Zero_Regardless_Of_Input(int inputOwnerId)
    {
        // Arrange
        var celestialObjectDto = new CelestialObjectDto
        {
            Id = 123,
            Name = "Test Asteroid",
            OwnerId = inputOwnerId
        };

        // Act
        var result = AsteroidMapper.ToGameObject(celestialObjectDto);

        // Assert
        result.OwnerId.Should().Be(0);
    }

    [Fact]
    public void ToGameObject_Should_Throw_NullReferenceException_When_CelestialObjectDto_Is_Null()
    {
        // Act & Assert
        var action = () => AsteroidMapper.ToGameObject(null!);
        action.Should().Throw<NullReferenceException>();
    }

    [Fact]
    public void ToGameObject_Should_Return_Object_With_Default_Values_When_Dto_Has_Default_Values()
    {
        // Arrange
        var celestialObjectDto = new CelestialObjectDto
        {
            Name = "Default Asteroid"
        };

        // Act
        var result = AsteroidMapper.ToGameObject(celestialObjectDto);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(0);
        result.Direction.Should().Be(0.0);
        result.Speed.Should().Be(0.0);
        result.X.Should().Be(0.0);
        result.Y.Should().Be(0.0);
        result.IsPreScanned.Should().BeFalse();
        result.Size.Should().Be(0.0f);
        result.Type.Should().Be(CelestialObjectType.Asteroid);
        result.OwnerId.Should().Be(0);
    }

    [Fact]
    public void ToGameObject_Should_Create_Independent_Object_Instance()
    {
        // Arrange
        var celestialObjectDto = new CelestialObjectDto
        {
            Id = 123,
            Name = "Test Asteroid",
            X = 100.0,
            Y = 200.0
        };

        // Act
        var result1 = AsteroidMapper.ToGameObject(celestialObjectDto);
        var result2 = AsteroidMapper.ToGameObject(celestialObjectDto);

        // Assert
        result1.Should().NotBeSameAs(result2);
        result1.Should().BeEquivalentTo(result2);
    }
} 