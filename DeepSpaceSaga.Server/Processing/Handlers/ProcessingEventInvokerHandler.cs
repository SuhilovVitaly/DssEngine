using DeepSpaceSaga.Common.Abstractions.Entities;
using DeepSpaceSaga.Common.Implementation.Entities.Events;
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

        var dialogs = sessionContext.GameSession.Dialogs.DialogsActivation(turnCommand, sessionContext);

        foreach (var dialog in dialogs)
        {
            var gameActionEvent = new GameActionEvent
            {
                Key = dialog.Key,
                Dialog = dialog,
                ConnectedDialogs = sessionContext.GameSession.Dialogs.GetConnectedDialogs(dialog)
            };

            if (IsNewEvent(sessionContext.GameSession, gameActionEvent.Key) )
            {
                sessionContext.GameSession.ActiveEvents.TryAdd(gameActionEvent.Key, gameActionEvent);
            }
        }
    }

    private static bool IsNewEvent(GameSession gameSession, string eventKey)
    {
        if (gameSession.ActiveEvents.Keys.Contains(eventKey))
        {
            return false;
        }

        if (gameSession.FinishedEvents.Keys.Contains(eventKey))
        {
            return false;
        }

        return true;
    }
}
