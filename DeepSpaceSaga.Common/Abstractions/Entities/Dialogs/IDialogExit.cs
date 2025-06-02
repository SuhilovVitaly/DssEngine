namespace DeepSpaceSaga.Common.Abstractions.Entities.Dialogs;

public interface IDialogExit
{
    string Key { get; set; }

    string NextKey { get; set; }

    string TextKey { get; set; }
}
