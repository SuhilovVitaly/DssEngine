using System.Linq;
using DeepSpaceSaga.Common.Abstractions.Dto.Ui;

namespace DeepSpaceSaga.Common.Abstractions.Mappers;

public class GameActionEventMapper
{
    public static GameActionEventDto ToDto(IGameActionEvent gameEvent)
    {
        var dialogsCopy = gameEvent.ConnectedDialogs?.Select(DialogMapper.ToDto).ToList() ?? new List<DialogDto>();

        return new GameActionEventDto
        {
           Key = gameEvent.Key,
           CalculationTurnId = gameEvent.CalculationTurnId,
           CelestialObjectId = gameEvent.CelestialObjectId,
           ConnectedDialogs = dialogsCopy,
           EventType = gameEvent.EventType,
           ModuleId = gameEvent.ModuleId,
           TargetObjectId = gameEvent.TargetObjectId,
           Dialog = gameEvent.Dialog != null ? DialogMapper.ToDto(gameEvent.Dialog) : null,
           Type = gameEvent.Type,
        };
    }
}
