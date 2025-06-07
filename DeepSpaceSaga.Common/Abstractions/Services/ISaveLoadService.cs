namespace DeepSpaceSaga.Common.Abstractions.Services;

public interface ISaveLoadService
{
    void Save(ISessionContextService sessionContext, string saveFileName);
    ISessionContextService Load(string saveFileName);
    void DeleteSave(string saveFileName);   
    List<string> GetAllSaves();
}
