namespace DeepSpaceSaga.Common.Abstractions.Dto;

public class CelestialObjectDto
{
    public Guid CelestialObjectId { get; set; }
    public CelestialObjectType Type { get; set; }
    public bool IsPreScanned { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
}