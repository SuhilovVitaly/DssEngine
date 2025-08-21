namespace DeepSpaceSaga.Common.Abstractions.Mappers;

public static class CelestialObjectMapper
{
    public static ICelestialObject ToGameObject(CelestialObjectDto celestialObjectDto)
    {
        var modules = celestialObjectDto.Modules.Select(module => ModuleMapper.ToGameObject(module)).ToList();
        ICelestialObject celestialObject = null;

        switch (celestialObjectDto.Type)
        {
            case CelestialObjectType.Unknown:
                break;
            case CelestialObjectType.PointInMap:
                break;
            case CelestialObjectType.Asteroid:
                celestialObject = new BaseAsteroid(celestialObjectDto) as ICelestialObject;
                break;
            case CelestialObjectType.Station:
                break;
            case CelestialObjectType.SpaceshipPlayer:
                celestialObject = new BaseSpaceship(celestialObjectDto) as ICelestialObject;
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

        var modulesSystem = new ModularSystem();
        
        foreach (var module in modules)
        {
            modulesSystem.Add(module);
        }

        celestialObject.ModulesS = modulesSystem;

        return celestialObject;
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

    public static CelestialObjectDto ToSaveFormat(ICelestialObject celestialObject)
    {
        var baseCelestialObject = new CelestialObjectDto
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

        baseCelestialObject.Modules = MapCelestialObjectModules(celestialObject.ModulesS.GetAll());

        return baseCelestialObject;
    }

    private static List<ModuleDto> MapCelestialObjectModules(List<IModule> modules)
    {
        return modules.Select(module => ModuleMapper.ToDto(module)).ToList();
    }
}