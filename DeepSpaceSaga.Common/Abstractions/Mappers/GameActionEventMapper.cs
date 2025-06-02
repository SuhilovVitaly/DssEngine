using System.Linq;

namespace DeepSpaceSaga.Common.Abstractions.Mappers;

public class GameActionEventMapper
{
    public static GameActionEventDto ToDto(IGameActionEvent gameEvent)
    {
        List<DialogDto> dialogsCopy = new();

        if(gameEvent.ConnectedDialogs != null)
        {
            foreach (var dialog in gameEvent.ConnectedDialogs)
            {
                dialogsCopy.Add(DialogMapper.ToDto(dialog));
            }
        }        

        return new GameActionEventDto
        {
           Id = gameEvent.Id,
           CalculationTurnId = gameEvent.CalculationTurnId,
           CelestialObjectId = gameEvent.CelestialObjectId,
           ConnectedDialogs = dialogsCopy,
           EventType = gameEvent.EventType,
           ModuleId = gameEvent.ModuleId,
           TargetObjectId = gameEvent.TargetObjectId,
           Dialog = gameEvent.Dialog != null ? DialogMapper.ToDto(gameEvent.Dialog) : null,
        };
    }
}
