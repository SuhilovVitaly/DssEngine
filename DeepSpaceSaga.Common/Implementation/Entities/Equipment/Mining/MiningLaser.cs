namespace DeepSpaceSaga.Common.Implementation.Entities.Equipment.Mining;

public class MiningLaser : AbstractModule, IModule, IMiningLaser
{
    public Category Category { get; set; } = Category.MiningLaser;
    public double ActivationCost { get; set; }
    public double Power { get; set; }
    public double MiningRange { get; set; }
    public double BasePrice { get; set; } = 2000;

    public MiningLaser()
    {
        var generator = new GenerationTool();

        Operations.Add(MiningOperationHarvest.Create(generator));
    }

    public Command Harvest(int targetCelestialObjectId)
    {
        var command = new Command
        {
            Category = CommandCategory.Mining,
            Type = CommandTypes.MiningOperationsHarvest,
            CelestialObjectId = OwnerId,
            TargetCelestialObjectId = targetCelestialObjectId,
            ModuleId = Id
        };

        return command;
    }
}
