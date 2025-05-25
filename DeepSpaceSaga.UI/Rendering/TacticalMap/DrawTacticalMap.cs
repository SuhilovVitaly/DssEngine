using DeepSpaceSaga.UI.Render.Tools;
using DeepSpaceSaga.Common.Geometry;

namespace DeepSpaceSaga.UI.Render.Rendering.TacticalMap;

public static class DrawTacticalMap
{
    public static void DrawTacticalMapScreen(GameSessionDto session, IScreenInfo screenParameters)
    {
        DrawTools.DrawLine(screenParameters, new SpaceMapColor(Color.Azure), new SpaceMapPoint(0,0), new SpaceMapPoint(1000, 10000));

        //DrawGrid.Execute(screenParameters);
    }
}

// Helper wrapper for DrawTools compatibility
internal class ScreenInfoWrapper : IScreenInfo
{
    private readonly SKCanvas _canvas;
    private readonly IScreenInfo _original;
    
    public ScreenInfoWrapper(SKCanvas canvas, IScreenInfo original)
    {
        _canvas = canvas;
        _original = original;
    }
    
    public SpaceMapPoint Center => _original.Center;
    public float Width => _original.Width;
    public float Height => _original.Height;
    public int DrawInterval { get => _original.DrawInterval; set => _original.DrawInterval = value; }
    public SpaceMapPoint CenterScreenOnMap { get => _original.CenterScreenOnMap; set => _original.CenterScreenOnMap = value; }
    public IZoomScreen Zoom => _original.Zoom;
    public SKCanvas GraphicSurface { get => _canvas; set { } }
    public int MonitorId { get => _original.MonitorId; set => _original.MonitorId = value; }
}
