namespace DeepSpaceSaga.Common.Abstractions.Dto;

public class CelestialObjectDto
{
    public int Id { get; set; }
    public CelestialObjectType Type { get; set; }
    public bool IsPreScanned { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
}