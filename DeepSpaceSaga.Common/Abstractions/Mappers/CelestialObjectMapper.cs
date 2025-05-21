namespace DeepSpaceSaga.Common.Abstractions.Mappers;

public static class CelestialObjectMapper
{
    public static CelestialObjectDto ToDto(CelestialObject celestialObject)
    {
        return new CelestialObjectDto
        {
            Type = celestialObject.Type,
            IsPreScanned = celestialObject.IsPreScanned,
            X = celestialObject.X,
            Y = celestialObject.Y,
            CelestialObjectId = celestialObject.CelestialObjectId,
        };
    }
}