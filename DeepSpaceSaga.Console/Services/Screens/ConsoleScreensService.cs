using DeepSpaceSaga.Common.Abstractions.UI.Screens;

namespace DeepSpaceSaga.Console.Services.Screens;

public class ConsoleScreensService :IScreensService
{
    public IScreenTacticalMap TacticalMap { get; set; }

    public ConsoleScreensService(IScreenTacticalMap tacticalMap)
    {
        TacticalMap = tacticalMap;
    }
    
    public void ShowGameMenuScreen()
    {
        
    }

    public void ShowTacticalMapScreen()
    {
        
    }

    public void ShowGameMenuModal()
    {
        
    }

    public void CloseTacticalMapScreen()
    {
        
    }
}