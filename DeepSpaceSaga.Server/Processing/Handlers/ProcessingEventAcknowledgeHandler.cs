namespace DeepSpaceSaga.Server.Processing.Handlers;

public class ProcessingEventAcknowledgeHandler
{
    public void Execute(ISessionContextService sessionContext)
    {
        var acknowledgedEvents = new ConcurrentDictionary<string, ICommand>();

        foreach (var command in sessionContext.GameSession.Commands.Values)
        {
            if(command.Category == CommandCategory.CommandAccept)
            {
                sessionContext.Metrics.Add(MetricsServer.ProcessingEventAcknowledgeReceived);
                
                acknowledgedEvents.TryAdd(command.Id.ToString(), command);
                
                sessionContext.Metrics.Add(MetricsServer.ProcessingEventAcknowledgeProcessed);
            }            
        }

        lock(sessionContext)
        {
            foreach (var acknowledgedEvent in acknowledgedEvents)
            {
                sessionContext.GameSession.FinishedEvents.TryAdd(acknowledgedEvent.Key, acknowledgedEvent.Key);

                sessionContext.GameSession.ActiveEvents.TryRemove(acknowledgedEvent.Key, out _);

                sessionContext.GameSession.Commands.TryRemove(acknowledgedEvent.Value.Id, out _);
                
                sessionContext.Metrics.Add(MetricsServer.ProcessingEventAcknowledgeRemoved);
            }
        }        
    }
}
