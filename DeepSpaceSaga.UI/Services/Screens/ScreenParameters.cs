using DeepSpaceSaga.Common.Geometry;
using DeepSpaceSaga.UI.Render.Model;
using DeepSpaceSaga.UI.Rendering.Abstractions;
using System.Drawing;
using System.Runtime.InteropServices;

namespace DeepSpaceSaga.UI.Services.Screens;

public class ScreenParameters : IScreenInfo
{
    [DllImport("user32.dll")]
    private static extern int GetSystemMetrics(int nIndex);
    
    private const int SM_CXSCREEN = 0;
    private const int SM_CYSCREEN = 1;

    private static readonly ILog Logger = LogManager.GetLogger(typeof(ScreenParameters));
    public SpaceMapPoint Center { get; private set; }
    public float Width { get; private set; }
    public float Height { get; private set; }
    public int DrawInterval { get; set; }
    public SpaceMapPoint CenterScreenOnMap { get; set; }
    public SKCanvas GraphicSurface { get; set; }
    public IZoomScreen Zoom { get; private set; } = new ZoomScreen();
    public int MonitorId { get; set; }

    public ScreenParameters()
    {
        Logger.Debug("Start screen initialization");

        var monitorId = 0;
        var screens = Screen.AllScreens;

        if (screens == null || screens.Length == 0)
        {
            throw new Exception("Monitors not found");
        }

        if (monitorId >= screens.Length)
        {
            throw new Exception($"Wrong ID of monitor: {monitorId}");
        }

        // Get the actual screen bounds (this will be the logical size)
        var screen = screens[monitorId];
        
        // Get real screen resolution using WinAPI
        var realWidth = GetSystemMetrics(SM_CXSCREEN);
        var realHeight = GetSystemMetrics(SM_CYSCREEN);
        var resolution = new Rectangle(0, 0, realWidth, realHeight);

        MonitorId = monitorId;
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
}

