using DeepSpaceSaga.Common.Abstractions.Dto.Ui;
using DeepSpaceSaga.Common.Abstractions.UI.Screens;
using DeepSpaceSaga.Common.Implementation.Entities.Dialogs;

namespace DeepSpaceSaga.Console.Services.Screens;

public class ConsoleScreensService :IScreensService
{
    public event Action<DialogExit>? OnDialogChoice;

    public IScreenTacticalMap TacticalMap { get; set; }

    public ConsoleScreensService(IScreenTacticalMap tacticalMap)
    {
        TacticalMap = tacticalMap;
    }

    event Action<DialogExit, DialogDto>? IScreensService.OnDialogChoice
    {
        add
        {
            throw new NotImplementedException();
        }

        remove
        {
            throw new NotImplementedException();
        }
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