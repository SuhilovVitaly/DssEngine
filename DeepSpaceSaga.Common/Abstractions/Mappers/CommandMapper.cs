namespace DeepSpaceSaga.Common.Abstractions.Mappers;

public static class CommandMapper
{
    public static CommandDto ToDto(Command command)
    {
        return new CommandDto
        {
            CommandId = command.CommandId,
        };
    }
}