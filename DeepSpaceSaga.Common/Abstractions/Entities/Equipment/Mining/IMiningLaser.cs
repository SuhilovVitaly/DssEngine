namespace DeepSpaceSaga.Common.Abstractions.Entities.Equipment.Mining;

public interface IMiningLaser : IModule
{    
    double MiningRange { get; set; }
    Command Harvest(int targetCelestialObjectId);
}
