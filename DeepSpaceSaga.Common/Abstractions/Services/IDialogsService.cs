namespace DeepSpaceSaga.Common.Abstractions.Services;

public interface IDialogsService
{
    void AddDialog(IDialog dialog);

    void AddDialogs(IList<IDialog> dialogs);

    IList<IDialog> DialogsActivation(ICommand command, ISessionContextService context);

    List<IDialog> GetConnectedDialogs(IDialog dialog);

    IDialog? GetDialog(string key);
}
