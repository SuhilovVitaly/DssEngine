using DeepSpaceSaga.Common.Abstractions.Services;
using DeepSpaceSaga.Common.Implementation;

namespace DeepSpaceSaga.Server
{
    public class LocalGameServer: IGameServer
    {
        private int _turn = 0;

        public GameSessionDTO TurnCalculation()
        {
            _turn++;

            return new GameSessionDTO { Id = new Guid() , Turn = _turn};
        }
    }
}
