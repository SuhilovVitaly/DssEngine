namespace DeepSpaceSaga.Server;

public static class MetricsServer
{
    public static string SessionStart = "Server.Session.Start";
    public static string SessionStop = "Server.Session.Stop";
    public static string SessionResume = "Server.Session.Resume";
    public static string SessionPause = "Server.Session.Pause";
    
    public static string ServerCommandReceived =  "Server.Command.Received";
    
    public static string ServerTurnRealTimeProcessing  = "Server.Turn.RealTime.Processing";
    public static string ServerTurnPauseProcessing  = "Server.Turn.Pause.Processing";
    
    public static string ProcessingEventAcknowledgeReceived = "Server.Processing.EventAcknowledge.Received";
    public static string ProcessingEventAcknowledgeProcessed = "Server.Processing.EventAcknowledge.Processed";
    public static string ProcessingEventAcknowledgeRemoved = "Server.Processing.EventAcknowledge.Removed";
}
