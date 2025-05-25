namespace DeepSpaceSaga.UI.Rendering.Abstractions;

public interface IZoomScreen
{
    int Scale { get; set; }
    float DrawScaleFactor { get; set; }
    void In();
    void Out();
    event Action? OnZoomIn;
    event Action? OnZoomOut;
}
