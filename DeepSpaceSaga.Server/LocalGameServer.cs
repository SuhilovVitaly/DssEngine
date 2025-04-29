namespace DeepSpaceSaga.Server;

public class LocalGameServer : IGameServer
{
    private int _turn = 0;

    public GameSessionDTO TurnCalculation(CalculationType type)
    {
        _turn++;

        return new GameSessionDTO 
        { 
            Id = Guid.NewGuid(), 
            Turn = _turn,
            SpaceMap = new List<int>()
        };
    }
}
