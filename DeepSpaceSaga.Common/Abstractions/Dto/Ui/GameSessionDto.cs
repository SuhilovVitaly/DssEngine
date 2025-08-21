﻿namespace DeepSpaceSaga.Common.Abstractions.Dto.Ui
{
    public class GameSessionDto
    {
        public Guid Id { get; set; }
        public GameStateDto State { get; set; }
        public Dictionary<int, CelestialObjectSaveFormatDto> CelestialObjects { get; set; } = new();
        public Dictionary<Guid, CommandDto> Commands { get; set; } = new();
        public Dictionary<string, GameActionEventDto> GameActionEvents { get; set; } = new();
        public Dictionary<string, string> FinishedEvents { get; set; } = new();
    }
}
