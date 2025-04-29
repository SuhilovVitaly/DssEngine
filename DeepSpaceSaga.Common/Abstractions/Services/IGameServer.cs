using DeepSpaceSaga.Common.Implementation;
using DeepSpaceSaga.Common.Implementation.GameLoop;

namespace DeepSpaceSaga.Common.Abstractions.Services
{
    public interface IGameServer
    {
        public GameSessionDTO TurnCalculation(CalculationType type);
    }
}
