using DeepSpaceSaga.Common.Abstractions.Dto.Ui;
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

    public async Task ShowGameMenuModal()
    {
        
    }

    public void CloseTacticalMapScreen()
    {
        
    }

    public async Task ShowDialogScreen(GameActionEventDto gameActionEvent)
    {
        throw new NotImplementedException();
    }
}