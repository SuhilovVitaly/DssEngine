using DeepSpaceSaga.Common.Abstractions.UI;

namespace DeepSpaceSaga.Console.Services.Screens;

public class ConsoleScreenResolution: IScreenResolution
{
    public int Width { get; set; } = 1000;
    public int Height { get; set; } = 1000;
    public int MonitorId { get; set; } = 1;
}