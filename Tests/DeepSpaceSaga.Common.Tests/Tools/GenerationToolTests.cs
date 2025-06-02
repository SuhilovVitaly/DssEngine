namespace DeepSpaceSaga.Common.Tests.Tools;

public class GenerationToolTests
{
    private readonly GenerationTool _tool;

    public GenerationToolTests()
    {
        _tool = new GenerationTool();
    }

    [Fact]
    public void GetInteger_ReturnsValueInRange()
    {
        // Arrange
        int min = 1;
        int max = 100;

        // Act
        int result = _tool.GetInteger(min, max);

        // Assert
        Assert.InRange(result, min, max);
    }

    [Fact]
    public void GenerateCelestialObjectName_ReturnsCorrectFormat()
    {
        // Act
        string result = _tool.GenerateCelestialObjectName();

        // Assert
        string[] parts = result.Split('-');
        Assert.Equal(3, parts.Length);
        Assert.Equal(4, parts[0].Length);
        Assert.Equal(4, parts[1].Length);
        Assert.Equal(3, parts[2].Length);
        Assert.Matches("^[A-Z]{4}-[0-9]{4}-[0-9]{3}$", result);
    }

    [Fact]
    public void Direction_ReturnsValueInValidRange()
    {
        // Act
        double result = _tool.Direction();

        // Assert
        Assert.InRange(result, 0, 359);
    }

    [Fact]
    public void GetDouble_ReturnsValueInRange()
    {
        // Arrange
        double min = 1.0;
        double max = 100.0;

        // Act
        double result = _tool.GetDouble(min, max);

        // Assert
        Assert.InRange(result, min, max);
    }

    [Fact]
    public void GetFloat_ReturnsValueInRange()
    {
        // Arrange
        double min = 1.0;
        double max = 100.0;

        // Act
        float result = _tool.GetFloat(min, max);

        // Assert
        Assert.InRange(result, min, max);
    }

    [Fact]
    public void GetId_ReturnsPositiveInteger()
    {
        // Act
        int result = _tool.GetId();

        // Assert
        Assert.True(result > 0);
    }

    [Theory]
    [InlineData(5)]
    [InlineData(10)]
    public void RandomString_ReturnsCorrectLength(int length)
    {
        // Act
        string result = _tool.RandomString(length);

        // Assert
        Assert.Equal(length, result.Length);
        Assert.Matches($"^[A-Z]{{{length}}}$", result);
    }

    [Theory]
    [InlineData(5)]
    [InlineData(10)]
    public void RandomNumber_ReturnsCorrectLength(int length)
    {
        // Act
        string result = _tool.RandomNumber(length);

        // Assert
        Assert.Equal(length, result.Length);
        Assert.Matches($"^[0-9]{{{length}}}$", result);
    }

    [Fact]
    public void RandomBase_IsNotNull()
    {
        // Assert
        Assert.NotNull(_tool.RandomBase);
    }
} 