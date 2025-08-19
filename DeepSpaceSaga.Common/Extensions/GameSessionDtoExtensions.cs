using DeepSpaceSaga.Common.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSpaceSaga.Common.Extensions
{
    public static class GameSessionDtoExtensions
    {
        public static ISpacecraft GetPlayerSpaceShip(ConcurrentDictionary<int, ICelestialObject> spaceMap)
        {
            foreach (var celestialObject in from celestialObject in spaceMap
                                            where celestialObject.Value.Type == CelestialObjectType.SpaceshipPlayer
                                            select celestialObject)
            {
                return celestialObject.Value.ToSpaceship();
            }

            throw new InvalidOperationException("Player spaceship not found in the game session");
        }

        public static ISpacecraft GetPlayerSpaceShip(this GameSession session)
        {
            return GetPlayerSpaceShip(session.CelestialObjects);
        }
    }
}
