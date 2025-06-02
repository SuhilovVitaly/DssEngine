using DeepSpaceSaga.Common.Abstractions.Entities;
using log4net;

namespace DeepSpaceSaga.UI.Controller.Services;

public class GameEventsService : IGameEventsService
{
    private readonly IGameManager _gameManager;
    private static readonly ILog Logger = LogManager.GetLogger(typeof(GameEventsService));

    public GameEventsService(IGameManager gameManager)
    {
        _gameManager = gameManager;
        _gameManager.OnUpdateGameData += UpdateGameData;
    }

    public void UpdateGameData(GameSessionDto session)
    {
        if (session.GameActionEvents.Count > 0)
        {
            foreach (var gameEvent in session.GameActionEvents.Values)
            {
                _gameManager.CommandExecute(new Command
                {
                    Category = Common.Abstractions.Entities.Commands.CommandCategory.CommandAccept,
                    CelestialObjectId = gameEvent.Id,
                    IsOneTimeCommand = true,
                    TargetCelestialObjectId = gameEvent.Id,
                });
            }            
        }
    }
}
