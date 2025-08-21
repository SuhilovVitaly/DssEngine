using DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects;
using DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects.Spacecrafts;
using DeepSpaceSaga.Common.Abstractions.Dto.Ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSpaceSaga.Common.Extensions
{
    public static class GameSessionExtensions
    {
        public static ISpacecraft GetPlayerSpaceShip(Dictionary<int, CelestialObjectSaveFormatDto> spaceMap)
        {
            var playerSpaceShip = spaceMap.Values.FirstOrDefault(x => x.Type == CelestialObjectType.SpaceshipPlayer);
            if (playerSpaceShip == null)
                return null;

            return new BaseSpaceship(playerSpaceShip);
        }

        public static ISpacecraft GetPlayerSpaceShip(this GameSessionDto session)
        {
            // TOODO: Cust to ICelestialObject and map to ISpacecraft dto?

            var spacecraft = GetPlayerSpaceShip(session.CelestialObjects);

            return spacecraft;
        }
    }
}
