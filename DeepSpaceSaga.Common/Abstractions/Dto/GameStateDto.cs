namespace DeepSpaceSaga.Common.Abstractions.Dto;

public class GameStateDto
{
    public bool IsPaused { get; set; } = true;

    public int Speed { get; set; } = 1;
}
