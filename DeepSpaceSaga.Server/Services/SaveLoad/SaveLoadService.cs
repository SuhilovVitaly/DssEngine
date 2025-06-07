namespace DeepSpaceSaga.Server.Services.SaveLoad;

public class SaveLoadService(string savesDirectory = "Saves") : ISaveLoadService
{
    private static readonly ILog Logger = LogManager.GetLogger(typeof(SaveLoadService));
    private readonly string _savesDirectory = savesDirectory;

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
        throw new NotImplementedException();
    }

    public void Save(ISessionContextService sessionContext, string saveFileName)
    {
        throw new NotImplementedException();
    }
}
