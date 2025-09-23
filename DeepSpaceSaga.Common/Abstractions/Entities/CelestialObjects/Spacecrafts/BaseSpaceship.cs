namespace DeepSpaceSaga.Common.Abstractions.Entities.CelestialObjects.Spacecrafts;

public class BaseSpaceship : BaseCelestialObject, ISpacecraft
{
    public float MaxSpeed { get; set; }
    public float Agility { get; set; }
    public List<ICharacter> Crew { get; }
    public void SetDirection(double direction) => Direction = direction;

    public BaseSpaceship()
    {
        Crew = new();
    }

    public BaseSpaceship(CelestialObjectDto celestialObjectDto) : this()
    {
        LoadObject(celestialObjectDto);
    }

    public void ChangeVelocity(double delta)
    {
        var updatedVelocity = Speed + delta;

        if (updatedVelocity > MaxSpeed)
        {
            updatedVelocity = MaxSpeed;
        }

        if (updatedVelocity < 0)
        {
            updatedVelocity = 0;
        }

        Speed = updatedVelocity;
    }

    public void AddCrewMember(ICharacter person)
    {
        Crew.Add(person);
    }

    public ICharacter GetCrewMember(int id)
    {
        return Crew.Where(x => x.Id == id).FirstOrDefault();
    }

    public int GetCrewCapacity()
    {
        var crewCapacity = 0;

        /*
        foreach (var module in ModulesS.GetByCategory(Category.LivingQuarters))
        {
            var livingQuarter = module as ILivingQuarters;

            if (livingQuarter != null)
            {
                crewCapacity = crewCapacity + livingQuarter.MaxCapacity;
            }
        }
        */

        return crewCapacity;
    }

}
