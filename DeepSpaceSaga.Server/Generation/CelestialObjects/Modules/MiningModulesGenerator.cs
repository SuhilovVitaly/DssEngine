namespace DeepSpaceSaga.Server.Generation.Modules;

public class MiningModulesGenerator
{
    public static IModule CreateMiningLaser(IGenerationTool randomizer, int ownerId, string id)
    {
        IMiningLaser resultModule = new MiningLaser
        {
            Id = randomizer.GetId(),
            OwnerId = ownerId,
        };

        switch (id)
        {
            case "MLC8002":
                resultModule.MiningRange = 3000;
                resultModule.ActivationCost = 100;
                resultModule.Power = 2000;
                resultModule.ReloadTime = 3;
                resultModule.Reloading = 3;
                resultModule.Name = "Civilian Mining Laser";
                break;
        }
        return resultModule;
    }
}
