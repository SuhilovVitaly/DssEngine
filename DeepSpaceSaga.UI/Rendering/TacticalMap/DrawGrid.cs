namespace DeepSpaceSaga.UI.Render.Rendering.TacticalMap;

public class DrawGrid
{
    // Cache for performance optimization
    private static float _lastScale = -1;
    private static SpaceMapPoint _lastCenter = new(-1, -1);
    private static float _lastWidth = -1;
    private static float _lastHeight = -1;
    private static Dictionary<float, SpaceMapPoint> _cornerCache = new();
    private static Dictionary<string, (int width, int height)> _stepsCache = new();
    private static float _lastCameraMovementSpeed = 0;
    
    public static void Execute(IScreenInfo screenInfo)
    {
        // Check if parameters changed and clear caches if needed
        var parametersChanged = _lastScale != screenInfo.Zoom.DrawScaleFactor || 
                               !_lastCenter.Equals(screenInfo.CenterScreenOnMap) ||
                               _lastWidth != screenInfo.Width ||
                               _lastHeight != screenInfo.Height;
        
        if (parametersChanged)
        {
            // Clear caches when parameters change
            _cornerCache.Clear();
            _stepsCache.Clear();
            
            // Update cache values
            _lastScale = screenInfo.Zoom.DrawScaleFactor;
            _lastCenter = screenInfo.CenterScreenOnMap;
            _lastWidth = screenInfo.Width;
            _lastHeight = screenInfo.Height;
        }
        
        // Adaptive quality - reduce detail during fast camera movement
        var cameraMovementSpeed = CalculateCameraMovementSpeed(screenInfo);
        _lastCameraMovementSpeed = cameraMovementSpeed;
        
        if (cameraMovementSpeed > 10.0f)
        {
            // Only draw large grid during fast movement
            DrawCommonGrid(screenInfo, 10000, new SpaceMapColor(Color.FromArgb(62, 62, 62)));
            return;
        }
        
        DrawCommonGrid(screenInfo, 10, new SpaceMapColor(Color.FromArgb(12, 12, 12)));
        DrawCommonGrid(screenInfo, 100, new SpaceMapColor(Color.FromArgb(22, 22, 22)));
        DrawCommonGrid(screenInfo, 1000, new SpaceMapColor(Color.FromArgb(42, 42, 42)));
        DrawCommonGrid(screenInfo, 10000, new SpaceMapColor(Color.FromArgb(62, 62, 62)));
    }
    
    private static float CalculateCameraMovementSpeed(IScreenInfo screenInfo)
    {
        // Simple movement speed calculation (can be enhanced with actual velocity tracking)
        return 0; // Placeholder - would need previous position tracking
    }

    private static void DrawCommonGrid(IScreenInfo screenInfo, float step, SpaceMapColor color)
    {
        var stepAfterScale = step * screenInfo.Zoom.DrawScaleFactor;
        
        // Skip drawing if grid step is too small
        if (stepAfterScale < 10)
            return;
            
        var mapTopLeftCorner = GetCommonLeftCornerCached(screenInfo, step);

        var cacheKey = $"{step}_{screenInfo.Width}_{screenInfo.Height}_{stepAfterScale}";
        if (!_stepsCache.TryGetValue(cacheKey, out var cachedSteps))
        {
            cachedSteps = (
                (int)(screenInfo.Width / stepAfterScale) * 2 + 2,
                (int)(screenInfo.Height / stepAfterScale) * 2 + 2
            );
            _stepsCache[cacheKey] = cachedSteps;
        }
        
        var stepsInScreenWidth = cachedSteps.width;
        var stepsInScreenHeight = cachedSteps.height;

        // Collect all lines for batch drawing
        var verticalLines = new List<(SpaceMapPoint from, SpaceMapPoint to)>();
        var horizontalLines = new List<(SpaceMapPoint from, SpaceMapPoint to)>();

        // Calculate screen bounds for culling
        var screenLeft = 0;
        var screenRight = screenInfo.Width;
        var screenTop = 0;
        var screenBottom = screenInfo.Height;

        for (var i = 0; i <= stepsInScreenWidth; i++)
        {
            var lineX = mapTopLeftCorner.X + i * stepAfterScale;
            
            // Cull lines outside screen bounds
            if (lineX >= screenLeft - stepAfterScale && lineX <= screenRight + stepAfterScale)
            {
                var lineFrom = new SpaceMapPoint(lineX, mapTopLeftCorner.Y);
                var lineTo = new SpaceMapPoint(lineX, mapTopLeftCorner.Y + stepsInScreenHeight * stepAfterScale);
                verticalLines.Add((lineFrom, lineTo));
            }
        }

        for (var i = 0; i <= stepsInScreenHeight; i++)
        {
            var lineY = mapTopLeftCorner.Y + i * stepAfterScale;
            
            // Cull lines outside screen bounds
            if (lineY >= screenTop - stepAfterScale && lineY <= screenBottom + stepAfterScale)
            {
                var lineFrom = new SpaceMapPoint(mapTopLeftCorner.X, lineY);
                var lineTo = new SpaceMapPoint(mapTopLeftCorner.X + stepsInScreenWidth * stepAfterScale, lineY);
                horizontalLines.Add((lineFrom, lineTo));
            }
        }
        
        // Batch draw all lines
        DrawLinesBatch(screenInfo, color, verticalLines);
        DrawLinesBatch(screenInfo, color, horizontalLines);
    }
    
    private static void DrawLinesBatch(IScreenInfo screenInfo, SpaceMapColor color, List<(SpaceMapPoint from, SpaceMapPoint to)> lines)
    {
        if (lines.Count == 0) return;
        
        // For now, draw individually (can be optimized further with actual batch drawing)
        foreach (var (from, to) in lines)
        {
            DrawTools.DrawLine(screenInfo, color, from, to);
        }
    }
    
    private static SpaceMapPoint GetCommonLeftCornerCached(IScreenInfo screenInfo, float cellSize)
    {
        // Use cached result if available
        if (_cornerCache.TryGetValue(cellSize, out var cachedCorner))
        {
            return cachedCorner;
        }
        
        var corner = GetCommonLeftCorner(screenInfo, cellSize);
        _cornerCache[cellSize] = corner;
        return corner;
    }

    internal static SpaceMapPoint GetCommonLeftCorner(IScreenInfo screenInfo, float cellSize)
    {
        var screenLeftX = screenInfo.CenterScreenOnMap.X - (screenInfo.Width / 2);
        var screenLeftY = screenInfo.CenterScreenOnMap.Y - (screenInfo.Height / 2);

        var positionX = (int)(screenLeftX / cellSize) * cellSize;
        var positionY = (int)(screenLeftY / cellSize) * cellSize;

        var cornerPosition = UiTools.ToScreenCoordinatesFull(screenInfo, new SpaceMapPoint(positionX, positionY), true);

        // Optimization: limit iterations and use mathematical calculation
        var maxIterations = 100;
        var iterations = 0;
        var stepSize = cellSize * screenInfo.Zoom.DrawScaleFactor;
        
        while (cornerPosition.X > 0 && iterations < maxIterations)
        {
            cornerPosition.X = cornerPosition.X - stepSize;
            cornerPosition.Y = cornerPosition.Y - stepSize;
            iterations++;
        }

        return cornerPosition;
    }
}
