namespace DeepSpaceSaga.Common.Abstractions.Mappers;

public static class GameSessionMapper
{
    public static GameSessionDto ToDto(Entities.GameSession gameSessionContext)
    {
        Dictionary<Guid, CelestialObjectDto> celestialObjectsCopy;
        Dictionary<Guid, CommandDto> commandsCopy;
            
        lock (gameSessionContext)
        {
            celestialObjectsCopy = gameSessionContext.CelestialObjects
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => CelestialObjectMapper.ToDto(kvp.Value));
                
            commandsCopy = gameSessionContext.Commands
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => CommandMapper.ToDto(kvp.Value));
        }

        return new GameSessionDto
        {
            CelestialObjects = celestialObjectsCopy,
            Commands = commandsCopy
        };
    }
}