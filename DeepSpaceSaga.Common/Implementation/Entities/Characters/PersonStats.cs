namespace DeepSpaceSaga.Common.Implementation.Entities.Characters;

public class PersonStats
{
    /// <summary>
    /// Health Points
    /// </summary>
    public int Health { get; set; } = 100;
    /// <summary>
    /// Energy Points
    /// </summary>
    public int Energy { get; set; } = 100;
    /// <summary>
    /// Pain Level
    /// </summary>
    public int Pain { get; set; } = 0;
    /// <summary>
    /// Stability / Mobility
    /// </summary>
    public int Stability { get; set; } = 1;

    public int Morale { get; set; } = 100;
}
