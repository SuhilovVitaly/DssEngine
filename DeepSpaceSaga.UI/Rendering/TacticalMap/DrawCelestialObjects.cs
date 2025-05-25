using DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects;
using DeepSpaceSaga.Common.Extensions.Entities.CelestialObjects;
using DeepSpaceSaga.Common.Abstractions.Session;

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
                    DrawPlayerSpaceship(screenInfo, currentObject);
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
            }
        }
    }

    private static void DrawPlayerSpaceship(IScreenInfo screenInfo, CelestialObjectDto spaceShip)
    {
        var screenCoordinates = UiTools.ToScreenCoordinates(screenInfo, spaceShip.GetLocation(), true);
        var color = spaceShip.GetColor();

        DrawTools.FillEllipse(screenInfo, screenCoordinates.X, screenCoordinates.Y, 4, color);
        DrawTools.DrawEllipse(screenInfo, screenCoordinates.X, screenCoordinates.Y, 4, color);
        DrawTools.DrawEllipse(screenInfo, screenCoordinates.X, screenCoordinates.Y, 8, color);
    }

    private static void DrawStation(IScreenInfo screenInfo, CelestialObjectDto spaceStation, GameSessionDto session)
    {
        var screenCoordinates = UiTools.ToScreenCoordinates(screenInfo, spaceStation.GetLocation(), true);
        var color = spaceStation.GetColor();

        DrawTools.FillEllipse(screenInfo, screenCoordinates.X, screenCoordinates.Y, 8, color);
        DrawTools.DrawEllipse(screenInfo, screenCoordinates.X, screenCoordinates.Y, 8, color);
        DrawTools.DrawEllipse(screenInfo, screenCoordinates.X, screenCoordinates.Y, 12, color);

        if (spaceStation.IsPreScanned)
        {
            DrawCelestialObjectInfo(screenInfo, spaceStation, color, session);
        }
    }

    private static void DrawCelestialObject(IScreenInfo screenInfo, CelestialObjectDto celestialObject, GameSessionDto session)
    {
        // TODO: Optimization by zoom
        if(ScaleSpaceMapFilters.IsHideCelestialObject(celestialObject, screenInfo))
        {
            // No need draw asteroid - scale too big
            return;
        }

        var screenCoordinates = UiTools.ToScreenCoordinates(screenInfo, celestialObject.GetLocation(), true);
        var color = celestialObject.GetColor();

        DrawTools.FillEllipse(screenInfo, screenCoordinates.X, screenCoordinates.Y, 4, color);
        DrawTools.DrawEllipse(screenInfo, screenCoordinates.X, screenCoordinates.Y, 4, color);

        if (celestialObject.IsPreScanned && screenInfo.Zoom.Scale < 180)
        {
            DrawCelestialObjectInfo(screenInfo, celestialObject, color, session);
        }

    }

    private static void DrawContainerObject(IScreenInfo screenInfo, CelestialObjectDto celestialObject, GameSessionDto session)
    {
        var screenCoordinates = UiTools.ToScreenCoordinates(screenInfo, celestialObject.GetLocation(), true);
        var color = celestialObject.GetColor();

        DrawTools.FillEllipse(screenInfo, screenCoordinates.X, screenCoordinates.Y, 4, color);
        DrawTools.DrawEllipse(screenInfo, screenCoordinates.X, screenCoordinates.Y, 4, color);

        if (celestialObject.IsPreScanned)
        {
            DrawCelestialObjectInfo(screenInfo, celestialObject, color, session);
        }
    }

    private static void DrawCelestialObjectInfo(IScreenInfo screenInfo, CelestialObjectDto celestialObject, SpaceMapColor color, GameSessionDto session)
    {
        var screenCoordinates = UiTools.ToScreenCoordinates(screenInfo, celestialObject.GetLocation(), true);

        var startLabel = GeometryTools.Move(screenCoordinates, 45, LabelDirection(celestialObject));

        DrawTools.DrawLine(screenInfo, new SpaceMapColor(32, 32, 32), screenCoordinates, new SpaceMapPoint(startLabel.X, startLabel.Y + 15));

        DrawTools.FillRectangle(screenInfo, new SpaceMapColor(Color.FromArgb(22, 22, 22)), startLabel, 120, 18);
        DrawTools.FillRectangle(screenInfo, new SpaceMapColor(Color.FromArgb(52, 52, 52)), startLabel.X, startLabel.Y + 15, 120, 4);

        if (session.State.ProcessedTurns % 2 == 0)
        {
            DrawTools.FillRectangle(screenInfo, new SpaceMapColor(Color.FromArgb(160, 90, 0)), startLabel.X, startLabel.Y + 3, 8, 8);
        }
        else
        {
            DrawTools.FillRectangle(screenInfo, new SpaceMapColor(Color.WhiteSmoke), startLabel.X, startLabel.Y + 3, 8, 8);
        }

        var label = celestialObject.IsPreScanned ? celestialObject.Name : "Unknown Celestial Object";

        DrawTools.DrawString(screenInfo, label, new Font("Tahoma", 12), color, new RectangleF(startLabel.X + 15, startLabel.Y + 12, 190, 50));
    }

    private static int LabelDirection(CelestialObjectDto celestialObject)
    {
        var direction = 0;

        if(celestialObject.Direction > 0 && celestialObject.Direction < 90)
        {
            return 270 + 90 / 2;
        }

        if (celestialObject.Direction > 90 && celestialObject.Direction < 180)
        {
            return 270 + 90 / 2;
        }

        if (celestialObject.Direction > 180 && celestialObject.Direction < 270)
        {
            return 45;
        }

        if (celestialObject.Direction > 270 && celestialObject.Direction < 360)
        {
            return 135;
        }


        return direction;
    }
}
