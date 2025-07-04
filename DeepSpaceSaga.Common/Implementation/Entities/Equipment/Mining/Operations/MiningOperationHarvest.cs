namespace DeepSpaceSaga.Common.Implementation.Entities.Equipment.Mining.Operations;

public class MiningOperationHarvest : IModuleOperation
{
    public int Id { get; set; }

    public string Image { get; set; } = @"Data/Images/Toolbar/Operations/Mining/mining-operations-harvest";

    public CommandCategory CommandCategory { get; set; } = CommandCategory.Mining;

    public CommandTypes Type { get; set; } = CommandTypes.MiningOperationsHarvest;

    public bool IsOneTimeCommand { get; set; } = false;

    public bool IsUnique { get; set; } = true;

    public string Tooltip { get; set; } = "toolip-operation-mining-harvest";

    public MiningOperationHarvest(IGenerationTool generation)
    {
        Id = generation.GetId();
    }

    public static IModuleOperation Create(IGenerationTool generation)
    {
        return new MiningOperationHarvest(generation);
    }

    //public void Execute(IGameManager gameManager)
    //{
    //    var spacecraft = gameManager.GetPlayerSpacecraft();

    //    var command = new Command
    //    {
    //        Category = CommandCategory.Mining,
    //        Type = CommandTypes.MiningOperationsHarvest,
    //        CelestialObjectId = spacecraft.Id,
    //        TargetCelestialObjectId = gameManager.OuterSpace.SelectedObjectId,
    //        ModuleId = spacecraft.ModulesS.Module(Category.MiningLaser).Id
    //    };

    //    _ = gameManager.ExecuteCommandAsync(command);
    //}

    //public ModuleStatus GetStatus(IGameManager gameManager)
    //{       
    //    if (gameManager.OuterSpace.SelectedObjectId == 0)
    //    {
    //        return ModuleStatus.Disabled;
    //    }

    //    var target = gameManager.GetCelestialObject(gameManager.OuterSpace.SelectedObjectId);

    //    if(target is null)
    //    {
    //        return ModuleStatus.Disabled;
    //    }

    //    if (target.Types != CelestialObjectType.Asteroid)
    //    {
    //        return ModuleStatus.Disabled;
    //    }

    //    if(IsModuleTargetInRange(gameManager, gameManager.OuterSpace.SelectedObjectId) == false)
    //    {
    //        return ModuleStatus.Disabled;
    //    }

    //    // TODO: Add here direction and speed checks

    //    if(IsModuleInProgress(gameManager))
    //    {
    //        return ModuleStatus.InProgress;
    //    }

    //    return ModuleStatus.Enabled;
    //}

    //private bool IsModuleInProgress(IGameManager gameManager)
    //{
    //    var spacecraft = gameManager.GetPlayerSpacecraft();
    //    var module = spacecraft.ModulesS.Module(Category.MiningLaser) as IMiningLaser;

    //    return !module.IsReloaded;
    //}

    //private bool IsModuleTargetInRange(IGameManager gameManager, int id)
    //{
    //    var spacecraft = gameManager.GetPlayerSpacecraft();
    //    var target = gameManager.GetCelestialObject(id);

    //    if (target is null)
    //    {
    //        return false;
    //    }        

    //    var distance = GeometryTools.Distance(spacecraft.GetLocation(), target.GetLocation());

    //    // Harvest asteroid
    //    var module = spacecraft.ModulesS.Module(Category.MiningLaser) as IMiningLaser;

    //    if (distance <= module.MiningRange)
    //    {
    //        return true;
    //    }

    //    return false;
    //}
}
