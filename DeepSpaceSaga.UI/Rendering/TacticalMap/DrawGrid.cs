namespace DeepSpaceSaga.UI.Render.Rendering.TacticalMap;

public class DrawGrid
{
    public static void Execute(IScreenInfo screenInfo)
    {
        DrawCommonGrid(screenInfo, 10, new SpaceMapColor(Color.FromArgb(12, 12, 12)));
        DrawCommonGrid(screenInfo, 100, new SpaceMapColor(Color.FromArgb(22, 22, 22)));
        DrawCommonGrid(screenInfo, 1000, new SpaceMapColor(Color.FromArgb(42, 42, 42)));
        DrawCommonGrid(screenInfo, 10000, new SpaceMapColor(Color.FromArgb(62, 62, 62)));
    }

    private static void DrawCommonGrid(IScreenInfo screenInfo, float step, SpaceMapColor color)
    {
        var stepAfterScale = step * screenInfo.Zoom.DrawScaleFactor;
        
        // Skip drawing if grid step is too small
        if (stepAfterScale < 10)
            return;
            
        var mapTopLeftCorner = GetCommonLeftCorner(screenInfo, step);

        var stepsInScreenWidth = (int)(screenInfo.Width / stepAfterScale) * 2 + 2;
        var stepsInScreenHeight = (int)(screenInfo.Height / stepAfterScale) * 2 + 2;

        for (var i = 0; i <= stepsInScreenWidth; i++)
        {
            var lineFrom = new SpaceMapPoint(mapTopLeftCorner.X + i * stepAfterScale, mapTopLeftCorner.Y);
            var lineTo = new SpaceMapPoint(mapTopLeftCorner.X + i * stepAfterScale, mapTopLeftCorner.Y + stepsInScreenHeight * stepAfterScale);

            DrawTools.DrawLine(screenInfo, color, lineFrom, lineTo);
        }

        for (var i = 0; i <= stepsInScreenHeight; i++)
        {
            var lineFrom = new SpaceMapPoint(mapTopLeftCorner.X, mapTopLeftCorner.Y + i * stepAfterScale);
            var lineTo = new SpaceMapPoint(mapTopLeftCorner.X + stepsInScreenWidth * stepAfterScale, mapTopLeftCorner.Y + i * stepAfterScale);

            DrawTools.DrawLine(screenInfo, color, lineFrom, lineTo);
        }
    }

    internal static SpaceMapPoint GetCommonLeftCorner(IScreenInfo screenInfo, float cellSize)
    {
        var screenLeftX = screenInfo.CenterScreenOnMap.X - (screenInfo.Width / 2);
        var screenLeftY = screenInfo.CenterScreenOnMap.Y - (screenInfo.Height / 2);

        var positionX = (int)(screenLeftX / cellSize) * cellSize;
        var positionY = (int)(screenLeftY / cellSize) * cellSize;

        var cornerPosition = UiTools.ToScreenCoordinatesFull(screenInfo, new SpaceMapPoint(positionX, positionY), true);

        while (cornerPosition.X > 0)
        {
            cornerPosition.X = cornerPosition.X - cellSize * screenInfo.Zoom.DrawScaleFactor;
            cornerPosition.Y = cornerPosition.Y - cellSize * screenInfo.Zoom.DrawScaleFactor;
        }

        return cornerPosition;
    }
}
