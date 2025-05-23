namespace DeepSpaceSaga.Common.Abstractions.Entities;

public class GameState
{
    public bool IsPaused { get; set; } = true;

    public int Speed { get; set; } = 1;
}
