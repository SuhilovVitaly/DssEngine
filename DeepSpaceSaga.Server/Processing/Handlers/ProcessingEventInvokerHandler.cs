namespace DeepSpaceSaga.Server.Processing.Handlers;

public class ProcessingEventInvokerHandler
{
    public void Execute(ISessionContextService sessionContext)
    {
        if(sessionContext.SessionInfo.Turn >= 1)
        {
            var gameActionEvent = new GameActionEvent
            {
                Id = 22,
                CelestialObjectId = 1,
                ModuleId = 1,
                TargetObjectId = 1,
                TriggerCommand = null
            };

            if(sessionContext.GameSession.FinishedEvents.Keys.Contains(gameActionEvent.Id) == false)
            {
                sessionContext.GameSession.Events.TryAdd(gameActionEvent.Id, gameActionEvent);
            }           
        }        
    }
}
