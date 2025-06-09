namespace DeepSpaceSaga.Common.Abstractions.Dto.Ui;

public class GameStateDto
{
    public int Cycle { get; set; }
    public int Turn { get; set; }
    public int Tick { get; set; }
    public int ProcessedTurns { get; set; }
    public bool IsPaused { get; set; } = true;
    public int Speed { get; set; } = 1;
}
