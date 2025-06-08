using DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects;

namespace DeepSpaceSaga.Common.Abstractions.Mappers;

public static class GameSessionMapper
{
    public static GameSession ToGameObject(GameSessionDto gameSessionDto)
    {
        ConcurrentDictionary<int, ICelestialObject> celestialObjectsCopy;

        lock (gameSessionDto)
        {
            celestialObjectsCopy = gameSessionDto.CelestialObjects
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => CelestialObjectMapper.ToGameObject(kvp.Value));
        }

        return new GameSession
        {
            CelestialObjects = celestialObjectsCopy
        };
    }

    public static GameSessionDto ToDto(ISessionContextService gameSessionContext)
    {
        Dictionary<int, CelestialObjectDto> celestialObjectsCopy;
        Dictionary<Guid, CommandDto> commandsCopy;
        Dictionary<long, GameActionEventDto> gameActionEventsCopy;
        Dictionary<long, long> finishedEventsCopy;

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

            gameActionEventsCopy = gameSessionContext.GameSession.ActiveEvents
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => GameActionEventMapper.ToDto(kvp.Value));

            finishedEventsCopy = gameSessionContext.GameSession.FinishedEvents
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value);
        }

        return new GameSessionDto
        {
            Id = gameSessionContext.GameSession.Id,
            State = GameStateMapper.ToDto(gameSessionContext),
            CelestialObjects = celestialObjectsCopy,
            GameActionEvents = gameActionEventsCopy,
            FinishedEvents = finishedEventsCopy,
            Commands = commandsCopy
        };
    }
}