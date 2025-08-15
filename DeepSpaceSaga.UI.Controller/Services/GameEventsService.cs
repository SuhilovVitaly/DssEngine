using DeepSpaceSaga.Common.Abstractions.Dto.Ui;
using DeepSpaceSaga.Common.Abstractions.Entities;
using log4net;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DeepSpaceSaga.UI.Controller.Services;

public class GameEventsService : IGameEventsService
{
    private readonly IGameManager _gameManager;
    private static readonly ILog Logger = LogManager.GetLogger(typeof(GameEventsService));
    public ConcurrentDictionary<string, string> ReceivedEvents { get; set; } = new();

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

                if (ReceivedEvents.Keys.Contains(gameEvent.Key))
                {
                    continue;
                }

                _gameManager.CommandExecute(new Command
                {
                    Category = Common.Abstractions.Entities.Commands.CommandCategory.CommandAccept,
                    CelestialObjectId = gameEvent.CelestialObjectId,
                    IsOneTimeCommand = true,
                    TargetCelestialObjectId = gameEvent.TargetObjectId,
                });

                ReceivedEvents.TryAdd(gameEvent.Key, gameEvent.Key);

                _gameManager.GameEventInvoke(gameEvent); 
            }            
        }
    }
}
