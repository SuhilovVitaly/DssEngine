using DeepSpaceSaga.UI.Controller.Abstractions;
using DeepSpaceSaga.UI.Controller.Models;
using DeepSpaceSaga.Common.Abstractions.Services;
using DeepSpaceSaga.Common.Abstractions.Entities;

namespace DeepSpaceSaga.UI.Controller;

/// <summary>
/// Controller for Main Menu screen handling business logic and coordination
/// </summary>
public class MainMenuController : IMainMenuController
{
    private readonly IGameServer _gameServer;
    private readonly IScreensService _screensService;
    
    public MainMenuController(IGameServer gameServer, IScreensService screensService)
    {
        _gameServer = gameServer ?? throw new ArgumentNullException(nameof(gameServer));
        _screensService = screensService ?? throw new ArgumentNullException(nameof(screensService));
    }

    /// <summary>
    /// Starts a new game session
    /// </summary>
    public async Task StartNewGameAsync()
    {
        try
        {
            Console.WriteLine("[MainMenuController] Starting new game session...");
            
            // Start new game session
            var newGameSession = new GameSession();
            _gameServer.SessionStart(newGameSession);
            Console.WriteLine($"[MainMenuController] Game session started with ID: {newGameSession.Id}");
            
            // Navigate to tactical map screen
            _screensService.ShowTacticalMapScreen();
            Console.WriteLine("[MainMenuController] Navigated to tactical map screen");
            
            // Simulate async operation
            await Task.CompletedTask;
            Console.WriteLine("[MainMenuController] New game startup completed");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[MainMenuController] Failed to start new game: {ex.Message}");
            throw new InvalidOperationException($"Failed to start new game: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Loads an existing game
    /// </summary>
    public async Task LoadGameAsync()
    {
        try
        {
            Console.WriteLine("[MainMenuController] Load game requested");
            
            // TODO: Implement load game functionality
            // Check if saved games exist
            // Load selected game
            // Navigate to appropriate screen
            
            await Task.CompletedTask;
            throw new NotImplementedException("Load game functionality not yet implemented");
        }
        catch (Exception ex) when (!(ex is NotImplementedException))
        {
            Console.WriteLine($"[MainMenuController] Failed to load game: {ex.Message}");
            throw new InvalidOperationException($"Failed to load game: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Exits the application
    /// </summary>
    public async Task ExitApplicationAsync()
    {
        try
        {
            Console.WriteLine("[MainMenuController] Application exit requested");
            
            // TODO: Perform any cleanup operations
            // Save any pending data
            // Close connections
            
            await Task.CompletedTask;
            
            // Exit application
            Console.WriteLine("[MainMenuController] Exiting application...");
            Environment.Exit(0);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[MainMenuController] Failed to exit application: {ex.Message}");
            throw new InvalidOperationException($"Failed to exit application: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Checks if loading games is available
    /// </summary>
    public async Task<bool> IsLoadGameAvailableAsync()
    {
        try
        {
            // TODO: Check if there are saved games available
            // Look for save files in designated directory
            // Validate save files integrity
            
            await Task.CompletedTask;
            return false; // For now, always return false as feature is not implemented
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[MainMenuController] Failed to check load game availability: {ex.Message}");
            // In case of error, assume load is not available
            throw new InvalidOperationException($"Failed to check load game availability: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Gets game information for display
    /// </summary>
    public async Task<GameInfoModel> GetGameInfoAsync()
    {
        try
        {
            var isLoadAvailable = await IsLoadGameAvailableAsync();
            
            return new GameInfoModel(
                gameTitle: "Deep Space Saga",
                versionInfo: "Version 1.0.0",
                isLoadGameEnabled: isLoadAvailable,
                statusMessage: isLoadAvailable ? "Saved games found" : "No saved games available"
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[MainMenuController] Failed to get game info: {ex.Message}");
            // Return default info in case of error
            return new GameInfoModel(
                gameTitle: "Deep Space Saga",
                versionInfo: "Version 1.0.0",
                isLoadGameEnabled: false,
                statusMessage: $"Error: {ex.Message}"
            );
        }
    }
} 