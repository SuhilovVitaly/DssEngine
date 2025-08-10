using DeepSpaceSaga.Common.Abstractions.Entities.Commands;

namespace DeepSpaceSaga.Server.Processing.Handlers;

public class ProcessingEventAcknowledgeHandler
{
    public void Execute(ISessionContextService sessionContext)
    {
        var acknowledgedEvents = new ConcurrentDictionary<string, ICommand>();

        foreach (var command in sessionContext.GameSession.Commands.Values)
        {
            if(command.Category == Common.Abstractions.Entities.Commands.CommandCategory.CommandAccept)
            {
                acknowledgedEvents.TryAdd(command.Id.ToString(), command);
            }            
        }

        lock(sessionContext)
        {
            foreach (var acknowledgedEvent in acknowledgedEvents)
            {
                sessionContext.GameSession.FinishedEvents.TryAdd(acknowledgedEvent.Key, acknowledgedEvent.Key);

                sessionContext.GameSession.ActiveEvents.TryRemove(acknowledgedEvent.Key, out _);

                sessionContext.GameSession.Commands.TryRemove(acknowledgedEvent.Value.Id, out _);
            }
        }        
    }
}
