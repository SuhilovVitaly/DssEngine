using DeepSpaceSaga.Common.Abstractions.Dto.Ui;
using DeepSpaceSaga.Common.Abstractions.UI;

namespace DeepSpaceSaga.UI.Render.Rendering.TacticalMap;

public static class ScaleSpaceMapFilters
{
    public static bool IsHideDirections(CelestialObjectDto celestialObject, IScreenInfo screenInfo)
    {
        return IsHideCelestialObject(celestialObject, screenInfo);
    }

    public static bool IsHideCelestialObject(CelestialObjectDto celestialObject, IScreenInfo screenInfo)
    {
        switch (celestialObject.Type)
        {
            case CelestialObjectType.Unknown:
                break;
            case CelestialObjectType.PointInMap:
                break;
            case CelestialObjectType.Asteroid:
                return (screenInfo.Zoom.Scale > 250);
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

        return false;
    }
}
