namespace DeepSpaceSaga.Server.Processing.Handlers;

public class ProcessingDialogHandler
{
    public GameActionEventDto? Execute(ISessionContextService sessionContext, ICommand command)
    {
        AssignmentDialogExitCommand.Execute(sessionContext, command);

        IGameActionEvent gameActionEvent = GetNextEvent(sessionContext, command as DialogExitCommand);

        if (gameActionEvent is null) return null;

        return GameActionEventMapper.ToDto(gameActionEvent);
    }

    private IGameActionEvent? GetNextEvent(ISessionContextService sessionContext, DialogExitCommand dialogCommand)
    {
        if (dialogCommand == null) return null;
        if (dialogCommand.Exit == null) return null;

        var dialog = sessionContext.GameSession.Dialogs.GetDialog(dialogCommand.Exit.NextKey);

        if(dialog == null) return null; 

        IGameActionEvent gameActionEvent = new GameActionEvent
        {
            Key = dialog.Key,
            Dialog = dialog,
            Type = dialog.Type,
            ConnectedDialogs = sessionContext.GameSession.Dialogs.GetConnectedDialogs(dialog),
        };

        return gameActionEvent;
    }

    public void Execute(ISessionContextService sessionContext)
    {
        var removeCommands = new ConcurrentDictionary<string, Guid>();

        foreach (var command in sessionContext.GameSession.Commands.Values)
        {
            if (command.Category == Common.Abstractions.Entities.Commands.CommandCategory.DialogExit)
            {
                var dialogCommand = command as DialogExitCommand;

                if (dialogCommand == null) continue;

                AssignmentDialogExitCommand.Execute(sessionContext, command);

                removeCommands.TryAdd(command.Id.ToString(), command.Id);
                sessionContext.GameSession.DialogsExits.TryAdd(dialogCommand.DialogKey, dialogCommand.DialogExitKey);

                InvokeNextDialog(sessionContext, dialogCommand);
            }
        }

        // Use write lock for modifying session data
        sessionContext.EnterWriteLock();
        try
        {
            foreach (var commands in removeCommands)
            {
                sessionContext.GameSession.Commands.TryRemove(commands.Value, out _);
            }
        }
        finally
        {
            sessionContext.ExitWriteLock();
        }
    }

    private void InvokeNextDialog(ISessionContextService sessionContext, DialogExitCommand dialogExitCommand)
    {
        IGameActionEvent gameActionEvent = GetNextEvent(sessionContext, dialogExitCommand);

        if (gameActionEvent is null) return;

        if (IsNewEvent(sessionContext.GameSession, gameActionEvent.Key))
        {
            sessionContext.GameSession.ActiveEvents.TryAdd(gameActionEvent.Key, gameActionEvent);
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
