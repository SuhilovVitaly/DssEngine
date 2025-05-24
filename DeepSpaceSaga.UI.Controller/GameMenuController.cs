using DeepSpaceSaga.UI.Controller.Abstractions;
using DeepSpaceSaga.Common.Abstractions.Services;

namespace DeepSpaceSaga.UI.Controller;

/// <summary>
/// Controller for handling Game Menu business logic
/// </summary>
public class GameMenuController : IGameMenuController
{
    private readonly IGameManager _gameManager;

    public GameMenuController(IGameManager gameManager)
    {
        _gameManager = gameManager ?? throw new ArgumentNullException(nameof(gameManager));
    }

    public async Task ResumeGameAsync()
    {
        _gameManager.SessionResume();
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
        _gameManager.ShowMainMenuScreen();
        await Task.CompletedTask;
    }
} 