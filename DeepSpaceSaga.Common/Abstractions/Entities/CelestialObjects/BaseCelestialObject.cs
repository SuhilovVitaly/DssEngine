namespace DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects;

public class BaseCelestialObject : ICelestialObject
{
    public int Id { get; set; }
    public int OwnerId { get; set; }
    public string Name { get; set; }
    public double Direction { get; set; }
    public double Speed { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
    public CelestialObjectType Type { get; set; } = CelestialObjectType.Unknown;
    public bool IsPreScanned { get; set; }
    public float Size { get; set; }

    public void LoadObject( CelestialObjectSaveFormatDto celestialObjectDto)
    {
        Id = celestialObjectDto.Id;
        OwnerId = celestialObjectDto.OwnerId;
        Name = celestialObjectDto.Name;
        Direction = celestialObjectDto.Direction;
        Speed = celestialObjectDto.Speed;
        X = celestialObjectDto.X;
        Y = celestialObjectDto.Y;
        Type = celestialObjectDto.Type;
        IsPreScanned = celestialObjectDto.IsPreScanned;
        Size = celestialObjectDto.Size;
    }
}
