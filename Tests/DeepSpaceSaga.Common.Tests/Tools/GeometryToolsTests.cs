namespace DeepSpaceSaga.Common.Tests.Tools;

public class GeometryToolsTests
{
    [Fact]
    public void Azimuth_Should_Return_Zero_For_Point_To_The_Right()
    {
        // Arrange
        var center = new SpaceMapPoint(0, 0);
        var destination = new SpaceMapPoint(1, 0);

        // Act
        var result = GeometryTools.Azimuth(destination, center);

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public void Azimuth_Should_Return_90_For_Point_Above()
    {
        // Arrange
        var center = new SpaceMapPoint(0, 0);
        var destination = new SpaceMapPoint(0, 1);

        // Act
        var result = GeometryTools.Azimuth(destination, center);

        // Assert
        result.Should().Be(90);
    }

    [Fact]
    public void Azimuth_Should_Return_180_For_Point_To_The_Left()
    {
        // Arrange
        var center = new SpaceMapPoint(0, 0);
        var destination = new SpaceMapPoint(-1, 0);

        // Act
        var result = GeometryTools.Azimuth(destination, center);

        // Assert
        result.Should().Be(180);
    }

    [Fact]
    public void Azimuth_Should_Return_270_For_Point_Below()
    {
        // Arrange
        var center = new SpaceMapPoint(0, 0);
        var destination = new SpaceMapPoint(0, -1);

        // Act
        var result = GeometryTools.Azimuth(destination, center);

        // Assert
        result.Should().Be(270);
    }

    [Theory]
    [InlineData(1, 1, 45)]
    [InlineData(-1, 1, 135)]
    [InlineData(-1, -1, 225)]
    [InlineData(1, -1, 315)]
    public void Azimuth_Should_Calculate_Diagonal_Angles_Correctly(float x, float y, double expectedAngle)
    {
        // Arrange
        var center = new SpaceMapPoint(0, 0);
        var destination = new SpaceMapPoint(x, y);

        // Act
        var result = GeometryTools.Azimuth(destination, center);

        // Assert
        result.Should().BeApproximately(expectedAngle, 0.001);
    }

    [Fact]
    public void Move_Should_Move_Point_Correctly_At_Zero_Degrees()
    {
        // Arrange
        var currentLocation = new SpaceMapPoint(0, 0);
        var speed = 10.0;
        var angle = 0.0;

        // Act
        var result = GeometryTools.Move(currentLocation, speed, angle);

        // Assert
        result.X.Should().BeApproximately(10, 0.001f);
        result.Y.Should().BeApproximately(0, 0.001f);
    }

    [Fact]
    public void Move_Should_Move_Point_Correctly_At_90_Degrees()
    {
        // Arrange
        var currentLocation = new SpaceMapPoint(0, 0);
        var speed = 10.0;
        var angle = 90.0;

        // Act
        var result = GeometryTools.Move(currentLocation, speed, angle);

        // Assert
        result.X.Should().BeApproximately(0, 0.001f);
        result.Y.Should().BeApproximately(10, 0.001f);
    }

    [Theory]
    [InlineData(0, 0, 3, 4, 5)]
    [InlineData(1, 1, 4, 5, 5)]
    [InlineData(-1, -1, 2, 2, 4.242640687)]
    public void Distance_Should_Calculate_Correctly(float x1, float y1, float x2, float y2, double expectedDistance)
    {
        // Arrange
        var p1 = new SpaceMapPoint(x1, y1);
        var p2 = new SpaceMapPoint(x2, y2);

        // Act
        var result = GeometryTools.Distance(p1, p2);

        // Assert
        result.Should().BeApproximately(expectedDistance, 0.001);
    }

    [Fact]
    public void Distance_Should_Return_Zero_For_Same_Points()
    {
        // Arrange
        var p1 = new SpaceMapPoint(5, 5);
        var p2 = new SpaceMapPoint(5, 5);

        // Act
        var result = GeometryTools.Distance(p1, p2);

        // Assert
        result.Should().Be(0);
    }

    [Theory]
    [InlineData(0, 90, 90)]
    [InlineData(90, 0, 90)]
    [InlineData(350, 10, 340)]
    [InlineData(10, 350, 340)]
    [InlineData(180, 180, 0)]
    [InlineData(45, 135, 90)]
    [InlineData(270, 90, 180)]
    public void DirectionsDelta_Should_Calculate_Correctly(float p1, float p2, double expectedDelta)
    {
        // Act
        var result = GeometryTools.DirectionsDelta(p1, p2);

        // Assert
        result.Should().BeApproximately(expectedDelta, 0.001);
    }

    [Theory]
    [InlineData(0, 0, 10, 10, 5, 5)]
    [InlineData(-5, -5, 5, 5, 0, 0)]
    [InlineData(1, 2, 3, 4, 2, 3)]
    public void GetCentreLine_Should_Calculate_Midpoint_Correctly(float x1, float y1, float x2, float y2, float expectedX, float expectedY)
    {
        // Arrange
        var from = new SpaceMapPoint(x1, y1);
        var to = new SpaceMapPoint(x2, y2);

        // Act
        var result = GeometryTools.GetCentreLine(from, to);

        // Assert
        result.X.Should().BeApproximately(expectedX, 0.001f);
        result.Y.Should().BeApproximately(expectedY, 0.001f);
    }

    [Fact]
    public void CalculateTangents_Should_Calculate_Tangent_Points_Correctly()
    {
        // Arrange
        var externalPoint = new SpaceMapPoint(5, 0);
        var circleCenter = new SpaceMapPoint(0, 0);
        var radius = 3f;

        // Act
        var (tangent1, tangent2) = GeometryTools.CalculateTangents(externalPoint, circleCenter, radius);

        // Assert
        // Check that tangent points are on the circle
        var distance1 = GeometryTools.Distance(tangent1, circleCenter);
        var distance2 = GeometryTools.Distance(tangent2, circleCenter);
        
        distance1.Should().BeApproximately(radius, 0.001);
        distance2.Should().BeApproximately(radius, 0.001);

        // Check that tangents are perpendicular to radii
        // For this we check that distance from external point to tangent points is equal
        var distanceToTangent1 = GeometryTools.Distance(externalPoint, tangent1);
        var distanceToTangent2 = GeometryTools.Distance(externalPoint, tangent2);
        
        distanceToTangent1.Should().BeApproximately(distanceToTangent2, 0.001);
    }

    [Fact]
    public void CalculateTangents_Should_Throw_Exception_When_Point_Inside_Circle()
    {
        // Arrange
        var externalPoint = new SpaceMapPoint(1, 0);
        var circleCenter = new SpaceMapPoint(0, 0);
        var radius = 3f;

        // Act & Assert
        var action = () => GeometryTools.CalculateTangents(externalPoint, circleCenter, radius);
        action.Should().Throw<ArgumentException>()
            .WithMessage("External point must be outside the circle.");
    }

    [Fact]
    public void CalculateTangents_Should_Throw_Exception_When_Point_On_Circle()
    {
        // Arrange
        var externalPoint = new SpaceMapPoint(3, 0);
        var circleCenter = new SpaceMapPoint(0, 0);
        var radius = 3f;

        // Act & Assert
        var action = () => GeometryTools.CalculateTangents(externalPoint, circleCenter, radius);
        action.Should().Throw<ArgumentException>()
            .WithMessage("External point must be outside the circle.");
    }

    [Fact]
    public void Move_Should_Handle_Negative_Speed()
    {
        // Arrange
        var currentLocation = new SpaceMapPoint(10, 10);
        var speed = -5.0;
        var angle = 0.0;

        // Act
        var result = GeometryTools.Move(currentLocation, speed, angle);

        // Assert
        result.X.Should().BeApproximately(5, 0.001f);
        result.Y.Should().BeApproximately(10, 0.001f);
    }

    [Fact]
    public void Move_Should_Handle_Large_Angles()
    {
        // Arrange
        var currentLocation = new SpaceMapPoint(0, 0);
        var speed = 10.0;
        var angle = 450.0; // 450 degrees = 90 degrees

        // Act
        var result = GeometryTools.Move(currentLocation, speed, angle);

        // Assert
        result.X.Should().BeApproximately(0, 0.001f);
        result.Y.Should().BeApproximately(10, 0.001f);
    }

    [Fact]
    public void Move_Should_Handle_Zero_Speed()
    {
        // Arrange
        var currentLocation = new SpaceMapPoint(5, 5);
        var speed = 0.0;
        var angle = 45.0;

        // Act
        var result = GeometryTools.Move(currentLocation, speed, angle);

        // Assert
        result.X.Should().BeApproximately(5, 0.001f);
        result.Y.Should().BeApproximately(5, 0.001f);
    }

    [Theory]
    [InlineData(0.0, 0.0)]
    [InlineData(45.0, 45.0)]
    [InlineData(90.0, 90.0)]
    [InlineData(180.0, 180.0)]
    [InlineData(270.0, 270.0)]
    [InlineData(0.0, 360.0)]
    [InlineData(270.0, -90.0)]
    [InlineData(0.0, 720.0)]
    public void Azimuth_Should_Handle_Various_Center_Positions(double expectedAngle, double inputAngle)
    {
        // Arrange
        var center = new SpaceMapPoint(10, 10);
        var radius = 5.0;
        var radians = inputAngle * Math.PI / 180.0;
        var destination = new SpaceMapPoint(
            (float)(center.X + radius * Math.Cos(radians)),
            (float)(center.Y + radius * Math.Sin(radians))
        );

        // Act
        var result = GeometryTools.Azimuth(destination, center);

        // Assert
        result.Should().BeApproximately(expectedAngle, 0.1);
    }

    [Fact]
    public void Azimuth_Should_Handle_Same_Points()
    {
        // Arrange
        var center = new SpaceMapPoint(5, 5);
        var destination = new SpaceMapPoint(5, 5);

        // Act
        var result = GeometryTools.Azimuth(destination, center);

        // Assert
        result.Should().Be(0);
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(180, 180, 0)]
    [InlineData(359, 1, 358)] // 359-1 = 358, To360Degrees делает abs(359-1) = 358
    [InlineData(1, 359, 358)] // 1-359 = -358, To360Degrees делает abs(-358) = 358  
    [InlineData(180, 0, 180)]
    [InlineData(0, 180, 180)]
    public void DirectionsDelta_Should_Handle_Edge_Cases(float p1, float p2, double expectedDelta)
    {
        // Act
        var result = GeometryTools.DirectionsDelta(p1, p2);

        // Assert
        result.Should().BeApproximately(expectedDelta, 0.001);
    }

    [Fact]
    public void Distance_Should_Handle_Very_Small_Distances()
    {
        // Arrange
        var p1 = new SpaceMapPoint(0, 0);
        var p2 = new SpaceMapPoint(0.0001f, 0.0001f);

        // Act
        var result = GeometryTools.Distance(p1, p2);

        // Assert
        result.Should().BeApproximately(0.0001414, 0.0001);
    }

    [Fact]
    public void Distance_Should_Handle_Large_Distances()
    {
        // Arrange
        var p1 = new SpaceMapPoint(-1000000, -1000000);
        var p2 = new SpaceMapPoint(1000000, 1000000);

        // Act
        var result = GeometryTools.Distance(p1, p2);

        // Assert
        result.Should().BeApproximately(2828427.125, 1);
    }

    [Fact]
    public void GetCentreLine_Should_Handle_Same_Points()
    {
        // Arrange
        var point = new SpaceMapPoint(10, 20);

        // Act
        var result = GeometryTools.GetCentreLine(point, point);

        // Assert
        result.X.Should().Be(10);
        result.Y.Should().Be(20);
    }

    [Fact]
    public void CalculateTangents_Should_Work_With_Different_Circle_Positions()
    {
        // Arrange
        var externalPoint = new SpaceMapPoint(10, 5);
        var circleCenter = new SpaceMapPoint(3, 2);
        var radius = 2f;

        // Act
        var (tangent1, tangent2) = GeometryTools.CalculateTangents(externalPoint, circleCenter, radius);

        // Assert
        // Check that tangent points are on the circle
        var distance1 = GeometryTools.Distance(tangent1, circleCenter);
        var distance2 = GeometryTools.Distance(tangent2, circleCenter);
        
        distance1.Should().BeApproximately(radius, 0.001);
        distance2.Should().BeApproximately(radius, 0.001);

        // Check that the lines from external point to tangent points have equal length
        var distanceToTangent1 = GeometryTools.Distance(externalPoint, tangent1);
        var distanceToTangent2 = GeometryTools.Distance(externalPoint, tangent2);
        
        distanceToTangent1.Should().BeApproximately(distanceToTangent2, 0.001);
    }

    [Fact]
    public void CalculateTangents_Should_Work_With_Large_Circle()
    {
        // Arrange
        var externalPoint = new SpaceMapPoint(100, 0);
        var circleCenter = new SpaceMapPoint(0, 0);
        var radius = 50f;

        // Act
        var (tangent1, tangent2) = GeometryTools.CalculateTangents(externalPoint, circleCenter, radius);

        // Assert
        // Check that tangent points are on the circle
        var distance1 = GeometryTools.Distance(tangent1, circleCenter);
        var distance2 = GeometryTools.Distance(tangent2, circleCenter);
        
        distance1.Should().BeApproximately(radius, 0.001);
        distance2.Should().BeApproximately(radius, 0.001);

        // For this specific case, tangent points should be symmetric about x-axis
        tangent1.Y.Should().BeApproximately(-tangent2.Y, 0.001f);
        tangent1.X.Should().BeApproximately(tangent2.X, 0.001f);
    }

    [Theory]
    [InlineData(180.0)]
    [InlineData(270.0)]
    [InlineData(360.0)]
    [InlineData(-90.0)]
    public void Move_Should_Handle_Various_Angle_Ranges(double angle)
    {
        // Arrange
        var currentLocation = new SpaceMapPoint(0, 0);
        var speed = 10.0;

        // Act
        var result = GeometryTools.Move(currentLocation, speed, angle);

        // Assert
        var expectedX = speed * Math.Cos(angle * Math.PI / 180.0);
        var expectedY = speed * Math.Sin(angle * Math.PI / 180.0);
        
        result.X.Should().BeApproximately((float)expectedX, 0.001f);
        result.Y.Should().BeApproximately((float)expectedY, 0.001f);
    }

    [Fact]
    public void Move_Should_Maintain_Precision_With_Non_Integer_Coordinates()
    {
        // Arrange
        var currentLocation = new SpaceMapPoint(1.5f, 2.7f);
        var speed = 3.14159;
        var angle = 33.33;

        // Act
        var result = GeometryTools.Move(currentLocation, speed, angle);

        // Assert
        var angleInRadians = angle * Math.PI / 180.0;
        var expectedX = currentLocation.X + speed * Math.Cos(angleInRadians);
        var expectedY = currentLocation.Y + speed * Math.Sin(angleInRadians);
        
        result.X.Should().BeApproximately((float)expectedX, 0.001f);
        result.Y.Should().BeApproximately((float)expectedY, 0.001f);
    }
} 