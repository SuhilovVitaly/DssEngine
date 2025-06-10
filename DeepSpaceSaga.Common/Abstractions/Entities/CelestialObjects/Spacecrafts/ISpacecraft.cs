namespace DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects.Spacecrafts;

public interface ISpacecraft : ICelestialObject
{
    float MaxSpeed { get; set; }
    float Agility { get; set; }
    List<ICharacter> Crew { get; }
    void SetDirection(double direction);
    void ChangeVelocity(double delta);
    void AddCrewMember(ICharacter person);
    int GetCrewCapacity();
}
