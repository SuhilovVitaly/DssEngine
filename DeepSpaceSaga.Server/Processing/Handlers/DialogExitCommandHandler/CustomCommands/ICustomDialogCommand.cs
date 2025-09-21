namespace DeepSpaceSaga.Server.Processing.Handlers.DialogExitCommandHandler.CustomCommands;

public interface ICustomDialogCommand
{
    void Execute(ISessionContextService sessionContext, ICommand command, DialogCommand dialogCommand);
}
