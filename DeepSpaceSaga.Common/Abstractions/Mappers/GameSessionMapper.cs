using DeepSpaceSaga.Common.Abstractions.Dto.Save;
using DeepSpaceSaga.Common.Abstractions.Dto.Ui;

namespace DeepSpaceSaga.Common.Abstractions.Mappers;

public static class GameSessionMapper
{
    public static GameSession ToGameObject(GameSessionSaveFormatDto gameSessionDto)
    {
        ConcurrentDictionary<int, ICelestialObject> celestialObjectsCopy;

        lock (gameSessionDto)
        {
            celestialObjectsCopy = new ConcurrentDictionary<int, ICelestialObject>(
                gameSessionDto.CelestialObjects
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => CelestialObjectMapper.ToGameObject(kvp.Value)));
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
        Dictionary<string, GameActionEventDto> gameActionEventsCopy;
        Dictionary<string, string> finishedEventsCopy;

        // Use read lock for better performance when multiple threads are reading
        gameSessionContext.EnterReadLock();
        try
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
        finally
        {
            gameSessionContext.ExitReadLock();
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

    public static GameSessionSaveFormatDto ToSaveFormat(ISessionContextService gameSessionContext)
    {
        Dictionary<int, CelestialObjectDto> celestialObjectsCopy;
        Dictionary<Guid, CommandDto> commandsCopy;
        Dictionary<string, GameActionEventDto> gameActionEventsCopy;
        Dictionary<string, string> finishedEventsCopy;

        // Use read lock for better performance when multiple threads are reading
        gameSessionContext.EnterReadLock();
        try
        {
            celestialObjectsCopy = gameSessionContext.GameSession.CelestialObjects
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => CelestialObjectMapper.ToSaveFormat(kvp.Value));

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
        finally
        {
            gameSessionContext.ExitReadLock();
        }

        return new GameSessionSaveFormatDto
        {
            Id = gameSessionContext.GameSession.Id,
            State = GameStateMapper.ToDto(gameSessionContext),
            CelestialObjects = celestialObjectsCopy,
            FinishedEvents = finishedEventsCopy,
            Commands = commandsCopy
        };
    }
}