namespace DeepSpaceSaga.UI.Controller.Screens;

/// <summary>
/// Controller for handling Game Menu business logic
/// </summary>
public class GameMenuController(IGameManager gameManager) : IGameMenuController
{
    private bool _isSessionOnPause = false;
    private readonly IGameManager _gameManager = gameManager ?? throw new ArgumentNullException(nameof(gameManager));
    public async Task LoadAsync()
    {
        _isSessionOnPause = _gameManager.GameSession().State.IsPaused;

        if (_isSessionOnPause == false)
        {
            _gameManager.SessionPause();
        }

        await Task.CompletedTask;
    }

    public async Task ResumeGameAsync()
    {
        if (_isSessionOnPause == false)
        {
            _gameManager.SessionResume();
        }
        
        await Task.CompletedTask;
    }

    public async Task SaveGameAsync()
    {
       await _gameManager.SaveGame("quick.save");
    }

    public async Task LoadGameAsync()
    {
        // Load game logic here
        await _gameManager.LoadGame("quick.save");
    }

    public async Task GoToMainMenuAsync()
    {
        // Stop current session before going to main menu
        _gameManager.SessionStop();
        _gameManager.ShowMainMenuScreen();
        await Task.CompletedTask;
    }
} 