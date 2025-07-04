namespace DeepSpaceSaga.Common.Abstractions.Entities.Equipment.Commands;

public interface IModuleOperation
{
    int Id { get; set; }
    string Image { get; }
    CommandCategory CommandCategory { get; }
    CommandTypes Type { get; }
    bool IsOneTimeCommand { get;}
    bool IsUnique { get; }
    //void Execute(IGameManager gameManager);
    /// <summary>
    /// ModuleStatus GetStatus(IGameManager gameManager);
    /// </summary>
    string Tooltip { get; }
}
