namespace DeepSpaceSaga.Tests.CommonTests.Geometry;

public class SpaceMapVectorTests
{
    [Fact]
    public void Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var from = new SpaceMapPoint(1.5f, 2.5f);
        var to = new SpaceMapPoint(3.5f, 4.5f);
        var direction = 123.45;

        // Act
        var vector = new SpaceMapVector(from, to, direction);

        // Assert
        vector.PointFrom.Should().Be(from);
        vector.PointTo.Should().Be(to);
        vector.Direction.Should().Be(direction);
    }

    [Fact]
    public void Properties_ShouldBeSettable()
    {
        // Arrange
        var vector = new SpaceMapVector(new SpaceMapPoint(0, 0), new SpaceMapPoint(0, 0), 0);
        var newFrom = new SpaceMapPoint(10, 20);
        var newTo = new SpaceMapPoint(30, 40);
        var newDirection = 270.0;

        // Act
        vector.PointFrom = newFrom;
        vector.PointTo = newTo;
        vector.Direction = newDirection;

        // Assert
        vector.PointFrom.Should().Be(newFrom);
        vector.PointTo.Should().Be(newTo);
        vector.Direction.Should().Be(newDirection);
    }
}