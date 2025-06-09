namespace DeepSpaceSaga.Common.Abstractions.Dto.Ui
{
    public class GameSessionDto
    {
        public Guid Id { get; set; }
        public GameStateDto State { get; set; }
        public Dictionary<int, CelestialObjectDto> CelestialObjects { get; set; } = new();
        public Dictionary<Guid, CommandDto> Commands { get; set; } = new();
        public Dictionary<long, GameActionEventDto> GameActionEvents { get; set; } = new();
        public Dictionary<long, long> FinishedEvents { get; set; } = new();
    }
}
