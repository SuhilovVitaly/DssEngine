namespace DeepSpaceSaga.Server.Processing;

public static class BaseProcessing
{
    public static GameSessionDto Process(GameSession sessionContext)
    {
        try
        {
            sessionContext.Turn++;
        
            Console.WriteLine($"[Process] Started processing for session {sessionContext.SessionId} Turn: {sessionContext.Turn}");

            foreach (var celestialObject in sessionContext.CelestialObjects.Values)
            {
                celestialObject.X += 1;
                celestialObject.Y += 1;
                Console.WriteLine($"[Process] Processing celestial object Id is [{celestialObject.CelestialObjectId}] Type is [{celestialObject.Type}] Location is: [{celestialObject.X}:{celestialObject.Y}]");
            }
        
            Console.WriteLine($"[Process] Finish processing for session {sessionContext.SessionId} Turn: {sessionContext.Turn}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        
        return GameSessionMapper.ToDto(sessionContext);
    }
}