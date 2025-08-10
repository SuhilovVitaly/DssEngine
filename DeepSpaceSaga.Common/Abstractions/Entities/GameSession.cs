namespace DeepSpaceSaga.Common.Abstractions.Entities;

public class GameSession
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public ConcurrentDictionary<int, ICelestialObject> CelestialObjects { get; set; } = new();
    public ConcurrentDictionary<Guid, ICommand> Commands { get; set; } = new();
    public ConcurrentDictionary<string, IGameActionEvent> ActiveEvents { get; set; } = new();
    public ConcurrentDictionary<string, string> FinishedEvents { get; set; } = new();

    public IDialogsService Dialogs { get; set; }

    public event EventHandler? Changed;

    private void OnChanged()
    {
        Changed?.Invoke(this, EventArgs.Empty);
    }

    public async Task AddCommand(ICommand command)
    {
        if (!Commands.TryAdd(command.Id, command))
        {
            throw new ArgumentException($"Command with ID {command.Id} already exists.", nameof(command));
        }

        await Task.CompletedTask;
    }
    
    public void RemoveCommand(Guid commandId)
    {
        Commands.TryRemove(commandId, out _);
        OnChanged();
    }
}