namespace DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects.Asteroids;

public interface IAsteroid : ICelestialObject
{
    int RemainingDrillAttempts { get; set; }
    void Drill();
}
