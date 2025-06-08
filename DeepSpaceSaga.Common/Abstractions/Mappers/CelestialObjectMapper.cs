using DeepSpaceSaga.Common.Abstractions.Dto;
using DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects;

namespace DeepSpaceSaga.Common.Abstractions.Mappers;

public static class CelestialObjectMapper
{
    // TODO: Generator in Server
    public static ICelestialObject ToGameObject(CelestialObjectDto celestialObjectDto)
    {
        switch (celestialObjectDto.Type)
        {
            case CelestialObjectType.Unknown:
                break;
            case CelestialObjectType.PointInMap:
                break;
            case CelestialObjectType.Asteroid:
                break;
            case CelestialObjectType.Station:
                break;
            case CelestialObjectType.SpaceshipPlayer:
                break;
            case CelestialObjectType.SpaceshipNpcNeutral:
                break;
            case CelestialObjectType.SpaceshipNpcEnemy:
                break;
            case CelestialObjectType.SpaceshipNpcFriend:
                break;
            case CelestialObjectType.Missile:
                break;
            case CelestialObjectType.Explosion:
                break;
            case CelestialObjectType.Container:
                break;
            default:
                break;
        }

        return AsteroidGenerator.;
    }

    public static CelestialObjectDto ToDto(ICelestialObject celestialObject)
    {
        return new CelestialObjectDto
        {
            Type = celestialObject.Type,
            IsPreScanned = celestialObject.IsPreScanned,
            X = celestialObject.X,
            Y = celestialObject.Y,
            Id = celestialObject.Id,
            Name = celestialObject.Name,
            Direction = celestialObject.Direction,
            OwnerId = celestialObject.OwnerId,  
            Size = celestialObject.Size,
            Speed = celestialObject.Speed
        };
    }
}