using DeepSpaceSaga.UI.Render.Tools;
using DeepSpaceSaga.Common.Geometry;

namespace DeepSpaceSaga.UI.Rendering.TacticalMap;

public class DrawGridX
{
    public static void Execute(IScreenInfo screenParameters)
    {
        const int gridStep = 500;
        var gridColor = new SpaceMapColor(Color.WhiteSmoke);
        
        // Apply zoom scale factor to grid step
        var scaledGridStep = (int)(gridStep * screenParameters.Zoom.DrawScaleFactor);
        
        // Calculate screen center
        var screenCenterX = screenParameters.Width / 2;
        var screenCenterY = screenParameters.Height / 2;
        
        // Calculate how many grid cells fit on screen
        var cellsOnScreenWidth = (int)Math.Ceiling(screenParameters.Width / (float)scaledGridStep);
        var cellsOnScreenHeight = (int)Math.Ceiling(screenParameters.Height / (float)scaledGridStep);
        
        // Add one cell on each side for safety
        var totalCellsWidth = cellsOnScreenWidth + 2;
        var totalCellsHeight = cellsOnScreenHeight + 2;
        
        // Find the grid line that should be closest to the left edge of screen
        var worldCenterX = screenParameters.CenterScreenOnMap.X;
        var worldCenterY = screenParameters.CenterScreenOnMap.Y;
        
        // Find the leftmost grid line we need to draw
        var leftmostGridX = (float)(Math.Floor((worldCenterX - screenCenterX) / scaledGridStep) * scaledGridStep) - scaledGridStep;
        var topmostGridY = (float)(Math.Floor((worldCenterY - screenCenterY) / scaledGridStep) * scaledGridStep) - scaledGridStep;
        
        // Draw vertical lines
        for (int i = 0; i < totalCellsWidth; i++)
        {
            var worldX = leftmostGridX + (i * scaledGridStep);
            var screenX = worldX - worldCenterX + screenCenterX;
            
            DrawTools.DrawLine(screenParameters, gridColor,
                new SpaceMapPoint(screenX, 0),
                new SpaceMapPoint(screenX, screenParameters.Height));
        }
        
        // Draw horizontal lines
        for (int i = 0; i < totalCellsHeight; i++)
        {
            var worldY = topmostGridY + (i * scaledGridStep);
            var screenY = worldY - worldCenterY + screenCenterY;
            
            DrawTools.DrawLine(screenParameters, gridColor,
                new SpaceMapPoint(0, screenY),
                new SpaceMapPoint(screenParameters.Width, screenY));
        }
    }
}
