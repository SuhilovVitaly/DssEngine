using DeepSpaceSaga.UI.Rendering.Abstractions;

namespace DeepSpaceSaga.UI.Render.Model;

public class ZoomScreen : IZoomScreen
{
    public int Scale { get; set; } = 1;
    public float DrawScaleFactor { get; set; } = 1;
    public event Action? OnZoomIn;
    public event Action? OnZoomOut;  

    public void In()
    {
        Scale = Scale - SetDeltaByScale(Scale);

        if(Scale < 40) Scale = 40;

        DrawScaleFactor = 100.0f / Scale;

        OnZoomIn?.Invoke();    
    }

    public void Out()
    {
        Scale = Scale + SetDeltaByScale(Scale);

        if (Scale > 1000) Scale = 1000;

        DrawScaleFactor = 100.0f / Scale;

        OnZoomOut?.Invoke();
    }

    private int SetDeltaByScale(int scale)
    {
        var delta = 10;

        if (scale > 200) delta = 20;

        if (scale > 400) delta = 50;

        if (scale > 600) delta = 100;

        return delta;
    }
}
