using DeepSpaceSaga.Common.Abstractions.Entities.Commands;

namespace DeepSpaceSaga.Server.Processing.Handlers;

public class ProcessingEventAcknowledgeHandler
{
    public void Execute(ISessionContextService sessionContext)
    {
        var acknowledgedEvents = new ConcurrentDictionary<long, ICommand>();

        foreach (var command in sessionContext.GameSession.Commands.Values)
        {
            if(command.Category == Common.Abstractions.Entities.Commands.CommandCategory.CommandAccept)
            {
                acknowledgedEvents.TryAdd(command.CelestialObjectId, command);
            }            
        }

        lock(sessionContext)
        {
            foreach (var acknowledgedEvent in acknowledgedEvents)
            {
                sessionContext.GameSession.FinishedEvents.TryAdd(acknowledgedEvent.Key, acknowledgedEvent.Key);

                sessionContext.GameSession.Events.TryRemove(acknowledgedEvent.Key, out _);

                sessionContext.GameSession.Commands.TryRemove(acknowledgedEvent.Value.Id, out _);
            }
        }        
    }
}
