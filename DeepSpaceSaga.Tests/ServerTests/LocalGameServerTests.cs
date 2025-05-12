namespace DeepSpaceSaga.Tests.ServerTests;

public class LocalGameServerTests
{
    private readonly LocalGameServer _sut;

    public LocalGameServerTests()
    {
        // Initialize the logger
        TestLoggerRepository.Initialize();
        _sut = new LocalGameServer();
    }

    [Fact]
    public void TurnCalculation_FirstCall_ReturnsTurn1()
    {
        // Arrange
        var calculationType = CalculationType.Tick; // Using the real Tick value instead of Standard

        // Act
        var result = _sut.TurnCalculation(calculationType);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Turn);
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.NotNull(result.SpaceMap); // Check if SpaceMap is initialized
    }

    [Fact]
    public void TurnCalculation_MultipleCalls_IncrementsTurnCorrectly()
    {
        // Arrange
        var calculationType = CalculationType.Turn; // Using the real Turn value instead of Standard
        var numberOfTurns = 5;
        GameSessionDTO? lastResult = null;

        // Act
        for (int i = 0; i < numberOfTurns; i++)
        {
            lastResult = _sut.TurnCalculation(calculationType);
        }

        // Assert
        Assert.NotNull(lastResult);
        Assert.Equal(numberOfTurns, lastResult.Turn);
    }

    [Fact]
    public async Task TurnCalculation_ConcurrentCalls_IncrementsTurnSafely()
    {
        // Arrange
        var calculationType = CalculationType.Cycle; // Using the real Cycle value instead of Standard
        var numberOfConcurrentCalls = 100;
        var tasks = new List<Task<GameSessionDTO>>();

        // Act
        for (int i = 0; i < numberOfConcurrentCalls; i++)
        {
            tasks.Add(Task.Run(() => _sut.TurnCalculation(calculationType)));
        }

        var results = await Task.WhenAll(tasks);

        // Assert
        // Check that all turn numbers from 1 to numberOfConcurrentCalls are present exactly once
        var turnNumbers = results.Select(r => r.Turn).OrderBy(t => t).ToList();
        Assert.Equal(numberOfConcurrentCalls, turnNumbers.Count); // Ensure we got all results
        Assert.Equal(Enumerable.Range(1, numberOfConcurrentCalls), turnNumbers); // Check if turns are 1, 2, 3... N

        // Verify the final turn number by calling one more time (optional, but good check)
        var finalTurn = _sut.TurnCalculation(calculationType);
        Assert.Equal(numberOfConcurrentCalls + 1, finalTurn.Turn);
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