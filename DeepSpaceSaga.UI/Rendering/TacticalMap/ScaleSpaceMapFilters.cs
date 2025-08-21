using DeepSpaceSaga.Common.Abstractions.Dto.Ui;
using DeepSpaceSaga.Common.Abstractions.UI;

namespace DeepSpaceSaga.UI.Render.Rendering.TacticalMap;

public static class ScaleSpaceMapFilters
{
    public static bool IsHideDirections(CelestialObjectSaveFormatDto celestialObject, IScreenInfo screenInfo)
    {
        return celestialObject.X < screenInfo.Width * 0.1 || celestialObject.X > screenInfo.Width * 0.9;
    }

    public static bool IsHideCelestialObject(CelestialObjectSaveFormatDto celestialObject, IScreenInfo screenInfo)
    {
        return celestialObject.X < screenInfo.Width * 0.05 || celestialObject.X > screenInfo.Width * 0.95;
    }
}
