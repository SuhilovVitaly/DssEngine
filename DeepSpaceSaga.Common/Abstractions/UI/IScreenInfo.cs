namespace DeepSpaceSaga.Common.Abstractions.UI;

public interface IScreenInfo
{
    SpaceMapPoint Center { get; }
    float Width { get; }
    float Height { get; }
    int DrawInterval { get; set; }
    SpaceMapPoint CenterScreenOnMap { get; set; }
    IZoomScreen Zoom { get; }
    SKCanvas GraphicSurface { get; set; }
    int MonitorId { get; set; }
    SpaceMapPoint MouseCelestialRelativeCoordinates { get; }
    SpaceMapPoint MouseCelestialCoordinates { get; }
    SpaceMapPoint MouseScreenCoordinates { get; }
    void SetMousePosition(SpaceMapPoint celestialCoordinates, SpaceMapPoint screenCoordinates);
}