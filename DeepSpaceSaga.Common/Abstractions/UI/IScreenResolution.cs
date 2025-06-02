namespace DeepSpaceSaga.Common.Abstractions.UI;

public interface IScreenResolution
{
    int Width { get; set; }
    int Height { get; set; }
    int MonitorId { get; set; }
}