namespace DeepSpaceSaga.Common.Implementation.Entities.Dialogs;

[ExcludeFromCodeCoverage]
public class DialogExit : IDialogExit
{
    public required string Key { get; set; }

    public required string NextKey { get; set; }

    public required string TextKey { get; set; }

    public string? DialogCommand { get; set; }
}
