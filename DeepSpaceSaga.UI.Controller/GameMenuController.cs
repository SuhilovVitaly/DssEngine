using DeepSpaceSaga.UI.Controller.Abstractions;
using DeepSpaceSaga.Common.Abstractions.Services;

namespace DeepSpaceSaga.UI.Controller;

/// <summary>
/// Controller for handling Game Menu business logic
/// </summary>
public class GameMenuController : IGameMenuController
{
    private readonly IGameServer _gameServer;
    private readonly IScreensService _screensService;

    public GameMenuController(IGameServer gameServer, IScreensService screensService)
    {
        _gameServer = gameServer ?? throw new ArgumentNullException(nameof(gameServer));
        _screensService = screensService ?? throw new ArgumentNullException(nameof(screensService));
    }

    public async Task ResumeGameAsync()
    {
        // Resume game logic here
        await Task.CompletedTask;
    }

    public async Task SaveGameAsync()
    {
        // Save game logic here  
        await Task.CompletedTask;
    }

    public async Task LoadGameAsync()
    {
        // Load game logic here
        await Task.CompletedTask;
    }

    public async Task GoToMainMenuAsync()
    {
        // Go to main menu logic here
        await Task.CompletedTask;
    }
} 