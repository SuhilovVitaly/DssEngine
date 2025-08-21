using DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects;
using DeepSpaceSaga.Common.Abstractions.Dto.Ui;

namespace DeepSpaceSaga.Common.Abstractions.Mappers.CelestialObjects;

public static class AsteroidMapper
{
    public static ICelestialObject ToGameObject(CelestialObjectSaveFormatDto celestialObjectDto)
    {
        return new BaseAsteroid(celestialObjectDto)
        {
            Id = celestialObjectDto.Id,
            OwnerId = celestialObjectDto.OwnerId,
            Name = celestialObjectDto.Name,
            Direction = celestialObjectDto.Direction,
            X = celestialObjectDto.X,
            Y = celestialObjectDto.Y,
            Speed = celestialObjectDto.Speed,
            Type = celestialObjectDto.Type,
            IsPreScanned = celestialObjectDto.IsPreScanned,
            Size = celestialObjectDto.Size
        };
    }
}
