namespace DeepSpaceSaga.Tests.ServerTests;

public class LocalGameServerTests
{
    private readonly LocalGameServer _sut;

    public LocalGameServerTests()
    {
        _sut = new LocalGameServer();
    }

    [Fact]
    public void TurnCalculation_ShouldIncrementTurnCounter()
    {
        // Arrange
        var firstExpectedTurn = 1;
        var secondExpectedTurn = 2;

        // Act
        var firstResult = _sut.TurnCalculation(CalculationType.Tick);
        var secondResult = _sut.TurnCalculation(CalculationType.Tick);

        // Assert
        Assert.Equal(firstExpectedTurn, firstResult.Turn);
        Assert.Equal(secondExpectedTurn, secondResult.Turn);
    }

    [Theory]
    [InlineData(CalculationType.Tick)]
    [InlineData(CalculationType.Turn)]
    [InlineData(CalculationType.Cycle)]
    public void TurnCalculation_ShouldWorkWithAllCalculationTypes(CalculationType type)
    {
        // Arrange
        var expectedTurn = 1;

        // Act
        var result = _sut.TurnCalculation(type);

        // Assert
        Assert.Equal(expectedTurn, result.Turn);
        Assert.NotEqual(Guid.Empty, result.Id);
    }

    [Fact]
    public void TurnCalculation_ShouldGenerateUniqueIds()
    {
        // Arrange
        var numberOfCalls = 3;
        var ids = new HashSet<Guid>();

        // Act
        for (int i = 0; i < numberOfCalls; i++)
        {
            var result = _sut.TurnCalculation(CalculationType.Tick);
            ids.Add(result.Id);
        }

        // Assert
        Assert.Equal(numberOfCalls, ids.Count);
    }

    [Fact]
    public void TurnCalculation_ShouldReturnValidGameSession()
    {
        // Act
        var result = _sut.TurnCalculation(CalculationType.Tick);

        // Assert
        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(1, result.Turn);
        Assert.NotNull(result.SpaceMap);
    }
} 