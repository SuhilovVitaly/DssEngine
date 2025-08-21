namespace DeepSpaceSaga.Common.Abstractions.Dto.Ui;

public class CelestialObjectDto
{
    public int Id { get; set; }
    public int OwnerId { get; set; }
    public required string Name { get; set; }
    public double Direction { get; set; }
    public double Speed { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
    public CelestialObjectType Type { get; set; } = CelestialObjectType.Unknown;
    public bool IsPreScanned { get; set; }
    public float Size { get; set; }

    public List<ModuleDto> Modules { get; set; } = new List<ModuleDto>();
    // Asteroid fields
    public int RemainingDrillAttempts { get; set; }
}