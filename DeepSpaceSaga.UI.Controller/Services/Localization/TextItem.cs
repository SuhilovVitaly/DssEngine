namespace DeepSpaceSaga.UI.Controller.Services.Localization;

public class TextItem
{
    public string Key { get; set; }
    [JsonPropertyName("Text")]
    public string TextMessage { get; set; }
}
