using System.Numerics;

namespace DeepSpaceSaga.Common.Tests.Extensions;

public class GeometryExtensionsTests
{
    #region ToSpaceMapCoordinates Tests

    [Fact]
    public void ToSpaceMapCoordinates_PointF_Should_Convert_Correctly()
    {
        // Arrange
        var pointF = new PointF(12.34f, 56.78f);

        // Act
        var result = pointF.ToSpaceMapCoordinates();

        // Assert
        result.Should().NotBeNull();
        result.X.Should().Be(12.34f);
        result.Y.Should().Be(56.78f);
    }

    [Fact]
    public void ToSpaceMapCoordinates_Point_Should_Convert_Correctly()
    {
        // Arrange
        var point = new Point(123, 456);

        // Act
        var result = point.ToSpaceMapCoordinates();

        // Assert
        result.Should().NotBeNull();
        result.X.Should().Be(123f);
        result.Y.Should().Be(456f);
    }

    [Fact]
    public void ToSpaceMapCoordinates_PointF_Zero_Should_Work()
    {
        // Arrange
        var pointF = new PointF(0f, 0f);

        // Act
        var result = pointF.ToSpaceMapCoordinates();

        // Assert
        result.X.Should().Be(0f);
        result.Y.Should().Be(0f);
    }

    [Fact]
    public void ToSpaceMapCoordinates_Point_Negative_Should_Work()
    {
        // Arrange
        var point = new Point(-100, -200);

        // Act
        var result = point.ToSpaceMapCoordinates();

        // Assert
        result.X.Should().Be(-100f);
        result.Y.Should().Be(-200f);
    }

    #endregion

    #region To360Degrees Tests

    [Theory]
    [InlineData(0f, 0f)]
    [InlineData(180f, 180f)]
    [InlineData(360f, 0f)]      // ИСПРАВЛЕНО: 360° = 0°
    [InlineData(450f, 90f)]
    [InlineData(720f, 0f)]      // ИСПРАВЛЕНО: 720° = 0° (2 полных оборота)
    [InlineData(-90f, 270f)]
    [InlineData(-180f, 180f)]
    [InlineData(-270f, 90f)]
    [InlineData(-360f, 0f)]
    [InlineData(-450f, 270f)]   // ИСПРАВЛЕНО: -450° = 270°
    public void To360Degrees_Float_Should_Normalize_Angle_Correctly(float input, float expected)
    {
        // Act
        var result = input.To360Degrees();

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(0.0, 0.0)]
    [InlineData(180.0, 180.0)]
    [InlineData(360.0, 0.0)]      // ИСПРАВЛЕНО: 360° = 0°
    [InlineData(450.0, 90.0)]
    [InlineData(720.0, 0.0)]      // ИСПРАВЛЕНО: 720° = 0° (2 полных оборота)
    [InlineData(-90.0, 270.0)]
    [InlineData(-180.0, 180.0)]
    [InlineData(-270.0, 90.0)]
    [InlineData(-360.0, 0.0)]
    [InlineData(-450.0, 270.0)]   // ИСПРАВЛЕНО: -450° = 270°
    public void To360Degrees_Double_Should_Normalize_Angle_Correctly(double input, double expected)
    {
        // Act
        var result = input.To360Degrees();

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void To360Degrees_Float_Multiple_Rotations_Should_Work()
    {
        // Arrange
        float angle = 1080f; // 3 full rotations

        // Act
        var result = angle.To360Degrees();

        // Assert
        // ИСПРАВЛЕНО: 3 полных оборота = 0°
        result.Should().Be(0f);
    }

    [Fact]
    public void To360Degrees_Double_Large_Negative_Should_Work()
    {
        // Arrange
        double angle = -1170.0; // -3.25 rotations

        // Act
        var result = angle.To360Degrees();

        // Assert
        // ИСПРАВЛЕНО: -1170° = 270° (-3.25 оборота = -3 полных + 0.75 = 270°)
        result.Should().Be(270.0);
    }

    [Theory]
    [InlineData(3600f, 0f)]     // 10 полных оборотов
    [InlineData(3690f, 90f)]    // 10 оборотов + 90°
    [InlineData(-3600f, 0f)]    // -10 полных оборотов
    [InlineData(-3690f, 270f)]  // -10 оборотов - 90° = 270°
    [InlineData(36000f, 0f)]    // 100 полных оборотов
    [InlineData(0.5f, 0.5f)]    // Дробные углы
    [InlineData(359.9f, 359.9f)] // Почти полный оборот
    public void To360Degrees_Float_Should_Handle_Edge_Cases(float input, float expected)
    {
        // Act
        var result = input.To360Degrees();

        // Assert
        result.Should().BeApproximately(expected, 0.0001f);
    }

    [Theory]
    [InlineData(3600.0, 0.0)]     // 10 полных оборотов
    [InlineData(3690.0, 90.0)]    // 10 оборотов + 90°
    [InlineData(-3600.0, 0.0)]    // -10 полных оборотов
    [InlineData(-3690.0, 270.0)]  // -10 оборотов - 90° = 270°
    [InlineData(36000.0, 0.0)]    // 100 полных оборотов
    [InlineData(0.5, 0.5)]        // Дробные углы
    [InlineData(359.9, 359.9)]    // Почти полный оборот
    public void To360Degrees_Double_Should_Handle_Edge_Cases(double input, double expected)
    {
        // Act
        var result = input.To360Degrees();

        // Assert
        result.Should().BeApproximately(expected, 0.0001);
    }

    #endregion

    #region ToInt Tests

    [Theory]
    [InlineData(0.0, 0)]
    [InlineData(1.0, 1)]
    [InlineData(1.9, 1)]
    [InlineData(2.1, 2)]
    [InlineData(-1.0, -1)]
    [InlineData(-1.9, -1)]
    [InlineData(-2.1, -2)]
    [InlineData(123.456, 123)]
    [InlineData(-123.456, -123)]
    public void ToInt_Should_Truncate_Double_Correctly(double input, int expected)
    {
        // Act
        var result = input.ToInt();

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void ToInt_MaxValue_Should_Work()
    {
        // Arrange
        double value = int.MaxValue;

        // Act
        var result = value.ToInt();

        // Assert
        result.Should().Be(int.MaxValue);
    }

    [Fact]
    public void ToInt_MinValue_Should_Work()
    {
        // Arrange
        double value = int.MinValue;

        // Act
        var result = value.ToInt();

        // Assert
        result.Should().Be(int.MinValue);
    }

    #endregion

    #region Vector2 Conversion Tests

    [Fact]
    public void ToVector2_PointF_Should_Convert_Correctly()
    {
        // Arrange
        var pointF = new PointF(12.34f, 56.78f);

        // Act
        var result = pointF.ToVector2();

        // Assert
        result.Should().NotBeNull();
        result.X.Should().Be(12.34f);
        result.Y.Should().Be(56.78f);
    }

    [Fact]
    public void ToVector2_Point_Should_Convert_Correctly()
    {
        // Arrange
        var point = new Point(123, 456);

        // Act
        var result = point.ToVector2();

        // Assert
        result.Should().NotBeNull();
        result.X.Should().Be(123f);
        result.Y.Should().Be(456f);
    }

    [Fact]
    public void ToVector2_Zero_Points_Should_Work()
    {
        // Arrange
        var pointF = new PointF(0f, 0f);
        var point = new Point(0, 0);

        // Act
        var resultF = pointF.ToVector2();
        var resultP = point.ToVector2();

        // Assert
        resultF.Should().Be(Vector2.Zero);
        resultP.Should().Be(Vector2.Zero);
    }

    #endregion

    #region Point Conversion Tests

    [Fact]
    public void ToPoint_Vector2_Should_Convert_Correctly()
    {
        // Arrange
        var vector = new Vector2(12.34f, 56.78f);

        // Act
        var result = vector.ToPoint();

        // Assert
        result.Should().NotBeNull();
        result.X.Should().Be(12);
        result.Y.Should().Be(56);
    }

    [Fact]
    public void ToPoint_Vector2_Should_Truncate_Decimals()
    {
        // Arrange
        var vector = new Vector2(12.99f, -56.99f);

        // Act
        var result = vector.ToPoint();

        // Assert
        result.X.Should().Be(12);
        result.Y.Should().Be(-56);
    }

    [Fact]
    public void ToPoint_Vector2_Zero_Should_Work()
    {
        // Arrange
        var vector = Vector2.Zero;

        // Act
        var result = vector.ToPoint();

        // Assert
        result.X.Should().Be(0);
        result.Y.Should().Be(0);
    }

    #endregion

    #region PointF Conversion Tests

    [Fact]
    public void ToPointF_Vector2_Should_Convert_Correctly()
    {
        // Arrange
        var vector = new Vector2(12.34f, 56.78f);

        // Act
        var result = vector.ToPointF();

        // Assert
        result.Should().NotBeNull();
        result.X.Should().Be(12.34f);
        result.Y.Should().Be(56.78f);
    }

    [Fact]
    public void ToPointF_Point_Should_Convert_Correctly()
    {
        // Arrange
        var point = new Point(123, 456);

        // Act
        var result = point.ToPointF();

        // Assert
        result.Should().NotBeNull();
        result.X.Should().Be(123f);
        result.Y.Should().Be(456f);
    }

    [Fact]
    public void ToPointF_Negative_Values_Should_Work()
    {
        // Arrange
        var vector = new Vector2(-12.34f, -56.78f);
        var point = new Point(-123, -456);

        // Act
        var resultV = vector.ToPointF();
        var resultP = point.ToPointF();

        // Assert
        resultV.X.Should().Be(-12.34f);
        resultV.Y.Should().Be(-56.78f);
        resultP.X.Should().Be(-123f);
        resultP.Y.Should().Be(-456f);
    }

    #endregion

    #region Roundtrip Conversion Tests

    [Fact]
    public void PointF_To_Vector2_To_PointF_Roundtrip_Should_Preserve_Values()
    {
        // Arrange
        var original = new PointF(12.34f, 56.78f);

        // Act
        var result = original.ToVector2().ToPointF();

        // Assert
        result.X.Should().Be(original.X);
        result.Y.Should().Be(original.Y);
    }

    [Fact]
    public void Point_To_Vector2_To_Point_Roundtrip_Should_Preserve_Values()
    {
        // Arrange
        var original = new Point(123, 456);

        // Act
        var result = original.ToVector2().ToPoint();

        // Assert
        result.X.Should().Be(original.X);
        result.Y.Should().Be(original.Y);
    }

    [Fact]
    public void PointF_To_SpaceMapPoint_Should_Preserve_Values()
    {
        // Arrange
        var original = new PointF(12.34f, 56.78f);

        // Act
        var result = original.ToSpaceMapCoordinates();

        // Assert
        result.X.Should().Be(original.X);
        result.Y.Should().Be(original.Y);
    }

    #endregion
}
