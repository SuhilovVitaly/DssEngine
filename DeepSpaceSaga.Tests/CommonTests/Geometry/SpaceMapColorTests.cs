namespace DeepSpaceSaga.Tests.CommonTests.Geometry;

public class SpaceMapColorTests
{
    [Fact]
    public void Constructor_WithBytes_SetsPropertiesCorrectly()
    {
        // Arrange
        byte red = 10;
        byte green = 20;
        byte blue = 30;
        byte alpha = 40;

        // Act
        var color = new SpaceMapColor(red, green, blue, alpha);

        // Assert
        Assert.Equal(red, color.Red);
        Assert.Equal(green, color.Green);
        Assert.Equal(blue, color.Blue);
        Assert.Equal(alpha, color.Alpha);
    }

    [Fact]
    public void Constructor_WithBytes_DefaultAlphaIs250()
    {
        // Act
        var color = new SpaceMapColor(1, 2, 3);

        // Assert
        Assert.Equal((byte)1, color.Red);
        Assert.Equal((byte)2, color.Green);
        Assert.Equal((byte)3, color.Blue);
        Assert.Equal((byte)250, color.Alpha);
    }

    [Fact]
    public void Constructor_WithSystemDrawingColor_SetsPropertiesCorrectly()
    {
        // Arrange
        var sysColor = Color.FromArgb(100, 110, 120, 130);

        // Act
        var color = new SpaceMapColor(sysColor);

        // Assert
        Assert.Equal(sysColor.R, color.Red);
        Assert.Equal(sysColor.G, color.Green);
        Assert.Equal(sysColor.B, color.Blue);
        Assert.Equal(sysColor.A, color.Alpha);
    }

    [Fact]
    public void Properties_CanBeSetAndGet()
    {
        // Arrange
        var color = new SpaceMapColor(0, 0, 0);

        // Act
        color.Red = 200;
        color.Green = 201;
        color.Blue = 202;
        color.Alpha = 203;

        // Assert
        Assert.Equal((byte)200, color.Red);
        Assert.Equal((byte)201, color.Green);
        Assert.Equal((byte)202, color.Blue);
        Assert.Equal((byte)203, color.Alpha);
    }
}