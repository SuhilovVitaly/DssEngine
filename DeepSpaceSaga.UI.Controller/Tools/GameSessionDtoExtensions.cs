using DeepSpaceSaga.Common.Abstractions.Dto.Ui;
using DeepSpaceSaga.Common.Abstractions.Entities;
using DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects;
using DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects.Spacecrafts;
using DeepSpaceSaga.Common.Extensions.Entities.CelestialObjects;
using System.Drawing;
using System.Linq;

namespace DeepSpaceSaga.UI.Controller.Tools;

public static class GameSessionDtoExtensions
{
    public static List<CelestialObjectSaveFormatDto> GetCelestialObjects(this GameSessionDto session)
    {
        return session.CelestialObjects.Values.ToList();
    }

    public static SpaceMapColor GetColor(this CelestialObjectSaveFormatDto celestialObject)
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

    public static List<CelestialObjectSaveFormatDto> GetCelestialObjectsByDistance(this GameSessionDto gameSession, SpaceMapPoint coordinates, int range)
    {

        var resultObjects = gameSession.CelestialObjects.Values.Select(celestialObject => (celestialObject,
                    Distance: CalculateDistance(coordinates, celestialObject.X, celestialObject.Y)))
                .Where(x => x.Distance <= range)
                .OrderBy(x => x.Distance)
                .Select(x => x.celestialObject)
                .ToList();

        return resultObjects;
    }

    public static CelestialObjectSaveFormatDto GetPlayerSpaceShip(this GameSessionDto gameSession)
    {
        return gameSession.CelestialObjects.Values.FirstOrDefault(x => x.Type == CelestialObjectType.SpaceshipPlayer);
    }

    private static double CalculateDistance(SpaceMapPoint point1, double x2, double y2)
    {
        var dx = point1.X - x2;
        var dy = point1.Y - y2;
        return Math.Sqrt(dx * dx + dy * dy);
    }
}
