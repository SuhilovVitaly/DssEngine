using DeepSpaceSaga.Common.Abstractions.Dto.Ui;

namespace DeepSpaceSaga.Common.Abstractions.Dto.Save
{
    public class GameSessionSaveFormatDto
    {
        public Guid Id { get; set; }
        public GameStateDto State { get; set; }
        public Dictionary<int, CelestialObjectSaveFormatDto> CelestialObjects { get; set; } = new();
        public Dictionary<Guid, CommandDto> Commands { get; set; } = new();
        public Dictionary<long, GameActionEventDto> GameActionEvents { get; set; } = new();
        public Dictionary<long, long> FinishedEvents { get; set; } = new();
    }
}
