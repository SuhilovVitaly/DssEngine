using DeepSpaceSaga.Common.Abstractions.Dto.Ui;

namespace DeepSpaceSaga.Common.Abstractions.Mappers
{
    public class DialogMapper
    {
        public static DialogDto ToDto(IDialog dialog)
        {
            return new DialogDto
            {
                ChainPart = dialog.ChainPart,
                Exits = dialog.Exits,
                Key = dialog.Key,
                Message = dialog.Message,
                Reporter = dialog.Reporter,
                Title = dialog.Title,
                Trigger = dialog.Trigger,
                TriggerValue = dialog.TriggerValue,
                Type = dialog.Type,
                UiScreenType = dialog.UiScreenType
            };
        }
    }
}
