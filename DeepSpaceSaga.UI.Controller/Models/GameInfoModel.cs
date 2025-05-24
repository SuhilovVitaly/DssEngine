namespace DeepSpaceSaga.UI.Controller.Models;

/// <summary>
/// Model containing game information for display purposes
/// </summary>
public class GameInfoModel
{
    /// <summary>
    /// Title of the game
    /// </summary>
    public string GameTitle { get; set; } = "Deep Space Saga";

    /// <summary>
    /// Version information
    /// </summary>
    public string VersionInfo { get; set; } = "Version 1.0.0";

    /// <summary>
    /// Indicates whether load game functionality is available
    /// </summary>
    public bool IsLoadGameEnabled { get; set; } = false;

    /// <summary>
    /// Additional game status information
    /// </summary>
    public string? StatusMessage { get; set; }

    /// <summary>
    /// Constructor with default values
    /// </summary>
    public GameInfoModel()
    {
    }

    /// <summary>
    /// Constructor with custom values
    /// </summary>
    /// <param name="gameTitle">Game title</param>
    /// <param name="versionInfo">Version information</param>
    /// <param name="isLoadGameEnabled">Load game availability</param>
    /// <param name="statusMessage">Optional status message</param>
    public GameInfoModel(string gameTitle, string versionInfo, bool isLoadGameEnabled, string? statusMessage = null)
    {
        GameTitle = gameTitle;
        VersionInfo = versionInfo;
        IsLoadGameEnabled = isLoadGameEnabled;
        StatusMessage = statusMessage;
    }
} 