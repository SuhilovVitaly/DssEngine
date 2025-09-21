using System.Diagnostics.CodeAnalysis;

namespace DeepSpaceSaga.Common.Implementation.Entities.Dialogs;

[ExcludeFromCodeCoverage]
public class DialogCommand
{
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Modification { get; set; } = string.Empty;
}