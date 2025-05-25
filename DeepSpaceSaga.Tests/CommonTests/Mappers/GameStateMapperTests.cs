namespace DeepSpaceSaga.Tests.CommonTests.Mappers;

public class GameStateMapperTests
{
    [Fact]
    public void ToDto_Should_Map_All_Properties_Correctly()
    {
        // Arrange
        var sessionInfoMock = new Mock<ISessionInfoService>();
        sessionInfoMock.Setup(x => x.Turn).Returns(100);
        sessionInfoMock.Setup(x => x.CycleCounter).Returns(5);
        sessionInfoMock.Setup(x => x.TurnCounter).Returns(25);
        sessionInfoMock.Setup(x => x.TickCounter).Returns(75);
        sessionInfoMock.Setup(x => x.IsPaused).Returns(false);
        sessionInfoMock.Setup(x => x.Speed).Returns(3);

        var sessionContextMock = new Mock<ISessionContextService>();
        sessionContextMock.Setup(x => x.SessionInfo).Returns(sessionInfoMock.Object);

        // Act
        var result = GameStateMapper.ToDto(sessionContextMock.Object);

        // Assert
        result.Should().NotBeNull();
        result.ProcessedTurns.Should().Be(100);
        result.Cycle.Should().Be(5);
        result.Turn.Should().Be(25);
        result.Tick.Should().Be(75);
        result.IsPaused.Should().Be(false);
        result.Speed.Should().Be(3);
    }

    [Fact]
    public void ToDto_Should_Map_Default_Values_Correctly()
    {
        // Arrange
        var sessionInfoMock = new Mock<ISessionInfoService>();
        sessionInfoMock.Setup(x => x.Turn).Returns(0);
        sessionInfoMock.Setup(x => x.CycleCounter).Returns(0);
        sessionInfoMock.Setup(x => x.TurnCounter).Returns(0);
        sessionInfoMock.Setup(x => x.TickCounter).Returns(0);
        sessionInfoMock.Setup(x => x.IsPaused).Returns(true);
        sessionInfoMock.Setup(x => x.Speed).Returns(1);

        var sessionContextMock = new Mock<ISessionContextService>();
        sessionContextMock.Setup(x => x.SessionInfo).Returns(sessionInfoMock.Object);

        // Act
        var result = GameStateMapper.ToDto(sessionContextMock.Object);

        // Assert
        result.Should().NotBeNull();
        result.ProcessedTurns.Should().Be(0);
        result.Cycle.Should().Be(0);
        result.Turn.Should().Be(0);
        result.Tick.Should().Be(0);
        result.IsPaused.Should().Be(true);
        result.Speed.Should().Be(1);
    }

    [Fact]
    public void ToDto_Should_Handle_Large_Numbers_Correctly()
    {
        // Arrange
        var sessionInfoMock = new Mock<ISessionInfoService>();
        sessionInfoMock.Setup(x => x.Turn).Returns(int.MaxValue);
        sessionInfoMock.Setup(x => x.CycleCounter).Returns(999999);
        sessionInfoMock.Setup(x => x.TurnCounter).Returns(888888);
        sessionInfoMock.Setup(x => x.TickCounter).Returns(777777);
        sessionInfoMock.Setup(x => x.IsPaused).Returns(false);
        sessionInfoMock.Setup(x => x.Speed).Returns(10);

        var sessionContextMock = new Mock<ISessionContextService>();
        sessionContextMock.Setup(x => x.SessionInfo).Returns(sessionInfoMock.Object);

        // Act
        var result = GameStateMapper.ToDto(sessionContextMock.Object);

        // Assert
        result.Should().NotBeNull();
        result.ProcessedTurns.Should().Be(int.MaxValue);
        result.Cycle.Should().Be(999999);
        result.Turn.Should().Be(888888);
        result.Tick.Should().Be(777777);
        result.IsPaused.Should().Be(false);
        result.Speed.Should().Be(10);
    }

    [Theory]
    [InlineData(true, 1)]
    [InlineData(false, 5)]
    [InlineData(true, 10)]
    [InlineData(false, 2)]
    public void ToDto_Should_Map_Different_Pause_And_Speed_Combinations(bool isPaused, int speed)
    {
        // Arrange
        var sessionInfoMock = new Mock<ISessionInfoService>();
        sessionInfoMock.Setup(x => x.Turn).Returns(50);
        sessionInfoMock.Setup(x => x.CycleCounter).Returns(2);
        sessionInfoMock.Setup(x => x.TurnCounter).Returns(10);
        sessionInfoMock.Setup(x => x.TickCounter).Returns(30);
        sessionInfoMock.Setup(x => x.IsPaused).Returns(isPaused);
        sessionInfoMock.Setup(x => x.Speed).Returns(speed);

        var sessionContextMock = new Mock<ISessionContextService>();
        sessionContextMock.Setup(x => x.SessionInfo).Returns(sessionInfoMock.Object);

        // Act
        var result = GameStateMapper.ToDto(sessionContextMock.Object);

        // Assert
        result.Should().NotBeNull();
        result.IsPaused.Should().Be(isPaused);
        result.Speed.Should().Be(speed);
    }

    [Fact]
    public void ToDto_Should_Not_Return_Null()
    {
        // Arrange
        var sessionInfoMock = new Mock<ISessionInfoService>();
        var sessionContextMock = new Mock<ISessionContextService>();
        sessionContextMock.Setup(x => x.SessionInfo).Returns(sessionInfoMock.Object);

        // Act
        var result = GameStateMapper.ToDto(sessionContextMock.Object);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void ToDto_Should_Create_New_Instance_Each_Time()
    {
        // Arrange
        var sessionInfoMock = new Mock<ISessionInfoService>();
        var sessionContextMock = new Mock<ISessionContextService>();
        sessionContextMock.Setup(x => x.SessionInfo).Returns(sessionInfoMock.Object);

        // Act
        var result1 = GameStateMapper.ToDto(sessionContextMock.Object);
        var result2 = GameStateMapper.ToDto(sessionContextMock.Object);

        // Assert
        result1.Should().NotBeNull();
        result2.Should().NotBeNull();
        result1.Should().NotBeSameAs(result2);
    }

    [Fact]
    public void ToDto_Should_Handle_Negative_Values_Correctly()
    {
        // Arrange
        var sessionInfoMock = new Mock<ISessionInfoService>();
        sessionInfoMock.Setup(x => x.Turn).Returns(-1);
        sessionInfoMock.Setup(x => x.CycleCounter).Returns(-5);
        sessionInfoMock.Setup(x => x.TurnCounter).Returns(-10);
        sessionInfoMock.Setup(x => x.TickCounter).Returns(-20);
        sessionInfoMock.Setup(x => x.IsPaused).Returns(true);
        sessionInfoMock.Setup(x => x.Speed).Returns(-2);

        var sessionContextMock = new Mock<ISessionContextService>();
        sessionContextMock.Setup(x => x.SessionInfo).Returns(sessionInfoMock.Object);

        // Act
        var result = GameStateMapper.ToDto(sessionContextMock.Object);

        // Assert
        result.Should().NotBeNull();
        result.ProcessedTurns.Should().Be(-1);
        result.Cycle.Should().Be(-5);
        result.Turn.Should().Be(-10);
        result.Tick.Should().Be(-20);
        result.IsPaused.Should().Be(true);
        result.Speed.Should().Be(-2);
    }
} 