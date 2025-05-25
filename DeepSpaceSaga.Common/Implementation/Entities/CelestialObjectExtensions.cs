using DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects;
using DeepSpaceSaga.Common.Geometry;

namespace DeepSpaceSaga.Common.Extensions.Entities.CelestialObjects;

public static class CelestialObjectExtensions
{
    public static SpaceMapPoint GetLocation(this ICelestialObject celestialObject)
    {
        return new SpaceMapPoint((float)celestialObject.X, (float)celestialObject.Y);
    }

    public static SpaceMapColor GetColor(this ICelestialObject celestialObject)
    {
        switch (celestialObject.Type)
        {
            case CelestialObjectType.Missile:
                break;
            case CelestialObjectType.SpaceshipPlayer:
                return new SpaceMapColor(Color.DarkOliveGreen);
            case CelestialObjectType.SpaceshipNpcNeutral:
                return new SpaceMapColor(Color.DarkGray);
            case CelestialObjectType.SpaceshipNpcEnemy:
                return new SpaceMapColor(Color.DarkRed);
            case CelestialObjectType.SpaceshipNpcFriend:
                return new SpaceMapColor(Color.SeaGreen);
            case CelestialObjectType.Asteroid:
                return new SpaceMapColor(Color.WhiteSmoke);
            case CelestialObjectType.Container:
                return new SpaceMapColor(Color.Gray);
            case CelestialObjectType.Explosion:
                break;
            case CelestialObjectType.Station:
                return new SpaceMapColor(Color.Orange);
        }

        return new SpaceMapColor(Color.FromArgb(30, 45, 65));
    }

    //public static ISpacecraft ToSpaceship(this ICelestialObject celestialObject)
    //{
    //    return celestialObject as ISpacecraft;
    //}
}
