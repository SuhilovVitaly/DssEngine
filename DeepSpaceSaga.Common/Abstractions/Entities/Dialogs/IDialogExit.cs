using DeepSpaceSaga.Common.Implementation.Entities.Dialogs;

namespace DeepSpaceSaga.Common.Abstractions.Entities.Dialogs;

public interface IDialogExit
{
    string Key { get; set; }

    string NextKey { get; set; }

    string TextKey { get; set; }

    List<DialogCommand> DialogCommands { get; set; }
}
