namespace DeepSpaceSaga.UI.Services;

public class ScreenResolution: IScreenResolution
{
    public int Width { get; set; }
    public int Height { get; set; }
    public int MonitorId { get; set; }
    
    [DllImport("user32.dll")]
    private static extern int GetSystemMetrics(int nIndex);
    
    private const int SM_CXSCREEN = 0;
    private const int SM_CYSCREEN = 1;

    public ScreenResolution()
    {
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
        Width = GetSystemMetrics(SM_CXSCREEN);
        Height = GetSystemMetrics(SM_CYSCREEN);
    }
}