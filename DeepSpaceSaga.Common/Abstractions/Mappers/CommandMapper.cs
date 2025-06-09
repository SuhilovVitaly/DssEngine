using DeepSpaceSaga.Common.Abstractions.Dto.Ui;
using DeepSpaceSaga.Common.Abstractions.Entities.Equipment;
using System.Reflection;

namespace DeepSpaceSaga.Common.Abstractions.Mappers;

public static class CommandMapper
{
    public static CommandDto ToDto(ICommand command)
    {
        return new CommandDto
        {
            CommandId = command.Id,
            Category = command.Category,
            Type = command.Type,
            Status = command.Status,
            CelestialObjectId = command.CelestialObjectId,
            MemberId = command.MemberId,
            TargetCelestialObjectId = command.TargetCelestialObjectId,
            ModuleId = command.ModuleId,
            IsOneTimeCommand = command.IsOneTimeCommand,
            IsUnique = command.IsUnique,
            IsPauseProcessed = command.IsPauseProcessed
        };
    }
}