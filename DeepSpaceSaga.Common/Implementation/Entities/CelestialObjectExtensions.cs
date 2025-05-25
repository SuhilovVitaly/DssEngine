using DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects;
using DeepSpaceSaga.Common.Geometry;

namespace DeepSpaceSaga.Common.Extensions.Entities.CelestialObjects;

public static class CelestialObjectExtensions
{
    public static SpaceMapPoint GetLocation(this ICelestialObject celestialObject)
    {
        return new SpaceMapPoint((float)celestialObject.X, (float)celestialObject.Y);
    }

    //public static ISpacecraft ToSpaceship(this ICelestialObject celestialObject)
    //{
    //    return celestialObject as ISpacecraft;
    //}
}
