using DeepSpaceSaga.Common.Abstractions.Dto.Ui;
using DeepSpaceSaga.Common.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSpaceSaga.Common.Extensions
{
    public static class GameSessionExtensions
    {
        public static ISpacecraft GetPlayerSpaceShip(Dictionary<int, CelestialObjectDto> spaceMap)
        {
            foreach (var celestialObject in from celestialObject in spaceMap
                                            where celestialObject.Value.Type == CelestialObjectType.SpaceshipPlayer
                                            select celestialObject)
            {
                var spacecraft = celestialObject.Value.ToSpaceship();

                return spacecraft;
            }

            throw new InvalidOperationException("Player spaceship not found in the game session");
        }

        public static ISpacecraft GetPlayerSpaceShip(this GameSessionDto session)
        {
            var spacecraft = GetPlayerSpaceShip(session.CelestialObjects);

            return spacecraft;
        }
    }
}
