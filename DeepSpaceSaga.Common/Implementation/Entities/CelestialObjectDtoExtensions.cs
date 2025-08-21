using DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects;
using DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects.Spacecrafts;
using DeepSpaceSaga.Common.Geometry;

namespace DeepSpaceSaga.Common.Extensions.Entities.CelestialObjects;

public static class CelestialObjectDtoExtensions
{
    public static SpaceMapPoint GetLocation(this CelestialObjectSaveFormatDto celestialObject)
    {
        return new SpaceMapPoint((float)celestialObject.X, (float)celestialObject.Y);
    }

    public static ISpacecraft ToSpaceship(this CelestialObjectSaveFormatDto celestialObject)
    {
        var spacecraft = celestialObject as ISpacecraft;

        return spacecraft;
    }
}
