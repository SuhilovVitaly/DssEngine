namespace DeepSpaceSaga.UI.Render.Tools;

public class UiTools
{
    public static SpaceMapPoint ToRelativeCoordinates(IScreenInfo screenParameters, SpaceMapPoint mouseLocation, SpaceMapPoint centerPosition)
    {
        var relativeX = (mouseLocation.X - centerPosition.X) ;
        var relativeY = (mouseLocation.Y - centerPosition.Y);

        return new SpaceMapPoint(relativeX, relativeY);
    }

    public static SpaceMapPoint ToTacticalMapCoordinates(IScreenInfo screenParameters, SpaceMapPoint currentMouseCoordinates, SpaceMapPoint centerPosition)
    {
        var relativeX = (centerPosition.X + currentMouseCoordinates.X / screenParameters.Zoom.DrawScaleFactor) ;
        var relativeY = (centerPosition.Y + currentMouseCoordinates.Y / screenParameters.Zoom.DrawScaleFactor);

        return new SpaceMapPoint(relativeX, relativeY);
    }

    public static SpaceMapPoint ToScreenCoordinates(IScreenInfo screenParameters, SpaceMapPoint celestialObjectPosition, bool isUseScaleFactor = false)
    {
        var relativeX = celestialObjectPosition.X - screenParameters.CenterScreenOnMap.X + screenParameters.Width / 2;
        var relativeY = celestialObjectPosition.Y - screenParameters.CenterScreenOnMap.Y + screenParameters.Height / 2;

        if(isUseScaleFactor)
        {
            relativeX = (celestialObjectPosition.X - screenParameters.CenterScreenOnMap.X) * screenParameters.Zoom.DrawScaleFactor + screenParameters.Width / 2;
            relativeY = (celestialObjectPosition.Y - screenParameters.CenterScreenOnMap.Y) * screenParameters.Zoom.DrawScaleFactor + screenParameters.Height / 2;
        }

        return new SpaceMapPoint(relativeX, relativeY);
    }

    public static SpaceMapPoint ToScreenCoordinatesFull(IScreenInfo screenParameters, SpaceMapPoint celestialObjectPosition, bool isUseScaleFactor = false)
    {
        var relativeX = (celestialObjectPosition.X - screenParameters.CenterScreenOnMap.X) * screenParameters.Zoom.DrawScaleFactor + screenParameters.Width / 2;
        var relativeY = (celestialObjectPosition.Y - screenParameters.CenterScreenOnMap.Y) * screenParameters.Zoom.DrawScaleFactor + screenParameters.Height / 2;

        return new SpaceMapPoint(relativeX, relativeY);
    }

    internal static SpaceMapPoint ToScreenCoordinates(IScreenInfo screenInfo, double positionX, double positionY)
    {
        return ToScreenCoordinates(screenInfo, new SpaceMapPoint((float)positionX, (float)positionY));
    }

    internal static int ZoomToCellSize(int zoomSize)
    {
        return zoomSize switch
        {
            1 => 50,
            2 => 25,
            _ => throw new ArgumentException(),
        };
    }
}