namespace DeepSpaceSaga.Server.Processing.Handlers.DialogExitCommandHandler.CustomCommands;

public class AddCharacterCommand : ICustomDialogCommand
{
    public void Execute(ISessionContextService sessionContext, ICommand command, DialogCommand dialogCommand)
    {
        ICharacter character;

        try
        {
            // Load CrewMember from file in Data\Scenarios\Default\Characters\ folder
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string characterFilePath = Path.Combine(baseDirectory, "Data", "Scenarios", "Default", "Characters", dialogCommand.Value);
            
            if (!File.Exists(characterFilePath))
            {
                throw new FileNotFoundException($"Character file not found: {characterFilePath}");
            }

            var jsonContent = File.ReadAllText(characterFilePath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            };
            character = JsonSerializer.Deserialize<CrewMember>(jsonContent, options);
        }
        catch (Exception ex)
        {
            throw;
        }

        command.Status = CommandStatus.Finalizing;
    }
}
