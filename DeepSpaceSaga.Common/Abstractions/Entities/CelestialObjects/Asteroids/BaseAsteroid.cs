namespace DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects.Asteroids;

public class BaseAsteroid : BaseCelestialObject, IAsteroid
{
    public int RemainingDrillAttempts { get; set; }

    public BaseAsteroid(int maxDrillAttempts) => RemainingDrillAttempts = maxDrillAttempts;

    public BaseAsteroid(CelestialObjectDto celestialObjectDto)
    {
        LoadObject(celestialObjectDto);

        RemainingDrillAttempts = celestialObjectDto.RemainingDrillAttempts;
    }

    public void Drill()
    {
        RemainingDrillAttempts--;
    }
}
