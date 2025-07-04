namespace DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects;

public interface ICelestialObject
{
    int Id { get; set; }
    int OwnerId { get; set; }
    string Name { get; set; }
    double Direction { get; set; }
    double Speed { get; set; }
    double X { get; set; }
    double Y { get; set; }
    CelestialObjectType Type { get; set; }
    bool IsPreScanned { get; set; }
    float Size { get; set; }

    IModularSystem ModulesS { get; }
}
