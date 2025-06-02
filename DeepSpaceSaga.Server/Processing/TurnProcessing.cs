namespace DeepSpaceSaga.Server.Processing;

public class TurnProcessing : IProcessingService
{
    public GameSessionDto Process(ISessionContextService sessionContext)
    {
        try
        {
            new ProcessingLocationsHandler().Execute(sessionContext);     
            new ProcessingEventInvokerHandler().Execute(sessionContext);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        
        return GameSessionMapper.ToDto(sessionContext);
    }
}