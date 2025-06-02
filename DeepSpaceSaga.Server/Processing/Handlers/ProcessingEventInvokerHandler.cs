using DeepSpaceSaga.Common.Implementation.Entities.Events;
using DeepSpaceSaga.Common.Tools;

namespace DeepSpaceSaga.Server.Processing.Handlers;

public class ProcessingEventInvokerHandler
{
    public void Execute(ISessionContextService sessionContext)
    {
        if(sessionContext.SessionInfo.Turn == 1)
        {
            var x = new GameActionEvent
            {
                Id = new GenerationTool().GetId(),
                CelestialObjectId = 1,
                ModuleId = 1,
                TargetObjectId = 1,
                TriggerCommand = null
            };

            sessionContext.GameSession.Events.TryAdd(x.Id, x);
        }        
    }
}
