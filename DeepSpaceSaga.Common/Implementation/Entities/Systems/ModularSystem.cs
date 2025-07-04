namespace DeepSpaceSaga.Common.Implementation.Entities.Systems
{
    public class ModularSystem : IModularSystem
    {
        private List<IModule> Modules { get; set; } = new List<IModule>();

        public void Add(IModule module)
        {
            Modules.Add(module);
        }

        public IModule Get(long moduleId)
        {
            return Modules.FirstOrDefault(module => module.Id == moduleId);
        }

        public List<IModule> GetAll()
        {
            return Modules;
        }

        public List<IModule> GetByCategory(Category category)
        {
            return Modules.Where(module => module.Category == category).Cast<IModule>().ToList();
        }

        public IModule Module(Category category)
        {
            return GetByCategory(category).FirstOrDefault();
        }

        public ICargoContainer GetCoreContainer { get; set; }
    }
}
