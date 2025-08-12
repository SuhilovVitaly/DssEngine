namespace DeepSpaceSaga.UI.Controller.Services.Localization;

[ExcludeFromCodeCoverage]
public class TextItem
{
    public string Key { get; set; }
    [JsonPropertyName("Text")]
    public string TextMessage { get; set; }
}
