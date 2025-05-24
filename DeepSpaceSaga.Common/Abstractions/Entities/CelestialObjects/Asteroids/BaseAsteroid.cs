namespace DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects.Asteroids;

public class BaseAsteroid(int maxDrillAttempts) : BaseCelestialObject, IAsteroid
{
    public int RemainingDrillAttempts { get; private set; } = maxDrillAttempts;

    public void Drill()
    {
        RemainingDrillAttempts--;
    }
}
