namespace DeepSpaceSaga.Common.Abstractions.UI;

public interface IZoomScreen
{
    int Scale { get; set; }
    float DrawScaleFactor { get; set; }
    void In();
    void Out();
    event Action? OnZoomIn;
    event Action? OnZoomOut;
}
