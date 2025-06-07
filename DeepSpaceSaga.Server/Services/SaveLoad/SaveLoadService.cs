namespace DeepSpaceSaga.Server.Services.SaveLoad;

public class SaveLoadService(string savesDirectory = "Saves") : ISaveLoadService
{
    private static readonly ILog Logger = LogManager.GetLogger(typeof(SaveLoadService));
    private readonly string _savesDirectory = savesDirectory;
    private readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

public void DeleteSave(string saveFileName)
    {
        throw new NotImplementedException();
    }

    public List<string> GetAllSaves()
    {
        throw new NotImplementedException();
    }

    public ISessionContextService Load(string saveFileName)
    {
        var filePath = Path.Combine(_savesDirectory, $"{saveFileName}.json");

        var savedFileContent = File.ReadAllText(filePath);
        var snapshotAfterLoad = JsonSerializer.Deserialize<GameSnapshot>(savedFileContent, jsonOptions);

        ISessionContextService loadedSessionContext = new SessionContextService(
            new SessionInfoService(
                snapshotAfterLoad.Session.State.Cycle,
                snapshotAfterLoad.Session.State.Turn,
                snapshotAfterLoad.Session.State.Tick,
                snapshotAfterLoad.Session.State.ProcessedTurns),
            new MetricsService(),
            new GenerationTool()
            );

        return loadedSessionContext;
    }

    public void Save(ISessionContextService sessionContext, string saveFileName)
    {
        var snapshot = new GameSnapshot
        {
            Session = GameSessionMapper.ToDto(sessionContext),
        };

        // Create saves directory if it doesn't exist
        Directory.CreateDirectory(_savesDirectory);

        // Create full file path
        var filePath = Path.Combine(_savesDirectory, $"{saveFileName}.json");

        try
        {
            var jsonString = JsonSerializer.Serialize(snapshot, jsonOptions);

            // Write to file
            File.WriteAllText(filePath, jsonString);

            Logger.Info($"Game saved successfully to: {filePath}");
        }
        catch (Exception ex)
        {
            Logger.Error($"Failed to save game to {filePath}: {ex.Message}", ex);
            throw;
        }
    }
}
