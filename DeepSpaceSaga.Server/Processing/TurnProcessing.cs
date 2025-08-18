using DeepSpaceSaga.Common.Abstractions.Dto.Ui;

namespace DeepSpaceSaga.Server.Processing;

public class TurnProcessing : IProcessingService
{
    public GameSessionDto PauseProcess(ISessionContextService sessionContext)
    {
        sessionContext.Metrics.Add(MetricsServer.ServerTurnPauseProcessing);
        
        try
        {
            new ProcessingEventAcknowledgeHandler().Execute(sessionContext);
            new ProcessingDialogHandler().Execute(sessionContext);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        var turnResult = GameSessionMapper.ToDto(sessionContext);

        return turnResult;
    }

    public GameSessionDto Process(ISessionContextService sessionContext)
    {
        sessionContext.Metrics.Add(MetricsServer.ServerTurnRealTimeProcessing);
        
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