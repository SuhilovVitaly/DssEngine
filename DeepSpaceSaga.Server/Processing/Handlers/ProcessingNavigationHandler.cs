using DeepSpaceSaga.Common.Abstractions.Entities.Commands;
using DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects.Spacecrafts;
using DeepSpaceSaga.Common.Abstractions.Services;
using DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects;

namespace DeepSpaceSaga.Server.Processing.Handlers;

public class ProcessingNavigationHandler
{
    public void Execute(ISessionContextService sessionContext)
    {
        var processedCommands = new List<Guid>();

        foreach (var command in sessionContext.GameSession.Commands.Values)
        {
            if (command.Category == CommandCategory.Navigation)
            {
                sessionContext.Metrics.Add(MetricsServer.ProcessingCommandNavigationReceived);
                
                ProcessNavigationCommand(command, sessionContext);
                processedCommands.Add(command.Id);
                
                sessionContext.Metrics.Add(MetricsServer.ProcessingCommandNavigationProcessed);
            }
        }

        // Use write lock for modifying session data
        sessionContext.EnterWriteLock();
        try
        {
            foreach (var commandId in processedCommands)
            {
                sessionContext.GameSession.Commands.TryRemove(commandId, out _);
            }
        }
        finally
        {
            sessionContext.ExitWriteLock();
        }
    }

    private void ProcessNavigationCommand(ICommand command, ISessionContextService sessionContext)
    {
        var playerSpaceship = GetPlayerSpaceship(sessionContext);
        if (playerSpaceship == null) return;

        switch (command.Type)
        {
            case CommandTypes.TurnLeft:
                playerSpaceship.SetDirection(playerSpaceship.Direction - 90);
                break;
            case CommandTypes.TurnRight:
                playerSpaceship.SetDirection(playerSpaceship.Direction + 90);
                break;
            case CommandTypes.IncreaseShipSpeed:
                playerSpaceship.ChangeVelocity(10);
                break;
            case CommandTypes.DecreaseShipSpeed:
                playerSpaceship.ChangeVelocity(-10);
                break;
        }
    }

    private ISpacecraft? GetPlayerSpaceship(ISessionContextService sessionContext)
    {
        return sessionContext.GameSession.CelestialObjects.Values
            .OfType<ISpacecraft>()
            .FirstOrDefault(x => x.Type == CelestialObjectType.SpaceshipPlayer);
    }
}
