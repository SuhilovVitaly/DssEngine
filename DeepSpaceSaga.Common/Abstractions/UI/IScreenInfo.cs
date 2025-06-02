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
    SpaceMapPoint RelativeMousePosition { get; }
    SpaceMapPoint MousePosition { get; }
    void SetMousePosition(SpaceMapPoint mousePosition);
}