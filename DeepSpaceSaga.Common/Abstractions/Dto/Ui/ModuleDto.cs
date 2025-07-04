namespace DeepSpaceSaga.Common.Abstractions.Dto.Ui
{
    public class ModuleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public long OwnerId { get; set; }
        public int Category { get; set; }
        public bool IsPacked { get; set; }
        public double Volume { get; set; }
        public double BasePrice { get; set; }
        public long TargetId { get; set; }
        public bool IsAutoRun { get; set; }
        public bool IsCalculated { get; set; }
        public double ActivationCost { get; set; }
        public bool IsActive { get; set; }
        public int LastReloadTurn { get; set; }
        public bool IsReloaded { get; set; }
        public double ReloadTime { get; set; }
        public double Reloading { get; set; }
    }
}
