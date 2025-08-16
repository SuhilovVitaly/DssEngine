using DeepSpaceSaga.Common.Implementation.Entities.Commands;

namespace DeepSpaceSaga.UI.Controller.Services;

public class GameEventsService : IGameEventsService
{
    private readonly IGameManager _gameManager;
    private static readonly ILog Logger = LogManager.GetLogger(typeof(GameEventsService));
    private ConcurrentDictionary<string, string> _receivedEvents { get; set; } = new();

    public GameEventsService(IGameManager gameManager)
    {
        _receivedEvents = new();
        _gameManager = gameManager;
        _gameManager.OnUpdateGameData += UpdateGameData;
    }

    public void UpdateGameData(GameSessionDto session)
    {
        if (session.GameActionEvents.Count > 0)
        {
            foreach (var gameEvent in session.GameActionEvents.Values)
            {

                if (_receivedEvents.Keys.Contains(gameEvent.Key))
                {
                    continue;
                }

                _gameManager.CommandExecute(new GameEventReceivedCommand
                {
                    Category = Common.Abstractions.Entities.Commands.CommandCategory.CommandAccept,
                    CelestialObjectId = gameEvent.CelestialObjectId,
                    IsOneTimeCommand = true,
                    TargetCelestialObjectId = gameEvent.TargetObjectId,
                    DialogKey = gameEvent.Key,
                });

                _receivedEvents.TryAdd(gameEvent.Key, gameEvent.Key);

                _gameManager.GameEventInvoke(gameEvent); 
            }            
        }
    }
}
