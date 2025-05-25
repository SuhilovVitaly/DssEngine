namespace DeepSpaceSaga.Server.Processing.Handlers;

public class ProcessingLocationsHandler
{
    private static int interval = 32;

    public void Execute(ISessionContextService sessionContext)
    {
        foreach (var celestialObject in sessionContext.GameSession.CelestialObjects.Values)
        {
            RecalculateOneTickObjectLocation(sessionContext, celestialObject);
        }
    }

    private void RecalculateOneTickObjectLocation(ISessionContextService context, ICelestialObject celestialObject)
    {
        double tickSpeed = celestialObject.Speed / (1000 / interval);

        tickSpeed *= context.SessionInfo.Speed;

        var position = GeometryTools.Move(
            celestialObject.GetLocation(),
            tickSpeed,
            celestialObject.Direction);

        celestialObject.X = position.X;
        celestialObject.Y = position.Y;
    }
}
