using DeepSpaceSaga.Common.Tools;

namespace DeepSpaceSaga.Server.Processing.Handlers;

public class ProcessingEventInvokerHandler
{
    public void Execute(ISessionContextService sessionContext)
    {
        // Activation dialogs by finished turn
        var turnCommand = new Command
        {
            Id = Guid.NewGuid(),
            Type = CommandTypes.DialogInitiationByTurn
        };

        foreach (var dialog in sessionContext.GameSession.Dialogs.DialogsActivation(turnCommand, sessionContext))
        {
            var gameActionEvent = new GameActionEvent
            {
                Dialog = dialog,
                ConnectedDialogs = sessionContext.GameSession.Dialogs.GetConnectedDialogs(dialog)
            };

            if (sessionContext.GameSession.FinishedEvents.Keys.Contains(gameActionEvent.Key) == false)
            {
                sessionContext.GameSession.ActiveEvents.TryAdd(gameActionEvent.Key, gameActionEvent);
            }
        }
    }
}
