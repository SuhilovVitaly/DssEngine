namespace DeepSpaceSaga.UI.Controller.Services;

public class ZoomScreen : IZoomScreen
{
    public int Scale { get; set; } = 100;
    public float DrawScaleFactor { get; set; } = 1;
    public event Action? OnZoomIn;
    public event Action? OnZoomOut;  

    public void In()
    {
        Scale -= SetDeltaByScale(Scale);

        if (Scale < 40) Scale = 40;

        DrawScaleFactor = (float)Math.Round(100.0f / Scale, 1);

        OnZoomIn?.Invoke();    
    }

    public void Out()
    {
        Scale += SetDeltaByScale(Scale);

        if (Scale > 1000) Scale = 1000;

        DrawScaleFactor = (float)Math.Round(100.0f / Scale, 1);

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
