namespace DeepSpaceSaga.Server;

public static class MetricsServer
{
    public const string SessionStart = "Server.Session.Start";
    public const string SessionStop = "Server.Session.Stop";
    public const string SessionResume = "Server.Session.Resume";
    public const string SessionPause = "Server.Session.Pause";
    
    public const string ServerCommandReceived =  "Server.Command.Received";
    
    public const string ServerTurnRealTimeProcessing  = "Server.Turn.RealTime.Processing";
    public const string ServerTurnPauseProcessing  = "Server.Turn.Pause.Processing";
    
    public const string ProcessingEventAcknowledgeReceived = "Server.Processing.EventAcknowledge.Received";
    public const string ProcessingEventAcknowledgeProcessed = "Server.Processing.EventAcknowledge.Processed";
    public const string ProcessingEventAcknowledgeRemoved = "Server.Processing.EventAcknowledge.Removed";
}
