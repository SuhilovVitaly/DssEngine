using DeepSpaceSaga.Common.Abstractions.Session.Entities;

namespace DeepSpaceSaga.Server;

public class LocalGameServer : IGameServer
{
    private static readonly ILog Logger = LogManager.GetLogger(Settings.LoggerRepository, typeof(LocalGameServer));
    private readonly SessionInfo _sessionInfo;
    private readonly object _lockObject = new();

    public LocalGameServer()
    {
        _sessionInfo = new SessionInfo
        {
            Turn = 0,
            State = SessionState.NotStarted
        };
    }

    public GameSessionDTO TurnCalculation(CalculationType type)
    {
        var sessionInfo = UpdateSessionInfo(_sessionInfo);

        Logger?.Debug($"Calculation {type} {sessionInfo.Turn}");

        return GameSessionMap(sessionInfo);
    }

    private SessionInfo UpdateSessionInfo(SessionInfo sessionInfo)
    {
        SessionInfo returnSessionInfo;
        
        lock (_lockObject)
        {
            sessionInfo.Turn++;
            returnSessionInfo = sessionInfo.DeepClone()!;
        }

        return returnSessionInfo;
    }

    private GameSessionDTO GameSessionMap(SessionInfo sessionInfo)
    {
        Logger?.Debug($"GameSessionMap {sessionInfo.Turn}");
        
        return new GameSessionDTO 
        { 
            Id = Guid.NewGuid(), 
            Turn = sessionInfo!.Turn,
            SpaceMap = []
        };
    }
}
