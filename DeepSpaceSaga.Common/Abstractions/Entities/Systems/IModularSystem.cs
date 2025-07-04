namespace DeepSpaceSaga.Common.Abstractions.Entities.Systems;

public interface IModularSystem
{
    List<IModule> GetAll();
    IModule Get(long moduleId);
    List<IModule> GetByCategory(Category category);
    void Add(IModule module);
    ICargoContainer GetCoreContainer { get; set; }
    IModule Module(Category category);
}
