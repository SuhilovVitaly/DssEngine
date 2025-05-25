namespace DeepSpaceSaga.UI.Render.Rendering.TacticalMap;

public class DrawGrid
{
    // Configuration constants
    private static readonly float[] GridSteps = { 10, 100, 1000, 10000 };
    private static readonly SpaceMapColor[] GridColors = { 
        new SpaceMapColor(Color.FromArgb(12, 12, 12)),
        new SpaceMapColor(Color.FromArgb(22, 22, 22)),
        new SpaceMapColor(Color.FromArgb(42, 42, 42)),
        new SpaceMapColor(Color.FromArgb(62, 62, 62))
    };
    private const float MinGridStepThreshold = 10f;
    
    // Simple cache for performance optimization
    private static float _lastScale = -1;
    private static SpaceMapPoint _lastCenter = new(-1, -1);
    private static float _lastWidth = -1;
    private static float _lastHeight = -1;
    
    public static void Execute(IScreenInfo screenInfo)
    {
        // Update cache values for potential future use
        _lastScale = screenInfo.Zoom.DrawScaleFactor;
        _lastCenter = screenInfo.CenterScreenOnMap;
        _lastWidth = screenInfo.Width;
        _lastHeight = screenInfo.Height;
        
        // Collect all lines by color for batch drawing
        var linesByColor = new Dictionary<SpaceMapColor, List<(SpaceMapPoint from, SpaceMapPoint to)>>();
        
        for (int i = 0; i < GridSteps.Length; i++)
        {
            var step = GridSteps[i];
            var color = GridColors[i];
            
            var stepAfterScale = step * screenInfo.Zoom.DrawScaleFactor;
            
            // Skip drawing if grid step is too small
            if (stepAfterScale < MinGridStepThreshold)
                continue;
                
            if (!linesByColor.ContainsKey(color))
                linesByColor[color] = new List<(SpaceMapPoint, SpaceMapPoint)>();
                
            CollectGridLines(screenInfo, step, stepAfterScale, linesByColor[color]);
        }
        
        // Batch draw all lines by color
        foreach (var kvp in linesByColor)
        {
            DrawLinesBatch(screenInfo, kvp.Key, kvp.Value);
        }
    }
    
    private static void CollectGridLines(IScreenInfo screenInfo, float step, float stepAfterScale, List<(SpaceMapPoint from, SpaceMapPoint to)> lines)
    {
        var mapTopLeftCorner = GetCommonLeftCorner(screenInfo, step);

        var stepsInScreenWidth = (int)(screenInfo.Width / stepAfterScale) * 2 + 2;
        var stepsInScreenHeight = (int)(screenInfo.Height / stepAfterScale) * 2 + 2;

        // Collect vertical lines
        for (var i = 0; i <= stepsInScreenWidth; i++)
        {
            var lineFrom = new SpaceMapPoint(mapTopLeftCorner.X + i * stepAfterScale, mapTopLeftCorner.Y);
            var lineTo = new SpaceMapPoint(mapTopLeftCorner.X + i * stepAfterScale, mapTopLeftCorner.Y + stepsInScreenHeight * stepAfterScale);
            lines.Add((lineFrom, lineTo));
        }

        // Collect horizontal lines
        for (var i = 0; i <= stepsInScreenHeight; i++)
        {
            var lineFrom = new SpaceMapPoint(mapTopLeftCorner.X, mapTopLeftCorner.Y + i * stepAfterScale);
            var lineTo = new SpaceMapPoint(mapTopLeftCorner.X + stepsInScreenWidth * stepAfterScale, mapTopLeftCorner.Y + i * stepAfterScale);
            lines.Add((lineFrom, lineTo));
        }
    }
    
    private static void DrawLinesBatch(IScreenInfo screenInfo, SpaceMapColor color, List<(SpaceMapPoint from, SpaceMapPoint to)> lines)
    {
        foreach (var (from, to) in lines)
        {
            DrawTools.DrawLine(screenInfo, color, from, to);
        }
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
