using log4net;

namespace DeepSpaceSaga.Server;

public class LocalGameServer : IGameServer
{
    private const string REPOSITORY_NAME = "ServerAppRepository";
    private static readonly ILog Logger = LogManager.GetLogger(REPOSITORY_NAME, typeof(LocalGameServer));
    private int _turn = 0;

    public GameSessionDTO TurnCalculation(CalculationType type)
    {
        _turn++;

        Logger?.Debug($"Calculation {type} {_turn}");

        CheckLogs();
        
        return new GameSessionDTO 
        { 
            Id = Guid.NewGuid(), 
            Turn = _turn,
            SpaceMap = new List<int>()
        };
    }

    private void CheckLogs()
    {
        Logger?.Debug($"Check {_turn}");
    }
}
