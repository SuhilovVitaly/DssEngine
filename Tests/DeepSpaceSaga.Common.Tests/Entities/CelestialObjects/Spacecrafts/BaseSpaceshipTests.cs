using DeepSpaceSaga.Common.Abstractions.Dto.Ui;
using DeepSpaceSaga.Common.Abstractions.Entities.Characters;
using DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects.Spacecrafts;
using DeepSpaceSaga.Common.Abstractions.Entities;
using FluentAssertions;
using Moq;

namespace DeepSpaceSaga.Common.Tests.Entities.CelestialObjects.Spacecrafts;

public class BaseSpaceshipTests
{
    [Fact]
    public void Constructor_Should_Initialize_Default_Values()
    {
        // Act
        var spaceship = new BaseSpaceship();

        // Assert
        spaceship.MaxSpeed.Should().Be(0);
        spaceship.Agility.Should().Be(0);
        spaceship.Crew.Should().NotBeNull();
        spaceship.Crew.Should().BeEmpty();
    }

    [Fact]
    public void Constructor_With_Dto_Should_Load_Object_Properties()
    {
        // Arrange
        var dto = new CelestialObjectDto
        {
            Id = 123,
            OwnerId = 456,
            Name = "Test Ship",
            Direction = 90.0,
            Speed = 15.0,
            X = 1000.0,
            Y = 2000.0,
            Type = CelestialObjectType.SpaceshipPlayer,
            IsPreScanned = true,
            Size = 50.0f
        };

        // Act
        var spaceship = new BaseSpaceship(dto);

        // Assert
        spaceship.Id.Should().Be(123);
        spaceship.OwnerId.Should().Be(456);
        spaceship.Name.Should().Be("Test Ship");
        spaceship.Direction.Should().Be(90.0);
        spaceship.Speed.Should().Be(15.0);
        spaceship.X.Should().Be(1000.0);
        spaceship.Y.Should().Be(2000.0);
        spaceship.Type.Should().Be(CelestialObjectType.SpaceshipPlayer);
        spaceship.IsPreScanned.Should().BeTrue();
        spaceship.Size.Should().Be(50.0f);
        spaceship.Crew.Should().NotBeNull();
        spaceship.Crew.Should().BeEmpty();
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(45.0)]
    [InlineData(90.0)]
    [InlineData(180.0)]
    [InlineData(270.0)]
    [InlineData(360.0)]
    public void SetDirection_Should_Update_Direction(double direction)
    {
        // Arrange
        var spaceship = new BaseSpaceship();

        // Act
        spaceship.SetDirection(direction);

        // Assert
        spaceship.Direction.Should().Be(direction);
    }

    [Theory]
    [InlineData(0, 10, 10)]
    [InlineData(10, 5, 15)]
    [InlineData(10, -5, 5)]
    [InlineData(0, -10, 0)]
    public void ChangeVelocity_Should_Update_Speed_Within_Bounds(double currentSpeed, double delta, double expectedSpeed)
    {
        // Arrange
        var spaceship = new BaseSpaceship
        {
            Speed = currentSpeed,
            MaxSpeed = 20
        };

        // Act
        spaceship.ChangeVelocity(delta);

        // Assert
        spaceship.Speed.Should().Be(expectedSpeed);
    }

    [Fact]
    public void ChangeVelocity_Should_Not_Exceed_MaxSpeed()
    {
        // Arrange
        var spaceship = new BaseSpaceship
        {
            Speed = 15,
            MaxSpeed = 20
        };

        // Act
        spaceship.ChangeVelocity(10);

        // Assert
        spaceship.Speed.Should().Be(20);
    }

    [Fact]
    public void ChangeVelocity_Should_Not_Go_Below_Zero()
    {
        // Arrange
        var spaceship = new BaseSpaceship
        {
            Speed = 5,
            MaxSpeed = 20
        };

        // Act
        spaceship.ChangeVelocity(-10);

        // Assert
        spaceship.Speed.Should().Be(0);
    }

    [Fact]
    public void AddCrewMember_Should_Add_Character_To_Crew()
    {
        // Arrange
        var spaceship = new BaseSpaceship();
        var mockCharacter = new Mock<ICharacter>();
        mockCharacter.Setup(c => c.Id).Returns(1);

        // Act
        spaceship.AddCrewMember(mockCharacter.Object);

        // Assert
        spaceship.Crew.Should().HaveCount(1);
        spaceship.Crew.Should().Contain(mockCharacter.Object);
    }

    [Fact]
    public void AddCrewMember_Should_Add_Multiple_Characters()
    {
        // Arrange
        var spaceship = new BaseSpaceship();
        var mockCharacter1 = new Mock<ICharacter>();
        var mockCharacter2 = new Mock<ICharacter>();
        mockCharacter1.Setup(c => c.Id).Returns(1);
        mockCharacter2.Setup(c => c.Id).Returns(2);

        // Act
        spaceship.AddCrewMember(mockCharacter1.Object);
        spaceship.AddCrewMember(mockCharacter2.Object);

        // Assert
        spaceship.Crew.Should().HaveCount(2);
        spaceship.Crew.Should().Contain(mockCharacter1.Object);
        spaceship.Crew.Should().Contain(mockCharacter2.Object);
    }

    [Fact]
    public void GetCrewCapacity_Should_Return_Zero_By_Default()
    {
        // Arrange
        var spaceship = new BaseSpaceship();

        // Act
        var capacity = spaceship.GetCrewCapacity();

        // Assert
        capacity.Should().Be(0);
    }

    [Theory]
    [InlineData(0.0f)]
    [InlineData(10.5f)]
    [InlineData(100.0f)]
    public void MaxSpeed_Should_Set_And_Get_Correctly(float maxSpeed)
    {
        // Arrange
        var spaceship = new BaseSpaceship();

        // Act
        spaceship.MaxSpeed = maxSpeed;

        // Assert
        spaceship.MaxSpeed.Should().Be(maxSpeed);
    }

    [Theory]
    [InlineData(0.0f)]
    [InlineData(5.5f)]
    [InlineData(10.0f)]
    public void Agility_Should_Set_And_Get_Correctly(float agility)
    {
        // Arrange
        var spaceship = new BaseSpaceship();

        // Act
        spaceship.Agility = agility;

        // Assert
        spaceship.Agility.Should().Be(agility);
    }
} 