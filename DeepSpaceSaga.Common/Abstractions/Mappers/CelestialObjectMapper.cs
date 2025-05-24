using DeepSpaceSaga.Common.Abstractions.Dto;
using DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects;

namespace DeepSpaceSaga.Common.Abstractions.Mappers;

public static class CelestialObjectMapper
{
    public static CelestialObjectDto ToDto(ICelestialObject celestialObject)
    {
        return new CelestialObjectDto
        {
            Type = celestialObject.Type,
            IsPreScanned = celestialObject.IsPreScanned,
            X = celestialObject.X,
            Y = celestialObject.Y,
            Id = celestialObject.Id,
        };
    }
}