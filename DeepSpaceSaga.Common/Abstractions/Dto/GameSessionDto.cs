namespace DeepSpaceSaga.Common.Abstractions.Dto
{
    public class GameSessionDto
    {
        public Guid Id { get; set; }        
        public GameStateDto State { get; set; }
        public Dictionary<Guid, CelestialObjectDto> CelestialObjects { get; set; } = new();
        public Dictionary<Guid, CommandDto> Commands { get; set; } = new();
    }
}
