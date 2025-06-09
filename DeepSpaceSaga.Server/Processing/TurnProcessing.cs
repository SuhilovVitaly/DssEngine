using DeepSpaceSaga.Common.Abstractions.Dto.Ui;

namespace DeepSpaceSaga.Server.Processing;

public class TurnProcessing : IProcessingService
{
    public GameSessionDto Process(ISessionContextService sessionContext)
    {
        try
        {
            new ProcessingEventAcknowledgeHandler().Execute(sessionContext);
            new ProcessingLocationsHandler().Execute(sessionContext);     
            new ProcessingEventInvokerHandler().Execute(sessionContext);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        
        var turnResult = GameSessionMapper.ToDto(sessionContext);

        return turnResult;
    }
}