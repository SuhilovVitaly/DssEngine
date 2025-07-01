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
    public static List<CelestialObjectDto> GetCelestialObjects(this GameSessionDto session)
    {
        return session.CelestialObjects.Values.ToList();
    }

    public static SpaceMapColor GetColor(this CelestialObjectDto celestialObject)
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

    public static List<CelestialObjectDto> GetCelestialObjectsByDistance(this GameSessionDto gameSession, SpaceMapPoint coordinates, int range)
    {

        var resultObjects = gameSession.CelestialObjects.Values.Select(celestialObject => (celestialObject,
                    GeometryTools.Distance(
                        coordinates,
                        celestialObject.GetLocation())
                )).
            Where(e => e.Item2 < range).
            OrderBy(e => e.Item2).
            Select(e => e.celestialObject).
            ToList();

        return resultObjects;
    }

    public static CelestialObjectDto GetPlayerSpaceShip(this GameSessionDto gameSession)
    {
        foreach (var celestialObject in from celestialObject in gameSession.CelestialObjects.Values
                                        where celestialObject.Type == CelestialObjectType.SpaceshipPlayer
                                        select celestialObject)
        {
            return celestialObject;
        }

        throw new InvalidOperationException("Player spaceship not found in the game session");
    }
}
