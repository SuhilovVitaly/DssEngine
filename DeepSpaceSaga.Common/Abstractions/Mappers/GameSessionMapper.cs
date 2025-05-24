namespace DeepSpaceSaga.Common.Abstractions.Mappers;

public static class GameSessionMapper
{
    public static GameSessionDto ToDto(ISessionContextService gameSessionContext)
    {
        Dictionary<int, CelestialObjectDto> celestialObjectsCopy;
        Dictionary<Guid, CommandDto> commandsCopy;
            
        lock (gameSessionContext)
        {
            celestialObjectsCopy = gameSessionContext.GameSession.CelestialObjects
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => CelestialObjectMapper.ToDto(kvp.Value));
                
            commandsCopy = gameSessionContext.GameSession.Commands
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => CommandMapper.ToDto(kvp.Value));
        }

        return new GameSessionDto
        {
            Id = gameSessionContext.GameSession.Id,
            State = GameStateMapper.ToDto(gameSessionContext),
            CelestialObjects = celestialObjectsCopy,
            Commands = commandsCopy
        };
    }
}