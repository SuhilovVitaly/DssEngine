namespace DeepSpaceSaga.Common.Abstractions.Services;

public interface IGameServer
{
    public GameSessionDTO TurnCalculation(CalculationType type);
}
