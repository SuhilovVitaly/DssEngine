using DeepSpaceSaga.Common.Abstractions.Dto.Ui;

namespace DeepSpaceSaga.Common.Abstractions.Mappers;

public static class GameStateMapper
{
    public static GameStateDto ToDto(ISessionContextService gameSessionContext)
    {
        return new GameStateDto
        {
            ProcessedTurns = gameSessionContext.SessionInfo.Turn,
            Cycle = gameSessionContext.SessionInfo.CycleCounter,
            Turn = gameSessionContext.SessionInfo.TurnCounter,
            Tick = gameSessionContext.SessionInfo.TickCounter,
            IsPaused = gameSessionContext.SessionInfo.IsPaused,
            Speed = gameSessionContext.SessionInfo.Speed
        };
    }
}
