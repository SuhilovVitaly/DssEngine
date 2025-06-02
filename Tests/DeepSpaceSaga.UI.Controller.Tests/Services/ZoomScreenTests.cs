namespace DeepSpaceSaga.UI.Controller.Tests.Services;

public class ZoomScreenTests
{
    [Fact]
    public void Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var zoomScreen = new ZoomScreen();

        // Assert
        Assert.Equal(100, zoomScreen.Scale);
        Assert.Equal(1, zoomScreen.DrawScaleFactor);
    }

    [Fact]
    public void In_ShouldDecreaseScale_WhenScaleIsAboveMinimum()
    {
        // Arrange
        var zoomScreen = new ZoomScreen();
        var initialScale = zoomScreen.Scale;

        // Act
        zoomScreen.In();

        // Assert
        Assert.True(zoomScreen.Scale < initialScale);
    }

    [Fact]
    public void In_ShouldNotGoBelow40_WhenScaleWouldBeTooLow()
    {
        // Arrange
        var zoomScreen = new ZoomScreen();
        zoomScreen.Scale = 45; // Set close to minimum

        // Act
        zoomScreen.In();

        // Assert
        Assert.Equal(40, zoomScreen.Scale);
    }

    [Fact]
    public void In_ShouldUpdateDrawScaleFactor_WhenScaleChanges()
    {
        // Arrange
        var zoomScreen = new ZoomScreen();
        zoomScreen.Scale = 200;

        // Act
        zoomScreen.In();

        // Assert
        var expectedDrawScaleFactor = (float)Math.Round(100.0f / zoomScreen.Scale, 1);
        Assert.Equal(expectedDrawScaleFactor, zoomScreen.DrawScaleFactor);
    }

    [Fact]
    public void In_ShouldTriggerOnZoomInEvent()
    {
        // Arrange
        var zoomScreen = new ZoomScreen();
        var eventTriggered = false;
        zoomScreen.OnZoomIn += () => eventTriggered = true;

        // Act
        zoomScreen.In();

        // Assert
        Assert.True(eventTriggered);
    }

    [Fact]
    public void Out_ShouldIncreaseScale_WhenScaleIsBelowMaximum()
    {
        // Arrange
        var zoomScreen = new ZoomScreen();
        var initialScale = zoomScreen.Scale;

        // Act
        zoomScreen.Out();

        // Assert
        Assert.True(zoomScreen.Scale > initialScale);
    }

    [Fact]
    public void Out_ShouldNotGoAbove1000_WhenScaleWouldBeTooHigh()
    {
        // Arrange
        var zoomScreen = new ZoomScreen();
        zoomScreen.Scale = 950; // Set close to maximum

        // Act
        zoomScreen.Out();

        // Assert
        Assert.Equal(1000, zoomScreen.Scale);
    }

    [Fact]
    public void Out_ShouldUpdateDrawScaleFactor_WhenScaleChanges()
    {
        // Arrange
        var zoomScreen = new ZoomScreen();
        zoomScreen.Scale = 50;

        // Act
        zoomScreen.Out();

        // Assert
        var expectedDrawScaleFactor = (float)Math.Round(100.0f / zoomScreen.Scale, 1);
        Assert.Equal(expectedDrawScaleFactor, zoomScreen.DrawScaleFactor);
    }

    [Fact]
    public void Out_ShouldTriggerOnZoomOutEvent()
    {
        // Arrange
        var zoomScreen = new ZoomScreen();
        var eventTriggered = false;
        zoomScreen.OnZoomOut += () => eventTriggered = true;

        // Act
        zoomScreen.Out();

        // Assert
        Assert.True(eventTriggered);
    }

    [Theory]
    [InlineData(50, 10)]
    [InlineData(150, 10)]
    [InlineData(250, 20)]
    [InlineData(450, 50)]
    [InlineData(750, 100)]
    public void SetDeltaByScale_ShouldReturnCorrectDelta_ForDifferentScales(int scale, int expectedDelta)
    {
        // Arrange
        var zoomScreen = new ZoomScreen();
        zoomScreen.Scale = scale;

        // Act
        zoomScreen.In(); // This will use SetDeltaByScale internally
        var actualDelta = scale - zoomScreen.Scale;

        // Assert
        Assert.Equal(expectedDelta, actualDelta);
    }

    [Fact]
    public void Scale_ShouldBeSettableAndGettable()
    {
        // Arrange
        var zoomScreen = new ZoomScreen();
        var newScale = 200;

        // Act
        zoomScreen.Scale = newScale;

        // Assert
        Assert.Equal(newScale, zoomScreen.Scale);
    }

    [Fact]
    public void DrawScaleFactor_ShouldBeSettableAndGettable()
    {
        // Arrange
        var zoomScreen = new ZoomScreen();
        var newDrawScaleFactor = 2.5f;

        // Act
        zoomScreen.DrawScaleFactor = newDrawScaleFactor;

        // Assert
        Assert.Equal(newDrawScaleFactor, zoomScreen.DrawScaleFactor);
    }

    [Fact]
    public void DrawScaleFactor_ShouldBeCalculatedCorrectly_AfterZoomOut()
    {
        // Arrange
        var zoomScreen = new ZoomScreen();
        zoomScreen.Scale = 50;

        // Act
        zoomScreen.Out(); // Scale becomes 60 (50 + 10)

        // Assert
        var expectedDrawScaleFactor = (float)Math.Round(100.0f / 60, 1);
        Assert.Equal(expectedDrawScaleFactor, zoomScreen.DrawScaleFactor);
    }

    [Fact]
    public void MultipleZoomIn_ShouldRespectMinimumLimit()
    {
        // Arrange
        var zoomScreen = new ZoomScreen();
        zoomScreen.Scale = 50;

        // Act
        for (int i = 0; i < 10; i++)
        {
            zoomScreen.In();
        }

        // Assert
        Assert.Equal(40, zoomScreen.Scale);
    }

    [Fact]
    public void MultipleZoomOut_ShouldRespectMaximumLimit()
    {
        // Arrange
        var zoomScreen = new ZoomScreen();
        zoomScreen.Scale = 900;

        // Act
        for (int i = 0; i < 10; i++)
        {
            zoomScreen.Out();
        }

        // Assert
        Assert.Equal(1000, zoomScreen.Scale);
    }

    [Fact]
    public void Events_ShouldNotThrow_WhenNoSubscribers()
    {
        // Arrange
        var zoomScreen = new ZoomScreen();

        // Act & Assert
        var exception1 = Record.Exception(() => zoomScreen.In());
        var exception2 = Record.Exception(() => zoomScreen.Out());

        Assert.Null(exception1);
        Assert.Null(exception2);
    }

    [Fact]
    public void Events_ShouldSupportMultipleSubscribers()
    {
        // Arrange
        var zoomScreen = new ZoomScreen();
        var zoomInCount = 0;
        var zoomOutCount = 0;

        zoomScreen.OnZoomIn += () => zoomInCount++;
        zoomScreen.OnZoomIn += () => zoomInCount++;
        zoomScreen.OnZoomOut += () => zoomOutCount++;
        zoomScreen.OnZoomOut += () => zoomOutCount++;

        // Act
        zoomScreen.In();
        zoomScreen.Out();

        // Assert
        Assert.Equal(2, zoomInCount);
        Assert.Equal(2, zoomOutCount);
    }
} 