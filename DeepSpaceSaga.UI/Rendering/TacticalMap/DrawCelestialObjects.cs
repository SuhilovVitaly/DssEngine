using DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects;
using DeepSpaceSaga.Common.Extensions.Entities.CelestialObjects;
using DeepSpaceSaga.Common.Abstractions.Session;
using DeepSpaceSaga.Common.Abstractions.UI;
using DeepSpaceSaga.UI.Rendering.Tools;
using DeepSpaceSaga.Common.Abstractions.Dto.Ui;
using DeepSpaceSaga.UI.Controller.Tools;

namespace DeepSpaceSaga.UI.Render.Rendering.TacticalMap;

public class DrawCelestialObjects
{
    public static void Execute(IScreenInfo screenInfo, GameSessionDto session)
    {
        foreach (var currentObject in session.GetCelestialObjects())
        {
            switch (currentObject.Type)
            {
                case CelestialObjectType.Unknown:
                    break;
                case CelestialObjectType.PointInMap:
                    break;
                case CelestialObjectType.Asteroid:
                    DrawCelestialObject(screenInfo, currentObject, session);
                    break;
                case CelestialObjectType.Station:
                    DrawStation(screenInfo, currentObject, session);
                    break;
                case CelestialObjectType.SpaceshipPlayer:
                    DrawPlayerSpaceship(screenInfo, currentObject, session);
                    break;
                case CelestialObjectType.SpaceshipNpcNeutral:
                    DrawCelestialObject(screenInfo, currentObject, session);
                    break;
                case CelestialObjectType.SpaceshipNpcEnemy:
                    DrawCelestialObject(screenInfo, currentObject, session);
                    break;
                case CelestialObjectType.SpaceshipNpcFriend:
                    DrawCelestialObject(screenInfo, currentObject, session);
                    break;
                case CelestialObjectType.Container:
                    DrawContainerObject(screenInfo, currentObject, session);
                    break;
                case CelestialObjectType.Missile:
                    break;
                case CelestialObjectType.Explosion:
                    break;
                default:
                    break;
            }
        }
    }

    private static void DrawPlayerSpaceship(IScreenInfo screenInfo, CelestialObjectSaveFormatDto spaceShip, GameSessionDto session)
    {
        var color = spaceShip.GetColor();
        DrawCelestialObjectInfo(screenInfo, spaceShip, color, session);
    }

    private static void DrawStation(IScreenInfo screenInfo, CelestialObjectSaveFormatDto spaceStation, GameSessionDto session)
    {
        var color = spaceStation.GetColor();
        DrawCelestialObjectInfo(screenInfo, spaceStation, color, session);
    }

    private static void DrawCelestialObject(IScreenInfo screenInfo, CelestialObjectSaveFormatDto celestialObject, GameSessionDto session)
    {
        var color = celestialObject.GetColor();
        DrawCelestialObjectInfo(screenInfo, celestialObject, color, session);
    }

    private static void DrawContainerObject(IScreenInfo screenInfo, CelestialObjectSaveFormatDto celestialObject, GameSessionDto session)
    {
        var color = celestialObject.GetColor();
        DrawCelestialObjectInfo(screenInfo, celestialObject, color, session);
    }

    private static void DrawCelestialObjectInfo(IScreenInfo screenInfo, CelestialObjectSaveFormatDto celestialObject, SpaceMapColor color, GameSessionDto session)
    {
        var x = (int)(celestialObject.X * screenInfo.Zoom.Scale);
        var y = (int)(celestialObject.Y * screenInfo.Zoom.Scale);

        if (ScaleSpaceMapFilters.IsHideCelestialObject(celestialObject, screenInfo))
            return;

        var size = (int)(celestialObject.Size * screenInfo.Zoom.Scale);
        if (size < 1) size = 1;

        DrawTools.FillEllipse(screenInfo, x, y, size, color);

        if (celestialObject.IsPreScanned)
        {
            var labelDirection = LabelDirection(celestialObject);
            var font = new Font("Arial", 12);
            var nameRect = new RectangleF(x - 50, y - size - 20, 100, 20);
            var directionRect = new RectangleF(x - 50, y + size + 5, 100, 20);
            DrawTools.DrawString(screenInfo, celestialObject.Name, font, color, nameRect);
            DrawTools.DrawString(screenInfo, $"Dir: {labelDirection}°", font, color, directionRect);
        }
    }

    private static int LabelDirection(CelestialObjectSaveFormatDto celestialObject)
    {
        return (int)celestialObject.Direction;
    }
}
