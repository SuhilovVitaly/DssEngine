using DeepSpaceSaga.Common.Implementation.Entities.Equipment.Mining;

namespace DeepSpaceSaga.Common.Generation.CelestialObjects.Modules;

public class MiningModulesGenerator
{
    public static IModule CreateMiningLaser(IGenerationTool randomizer, long ownerId, string code)
    {
        IMiningLaser resultModule = new MiningLaser
        {
            Id = randomizer.GetId(),
            OwnerId = ownerId,
            Code = code
        };

        switch (code)
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
