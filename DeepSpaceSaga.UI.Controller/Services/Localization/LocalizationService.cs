namespace DeepSpaceSaga.UI.Controller.Services.Localization;

public class LocalizationService : ILocalizationService
{
    public ISettings GameSettings { get; set; }

    private readonly Dictionary<string, Dictionary<int, Text>> _texts;

    public LocalizationService(ISettings settings, string folderPath = "Localization")
    {
        GameSettings = settings;

        if (string.IsNullOrWhiteSpace(folderPath))
            throw new ArgumentNullException(nameof(folderPath));

        string localizationFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", folderPath);

        _texts = Load(localizationFolderPath);
    }

    public string GetText(string textkey)
    {
        var message = _texts.TryGetValue(textkey, out var textLanguages)
            && textLanguages.TryGetValue(GameSettings.LanguageId, out var text)
            ? text.TextMessage : textkey;

        return message.Replace("{Environment.NewLine}", Environment.NewLine);
    }

    private Dictionary<string, Dictionary<int, Text>> Load(string folderPath)
    {
        var result = new Dictionary<string, Dictionary<int, Text>>();

        if (!Directory.Exists(folderPath))
        {
            throw new DirectoryNotFoundException($"Directory not found: {folderPath}");
        }

        var jsonFiles = Directory.GetFiles(folderPath, "*.json", SearchOption.AllDirectories);
        foreach (var jsonFile in jsonFiles)
        {
            ProcessJsonFile(jsonFile, result);
        }
        return result;
    }

    private void ProcessJsonFile(string jsonFile, Dictionary<string, Dictionary<int, Text>> result)
    {
        try
        {
            var jsonContent = File.ReadAllText(jsonFile);
            var fileData = JsonSerializer.Deserialize<JsonFileStructure>(jsonContent);

            if (fileData?.texts == null) return;

            foreach (var item in fileData.texts)
            {
                if (!result.ContainsKey(item.Key))
                {
                    result[item.Key] = new Dictionary<int, Text>();
                }

                var text = new Text
                {
                    LanguageId = fileData.LanguageId,
                    Key = item.Key,
                    TextMessage = item.TextMessage
                };
                result[item.Key][fileData.LanguageId] = text;
            }
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Error parsing file {jsonFile}: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing file {jsonFile}: {ex.Message}");
        }
    }
}
