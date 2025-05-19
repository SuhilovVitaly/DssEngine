namespace DeepSpaceSaga.Tests.CommonTests.Geometry;

public class SpaceMapPointTests
{
    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        float x = 12.34f;
        float y = 56.78f;

        // Act
        var point = new SpaceMapPoint(x, y);

        // Assert
        point.X.Should().Be(x);
        point.Y.Should().Be(y);
    }

    [Fact]
    public void Properties_CanBeSetAndGet()
    {
        // Arrange
        var point = new SpaceMapPoint(0, 0);

        // Act
        point.X = 99.99f;
        point.Y = -42.42f;

        // Assert
        point.X.Should().Be(99.99f);
        point.Y.Should().Be(-42.42f);
    }
}