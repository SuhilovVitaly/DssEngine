using DeepSpaceSaga.Common.Abstractions.Services;

namespace DeepSpaceSaga.Common.Abstractions.Mappers;

public static class GameStateMapper
{
    public static GameStateDto ToDto(ISessionContextService gameSessionContext)
    {
        return new GameStateDto
        {
            IsPaused = gameSessionContext.SessionInfo.IsPaused,
            Speed = gameSessionContext.SessionInfo.Speed
        };
    }
}
