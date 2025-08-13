using DeepSpaceSaga.Common.Abstractions.Dto.Ui;
using DeepSpaceSaga.Common.Extensions.Entities.CelestialObjects;
using DeepSpaceSaga.Common.Geometry;

namespace DeepSpaceSaga.Common.Tests.Extensions;

public class CelestialObjectDtoExtensionsTests
{
    #region GetLocation Method Tests

    [Fact]
    public void GetLocation_WithPositiveCoordinates_ShouldReturnCorrectSpaceMapPoint()
    {
        // Arrange
        var celestialObject = new CelestialObjectDto
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
        var celestialObject = new CelestialObjectDto
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
        var celestialObject = new CelestialObjectDto
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
        var celestialObject = new CelestialObjectDto
        {
            Name = "Test Object",
            X = -25.5,
            Y = 40.2
        };

        // Act
        var result = celestialObject.GetLocation();

        // Assert
        result.Should().NotBeNull();
        result.X.Should().Be(-25.5f);
        result.Y.Should().Be(40.2f);
    }

    [Fact]
    public void GetLocation_WithLargeCoordinates_ShouldReturnCorrectSpaceMapPoint()
    {
        // Arrange
        var celestialObject = new CelestialObjectDto
        {
            Name = "Test Object",
            X = 999999.999,
            Y = -888888.888
        };

        // Act
        var result = celestialObject.GetLocation();

        // Assert
        result.Should().NotBeNull();
        result.X.Should().Be(999999.999f);
        result.Y.Should().Be(-888888.888f);
    }

    [Fact]
    public void GetLocation_WithSmallFractionalCoordinates_ShouldReturnCorrectSpaceMapPoint()
    {
        // Arrange
        var celestialObject = new CelestialObjectDto
        {
            Name = "Test Object",
            X = 0.001,
            Y = 0.002
        };

        // Act
        var result = celestialObject.GetLocation();

        // Assert
        result.Should().NotBeNull();
        result.X.Should().BeApproximately(0.001f, 0.0001f);
        result.Y.Should().BeApproximately(0.002f, 0.0001f);
    }

    [Fact]
    public void GetLocation_WithMaxDoubleValues_ShouldHandleConversionToFloat()
    {
        // Arrange
        var celestialObject = new CelestialObjectDto
        {
            Name = "Test Object",
            X = double.MaxValue,
            Y = double.MinValue
        };

        // Act
        var result = celestialObject.GetLocation();

        // Assert
        result.Should().NotBeNull();
        result.X.Should().Be(float.PositiveInfinity);
        result.Y.Should().Be(float.NegativeInfinity);
    }

    [Fact]
    public void GetLocation_WithNaNValues_ShouldReturnNaNInSpaceMapPoint()
    {
        // Arrange
        var celestialObject = new CelestialObjectDto
        {
            Name = "Test Object",
            X = double.NaN,
            Y = double.NaN
        };

        // Act
        var result = celestialObject.GetLocation();

        // Assert
        result.Should().NotBeNull();
        float.IsNaN(result.X).Should().BeTrue();
        float.IsNaN(result.Y).Should().BeTrue();
    }

    [Fact]
    public void GetLocation_WithInfinityValues_ShouldReturnInfinityInSpaceMapPoint()
    {
        // Arrange
        var celestialObject = new CelestialObjectDto
        {
            Name = "Test Object",
            X = double.PositiveInfinity,
            Y = double.NegativeInfinity
        };

        // Act
        var result = celestialObject.GetLocation();

        // Assert
        result.Should().NotBeNull();
        result.X.Should().Be(float.PositiveInfinity);
        result.Y.Should().Be(float.NegativeInfinity);
    }

    [Theory]
    [InlineData(1.0, 2.0, 1.0f, 2.0f)]
    [InlineData(-1.0, -2.0, -1.0f, -2.0f)]
    [InlineData(0.0, 0.0, 0.0f, 0.0f)]
    [InlineData(123.456, 789.012, 123.456f, 789.012f)]
    [InlineData(-999.999, 1000.001, -999.999f, 1000.001f)]
    public void GetLocation_WithVariousCoordinates_ShouldConvertCorrectly(
        double inputX, double inputY, float expectedX, float expectedY)
    {
        // Arrange
        var celestialObject = new CelestialObjectDto
        {
            Name = "Test Object",
            X = inputX,
            Y = inputY
        };

        // Act
        var result = celestialObject.GetLocation();

        // Assert
        result.Should().NotBeNull();
        result.X.Should().Be(expectedX);
        result.Y.Should().Be(expectedY);
    }

    [Fact]
    public void GetLocation_CalledMultipleTimes_ShouldReturnConsistentResults()
    {
        // Arrange
        var celestialObject = new CelestialObjectDto
        {
            Name = "Test Object",
            X = 42.5,
            Y = 37.8
        };

        // Act
        var result1 = celestialObject.GetLocation();
        var result2 = celestialObject.GetLocation();

        // Assert
        result1.Should().NotBeNull();
        result2.Should().NotBeNull();
        result1.X.Should().Be(result2.X);
        result1.Y.Should().Be(result2.Y);
        result1.X.Should().Be(42.5f);
        result1.Y.Should().Be(37.8f);
        result2.X.Should().Be(42.5f);
        result2.Y.Should().Be(37.8f);
    }

    [Fact]
    public void GetLocation_WithDifferentObjectProperties_ShouldOnlyUseCoordinates()
    {
        // Arrange
        var celestialObject = new CelestialObjectDto
        {
            Id = 123,
            OwnerId = 456,
            Name = "Complex Object",
            Direction = 90.0,
            Speed = 15.5,
            X = 10.0,
            Y = 20.0,
            Type = CelestialObjectType.Asteroid,
            IsPreScanned = true,
            Size = 5.5f
        };

        // Act
        var result = celestialObject.GetLocation();

        // Assert
        result.Should().NotBeNull();
        result.X.Should().Be(10.0f);
        result.Y.Should().Be(20.0f);
    }

    #endregion
}
