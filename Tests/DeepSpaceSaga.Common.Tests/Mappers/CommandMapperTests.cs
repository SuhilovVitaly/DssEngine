namespace DeepSpaceSaga.Common.Tests.Mappers;

public class CommandMapperTests
{
    [Fact]
    public void ToDto_Should_Map_CommandId_Correctly()
    {
        // Arrange
        var commandId = Guid.NewGuid();
        var command = new DeepSpaceSaga.Common.Abstractions.Entities.Command { Id = commandId };

        // Act
        var dto = DeepSpaceSaga.Common.Abstractions.Mappers.CommandMapper.ToDto(command);

        // Assert
        dto.CommandId.Should().Be(commandId);
    }

    [Fact]
    public void ToDto_Should_Not_Return_Null()
    {
        // Arrange
        var command = new DeepSpaceSaga.Common.Abstractions.Entities.Command();

        // Act
        var dto = DeepSpaceSaga.Common.Abstractions.Mappers.CommandMapper.ToDto(command);

        // Assert
        dto.Should().NotBeNull();
    }
} 