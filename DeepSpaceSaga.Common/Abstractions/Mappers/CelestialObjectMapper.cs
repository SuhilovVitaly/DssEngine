using DeepSpaceSaga.Common.Abstractions.Dto.Ui;
using DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects;
using DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects.Asteroids;
using DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects.Spacecrafts;
using DeepSpaceSaga.Common.Abstractions.Mappers.CelestialObjects;

namespace DeepSpaceSaga.Common.Abstractions.Mappers;

public static class CelestialObjectMapper
{
    // TODO: Generator in Server
    public static ICelestialObject ToGameObject(CelestialObjectSaveFormatDto celestialObjectDto)
    {
        switch (celestialObjectDto.Type)
        {
            case CelestialObjectType.Unknown:
                break;
            case CelestialObjectType.PointInMap:
                break;
            case CelestialObjectType.Asteroid:
                return new BaseAsteroid(celestialObjectDto) as ICelestialObject;
            case CelestialObjectType.Station:
                break;
            case CelestialObjectType.SpaceshipPlayer:
                return new BaseSpaceship(celestialObjectDto) as ICelestialObject;
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

        return null;
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

    public static CelestialObjectSaveFormatDto ToSaveFormat(ICelestialObject celestialObject)
    {
        var baseCelestialObject = new CelestialObjectSaveFormatDto
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

        switch (celestialObject.Type)
        {
            case CelestialObjectType.Unknown:
                break;
            case CelestialObjectType.PointInMap:
                break;
            case CelestialObjectType.Asteroid:
                baseCelestialObject.RemainingDrillAttempts = ((IAsteroid)celestialObject).RemainingDrillAttempts;
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

        return baseCelestialObject;
    }
}