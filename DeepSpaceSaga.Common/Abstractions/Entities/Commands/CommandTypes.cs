using System.ComponentModel;

namespace DeepSpaceSaga.Common.Abstractions.Entities.Commands;

public enum CommandTypes
{
    [Description("")]
    MoveForward = 200,
    [Description("Turn Left")]
    TurnLeft = 201,
    [Description("Turn Right")]
    TurnRight = 202,
    [Description("Rotate to Target")]
    RotateToTarget = 203,
    [Description("Decrease Ship Speed")]
    DecreaseShipSpeed = 204,
    [Description("Increase Ship Speed")]
    IncreaseShipSpeed = 205,
    [Description("Sync speed with Target")]
    SyncSpeedWithTarget = 216,
    [Description("Sync direction with Target")]
    SyncDirectionWithTarget = 217,
    [Description("Dock spacecraft to station")]
    Dock = 218,
    [Description("")]
    StopShip = 233,
    FullSpeed = 234,
    [Description("")]
    Acceleration = 244,
    [Description("")]
    Fire = 300,
    [Description("")]
    AlignTo = 100,
    [Description("")]
    Orbit = 110,
    [Description("")]
    Explosion = 800,
    [Description("")]
    ReloadWeapon = 900,
    [Description("")]
    Scanning = 1600,
    [Description("")]
    Shot = 2001,
    [Description("[Scanning] Pre scan celestial object")]
    PreScanCelestialObject = 3001,
    PreScanCelestialObjectFinished = 3002,
    [Description("Object detected")]
    GenerateAsteroid = 5001,
    EventReceipt = 7001,
    MiningOperationsHarvest = 8001,
    MiningOperationsResult = 8010,
    MiningOperationsDestroyAsteroid = 8020,
    CargoOperationsShow = 9001,
    CargoOperationsTransfer = 9005,
    CargoOperationsSell = 9006,
    UiSelectCelestialObject = 12001,
    DialogInitiationByTurn = 20001,
    //---- Crew Interaction Commands
    HireRecrut = 30001
}

public static class CommandTypesExtensions
{
    public static int ToInt(this CommandTypes command)
    {
        return (int)command;
    }
}