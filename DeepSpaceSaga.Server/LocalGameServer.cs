using DeepSpaceSaga.Common.Abstractions.Session.Entities;

namespace DeepSpaceSaga.Server;

public class LocalGameServer : IGameServer
{
    private static readonly ILog Logger = LogManager.GetLogger(Settings.LoggerRepository, typeof(LocalGameServer));
    private readonly ISessionInfo _sessionInfo;
    private readonly object _lockObject = new();

    public LocalGameServer(ISessionInfo sessionInfo)
    {
        _sessionInfo = sessionInfo;
    }

    public GameSessionDTO TurnCalculation(CalculationType type)
    {
        Logger?.Debug($"Calculation {type} {_sessionInfo.Turn}");

        var newTurn = _sessionInfo.IncrementTurn();

        return new GameSessionDTO 
        { 
            Id = Guid.NewGuid(), 
            Turn = newTurn,
            SpaceMap = []
        };
    }

    private void UpdateSessionInfo(ISessionInfo sessionInfo)
    {
        sessionInfo.IncrementTurn();
    }

    private GameSessionDTO GameSessionMap(ISessionInfo sessionInfo)
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
