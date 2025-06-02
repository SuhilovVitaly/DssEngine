namespace DeepSpaceSaga.Common.Abstractions.Mappers;

public static class CommandMapper
{
    public static CommandDto ToDto(ICommand command)
    {
        return new CommandDto
        {
            CommandId = command.Id,
        };
    }
}