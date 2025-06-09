using DeepSpaceSaga.Common.Abstractions.UI;
using DeepSpaceSaga.UI.Render.Tools;
using DeepSpaceSaga.Common.Geometry;
using DeepSpaceSaga.UI.Rendering.Tools;
using DeepSpaceSaga.Common.Abstractions.Dto.Ui;

namespace DeepSpaceSaga.UI.Render.Rendering.TacticalMap;

public static class DrawTacticalMap
{
    public static void DrawTacticalMapScreen(GameSessionDto session, IScreenInfo screenParameters)
    {
        DrawGrid.Execute(screenParameters);
        DrawCelestialObjects.Execute(screenParameters, session);

        var centerScreen = UiTools.ToScreenCoordinates(screenParameters, screenParameters.CenterScreenOnMap.X, screenParameters.CenterScreenOnMap.Y);
        // Draw green circle at screen center
        var centerX = screenParameters.Width / 2;
        var centerY = screenParameters.Height / 2;
        var greenColor = new SpaceMapColor(Color.Green);
        //DrawTools.FillEllipse(screenParameters, centerScreen.X, centerScreen.Y, 10, greenColor);
        
        // Draw Hello World text in bottom left corner
        var font = new Font("Arial", 16);
        var whiteColor = new SpaceMapColor(Color.White);
        var textRect = new RectangleF(10, screenParameters.Height - 30, 200, 20);
        DrawTools.DrawString(screenParameters, $"Zoom is {screenParameters.Zoom.Scale} DrawScaleFactor {screenParameters.Zoom.DrawScaleFactor}", font, whiteColor, textRect);
    }
}
