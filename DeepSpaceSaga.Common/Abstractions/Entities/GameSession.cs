namespace DeepSpaceSaga.Common.Abstractions.Entities;

public class GameSession
{
    public GameSession()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; set; }
    public Dictionary<int, ICelestialObject> CelestialObjects { get; set; } = new();
    public Dictionary<Guid, ICommand> Commands { get; set; } = new();
    public event EventHandler? Changed;
    
    private readonly object _lock = new();
    private Guid _sessionId;
    private int _turn;

    public Guid SessionId
    {
        get { lock (_lock) { return _sessionId; } }
        set { lock (_lock) { _sessionId = value; } }
    }

    public int Turn
    {
        get { lock (_lock) { return _turn; } }
        set { lock (_lock) { _turn = value; } }
    }

    private void OnChanged()
    {
        Changed?.Invoke(this, EventArgs.Empty);
    }

    public void AddCommand(Command command)
    {
        lock (_lock)
        {
            Commands.Add(command.Id, command);
        }
        OnChanged();
    }
    
    public void RemoveCommand(Guid commandId)
    {
        lock (_lock)
        {
            Commands.Remove(commandId);
        }
        OnChanged();
    }
}