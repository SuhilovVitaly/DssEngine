using DeepSpaceSaga.Server.Processing.Handlers.DialogExitCommandHandler;

namespace DeepSpaceSaga.Server.Processing.Handlers;

public class ProcessingDialogHandler
{
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
}
