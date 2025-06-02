using DeepSpaceSaga.Common.Tools;
using DeepSpaceSaga.Common.Abstractions.UI;

namespace DeepSpaceSaga.Common.Abstractions.Services;

/// <summary>
/// Interface for Game Manager handling core game operations
/// </summary>
public interface IGameManager
{
    /// <summary>
    /// Event fired when game data is updated
    /// </summary>
    event Action<GameSessionDto>? OnUpdateGameData;

    /// <summary>
    /// Generation tool for creating game content
    /// </summary>
    IGenerationTool GenerationTool { get; set; }

    /// <summary>
    /// Screens service for managing UI screens
    /// </summary>
    IScreensService Screens { get; set; }

    /// <summary>
    /// Screen information and parameters
    /// </summary>
    IScreenInfo ScreenInfo { get; set; }

    /// <summary>
    /// Outer space service for space-related operations
    /// </summary>
    IOuterSpaceService OuterSpace { get; set; }

    /// <summary>
    /// Current mouse position on tactical map
    /// </summary>
    SpaceMapPoint TacticalMapMousePosition { get; }

    /// <summary>
    /// Sets the game speed
    /// </summary>
    /// <param name="speed">Game speed value</param>
    void SetGameSpeed(int speed);

    /// <summary>
    /// Starts a new game session
    /// </summary>
    void SessionStart();

    /// <summary>
    /// Pauses the current game session
    /// </summary>
    void SessionPause();

    /// <summary>
    /// Resumes the current game session
    /// </summary>
    void SessionResume();

    /// <summary>
    /// Stops the current game session
    /// </summary>
    void SessionStop();

    /// <summary>
    /// Shows the tactical map screen
    /// </summary>
    void ShowTacticalMapScreen();

    /// <summary>
    /// Shows the main menu screen
    /// </summary>
    void ShowMainMenuScreen();

    /// <summary>
    /// Handles mouse movement on tactical map
    /// </summary>
    /// <param name="coordinatesGame">Game coordinates</param>
    /// <param name="coordinatesScreen">Screen coordinates</param>
    void TacticalMapMouseMove(SpaceMapPoint coordinatesGame, SpaceMapPoint coordinatesScreen);

    /// <summary>
    /// Handles mouse click on tactical map
    /// </summary>
    /// <param name="coordinates">Click coordinates</param>
    void TacticalMapMouseClick(SpaceMapPoint coordinates);

    /// <summary>
    /// Handles left mouse click on tactical map
    /// </summary>
    /// <param name="mouseLocation">Mouse location</param>
    void TacticalMapLeftMouseClick(SpaceMapPoint mouseLocation);
} 