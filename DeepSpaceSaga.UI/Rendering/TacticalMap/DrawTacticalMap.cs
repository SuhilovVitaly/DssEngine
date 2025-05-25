using DeepSpaceSaga.UI.Render.Tools;
using DeepSpaceSaga.Common.Geometry;
using DeepSpaceSaga.UI.Rendering.TacticalMap;

namespace DeepSpaceSaga.UI.Render.Rendering.TacticalMap;

public static class DrawTacticalMap
{
    public static void DrawTacticalMapScreen(GameSessionDto session, IScreenInfo screenParameters)
    {
        //DrawTools.DrawLine(screenParameters, new SpaceMapColor(Color.Azure), new SpaceMapPoint(0,0), new SpaceMapPoint(1000, 10000));

        DrawGrid.Execute(screenParameters);
    }
}
