namespace DeepSpaceSaga.UI.Controller.Services;

public class ScreenParameters : IScreenInfo
{
    private static readonly ILog Logger = LogManager.GetLogger(typeof(ScreenParameters));
    public SpaceMapPoint Center { get; private set; }
    public float Width { get; private set; }
    public float Height { get; private set; }
    public int DrawInterval { get; set; }
    public SpaceMapPoint CenterScreenOnMap { get; set; }
    public SKCanvas GraphicSurface { get; set; }
    public IZoomScreen Zoom { get; private set; } = new ZoomScreen();
    public int MonitorId { get; set; }
    public SpaceMapPoint MouseCelestialCoordinates { get; private set; }
    public SpaceMapPoint MouseCelestialRelativeCoordinates { get; private set; }
    public SpaceMapPoint MouseScreenCoordinates { get; private set; }

    public ScreenParameters(IScreenResolution screenResolution)
    {
        Logger.Debug("Start screen initialization");
        var resolution = new Rectangle(0, 0, screenResolution.Width, screenResolution.Height);

        MonitorId = screenResolution.MonitorId;
        Initialization(resolution, 10000, 10000);
    }

    private void Initialization(Rectangle screen, int centerScreenX = 10000, int centerScreenY = 10000)
    {
        Center = new SpaceMapPoint(screen.Width / 2, screen.Height / 2);

        // Start player ship coordinates in each battle (10000, 10000)
        CenterScreenOnMap = new SpaceMapPoint(centerScreenX, centerScreenY);

        Width = screen.Width;
        Height = screen.Height;
    }

    public void SetMousePosition(SpaceMapPoint celestialCoordinates, SpaceMapPoint screenCoordinates)
    {
        MouseCelestialCoordinates = celestialCoordinates;

        MouseCelestialRelativeCoordinates = UiTools.ToRelativeCoordinates(this, celestialCoordinates, Center);

        MouseScreenCoordinates = screenCoordinates;
    }
}

