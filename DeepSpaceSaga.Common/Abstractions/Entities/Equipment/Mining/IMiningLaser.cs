namespace DeepSpaceSaga.Common.Abstractions.Entities.Equipment.Mining;

public interface IMiningLaser : IModule
{
    double Power { get; set; }
    double MiningRange { get; set; }
    Command Harvest(int targetCelestialObjectId);
}
