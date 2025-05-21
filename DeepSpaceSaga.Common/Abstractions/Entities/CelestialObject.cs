namespace DeepSpaceSaga.Common.Abstractions.Entities;

public class CelestialObject
{
    public Guid CelestialObjectId { get; set; } = Guid.NewGuid();
    public CelestialObjectType Type { get; set; }
    public bool IsPreScanned { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
}