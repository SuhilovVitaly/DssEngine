namespace DeepSpaceSaga.Tests.CommonTests.Geometry;

public class SpaceMapLineTests
{
    [Fact]
    public void Constructor_ShouldSetFromAndToPoints()
    {
        // Arrange
        var from = new SpaceMapPoint(1.5f, 2.5f);
        var to = new SpaceMapPoint(3.5f, 4.5f);

        // Act
        var line = new SpaceMapLine(from, to);

        // Assert
        line.From.Should().Be(from);
        line.To.Should().Be(to);
    }

    [Fact]
    public void Properties_ShouldBeSettable()
    {
        // Arrange
        var line = new SpaceMapLine(new SpaceMapPoint(0, 0), new SpaceMapPoint(1, 1));
        var newFrom = new SpaceMapPoint(10, 20);
        var newTo = new SpaceMapPoint(30, 40);

        // Act
        line.From = newFrom;
        line.To = newTo;

        // Assert
        line.From.Should().Be(newFrom);
        line.To.Should().Be(newTo);
    }

    [Fact]
    public void FromAndTo_ShouldAllowSamePoint()
    {
        // Arrange
        var point = new SpaceMapPoint(5, 5);

        // Act
        var line = new SpaceMapLine(point, point);

        // Assert
        line.From.Should().Be(point);
        line.To.Should().Be(point);
    }
}