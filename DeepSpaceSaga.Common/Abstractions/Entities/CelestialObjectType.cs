using System.ComponentModel;

namespace DeepSpaceSaga.Common.Abstractions.Entities;

[Flags]
public enum CelestialObjectType
{
    Unknown = -1000,
    PointInMap = -1,
    [Description("Asteroid")]
    Asteroid = 1,
    [Description("Station")]
    Station = 100,
    [Description("Spaceship")]
    SpaceshipPlayer = 200,
    [Description("Spaceship")]
    SpaceshipNpcNeutral = 201,
    [Description("Spaceship")]
    SpaceshipNpcEnemy = 202,
    [Description("Spaceship")]
    SpaceshipNpcFriend = 203,
    [Description("Missile")]
    Missile = 300,
    Explosion = 800,
    Container = 1000
}