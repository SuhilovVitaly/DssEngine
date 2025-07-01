using DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects;
using DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects.Spacecrafts;
using DeepSpaceSaga.Common.Geometry;

namespace DeepSpaceSaga.Common.Extensions.Entities.CelestialObjects;

public static class CelestialObjectDtoExtensions
{
    public static SpaceMapPoint GetLocation(this CelestialObjectDto celestialObject)
    {
        return new SpaceMapPoint((float)celestialObject.X, (float)celestialObject.Y);
    }
}
