namespace DeepSpaceSaga.Server.Processing;

public static class BaseProcessing
{
    public static GameSessionDto Process(ISessionContextService sessionContext)
    {
        try
        {
            sessionContext.GameSession.Turn++;
        
            Console.WriteLine($"[Process] Started processing for session {sessionContext.GameSession.SessionId} Turn: {sessionContext.GameSession.Turn}");

            foreach (var celestialObject in sessionContext.GameSession.CelestialObjects.Values)
            {
                celestialObject.X += 1;
                celestialObject.Y += 1;
                Console.WriteLine($"[Process] Processing celestial object Id is [{celestialObject.Id}] Type is [{celestialObject.Type}] Location is: [{celestialObject.X}:{celestialObject.Y}]");
            }
        
            Console.WriteLine($"[Process] Finish processing for session {sessionContext.GameSession.SessionId} Turn: {sessionContext.GameSession.Turn}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        
        return GameSessionMapper.ToDto(sessionContext);
    }
}