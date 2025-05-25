using DeepSpaceSaga.Common.Geometry;
using SkiaSharp;

namespace DeepSpaceSaga.UI.Rendering.Abstractions;

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
}