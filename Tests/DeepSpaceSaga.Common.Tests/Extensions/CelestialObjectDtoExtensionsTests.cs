using DeepSpaceSaga.Common.Abstractions.Dto.Ui;
using DeepSpaceSaga.Common.Extensions.Entities.CelestialObjects;
using DeepSpaceSaga.Common.Geometry;
using FluentAssertions;

namespace DeepSpaceSaga.Common.Tests.Extensions;

public class CelestialObjectDtoExtensionsTests
{
    #region GetLocation Method Tests

    [Fact]
    public void GetLocation_WithPositiveCoordinates_ShouldReturnCorrectSpaceMapPoint()
    {
        // Arrange
        var celestialObject = new CelestialObjectSaveFormatDto
        {
            Name = "Test Object",
            X = 100.5,
            Y = 200.7
        };

        // Act
        var result = celestialObject.GetLocation();

        // Assert
        result.Should().NotBeNull();
        result.X.Should().Be(100.5f);
        result.Y.Should().Be(200.7f);
        result.Should().BeOfType<SpaceMapPoint>();
    }

    [Fact]
    public void GetLocation_WithNegativeCoordinates_ShouldReturnCorrectSpaceMapPoint()
    {
        // Arrange
        var celestialObject = new CelestialObjectSaveFormatDto
        {
            Name = "Test Object",
            X = -50.3,
            Y = -75.8
        };

        // Act
        var result = celestialObject.GetLocation();

        // Assert
        result.Should().NotBeNull();
        result.X.Should().Be(-50.3f);
        result.Y.Should().Be(-75.8f);
    }

    [Fact]
    public void GetLocation_WithZeroCoordinates_ShouldReturnOriginPoint()
    {
        // Arrange
        var celestialObject = new CelestialObjectSaveFormatDto
        {
            Name = "Test Object",
            X = 0.0,
            Y = 0.0
        };

        // Act
        var result = celestialObject.GetLocation();

        // Assert
        result.Should().NotBeNull();
        result.X.Should().Be(0.0f);
        result.Y.Should().Be(0.0f);
    }

    [Fact]
    public void GetLocation_WithMixedSignCoordinates_ShouldReturnCorrectSpaceMapPoint()
    {
        // Arrange
        var celestialObject = new CelestialObjectSaveFormatDto
        {
            Name = "Test Object",
            X = 150.2,
            Y = -25.8
        };

        // Act
        var result = celestialObject.GetLocation();

        // Assert
        result.Should().NotBeNull();
        result.X.Should().Be(150.2f);
        result.Y.Should().Be(-25.8f);
    }

    #endregion

    #region ToSpaceship Method Tests

    [Fact]
    public void ToSpaceship_ShouldReturnNull_WhenCelestialObjectIsNotSpaceship()
    {
        // Arrange
        var celestialObject = new CelestialObjectSaveFormatDto
        {
            Name = "Test Object",
            Type = CelestialObjectType.Asteroid
        };

        // Act
        var result = celestialObject.ToSpaceship();

        // Assert
        result.Should().BeNull();
    }

    #endregion
}
