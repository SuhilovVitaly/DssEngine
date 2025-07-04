using DeepSpaceSaga.Common.Abstractions.Entities.Equipment.Commands;

namespace DeepSpaceSaga.Common.Implementation.Entities.Equipment;

public abstract class AbstractModule : AbstractItem
{
    public string Code { get; set; }
    public long TargetId { get; set; }
    public double Power { get; set; }
    public double ActivationCost { get; set; }
    public bool IsAutoRun { get; set; } = false;
    public bool IsCalculated { get; set; } = true;
    public bool IsActive { get; set; }
    public double ReloadTime { get; set; }
    public int LastReloadTurn { get; set; }
    public double Reloading { get; set; }
    public bool IsReloaded { get; set; } = true;

    public List<IModuleOperation> Operations { get; set; } = new List<IModuleOperation>();

    public void Execute()
    {
        IsCalculated = false;
        Reloading = 0;
        IsReloaded = false;
    }

    public void Reload(double progress, int turn = 0)
    {
        Reloading += progress;

        if (Reloading >= ReloadTime)
        {
            LastReloadTurn = turn;
            IsReloaded = true;
        }
    }
}
