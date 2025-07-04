namespace DeepSpaceSaga.Common.Abstractions.Mappers
{
    public static class ModuleMapper
    {
        public static ModuleDto ToDto(IModule module)
        {
            var dto = new ModuleDto
            {
                Id = module.Id,
                Name = module.Name,
                Image = module.Image,
                OwnerId = module.OwnerId,
                Category = (int)module.Category,
                IsPacked = module.IsPacked,
                Volume = module.Volume,
                BasePrice = module.BasePrice,
                TargetId = module.TargetId,
                IsAutoRun = module.IsAutoRun,
                IsCalculated = module.IsCalculated,
                ActivationCost = module.ActivationCost,
                IsActive = module.IsActive,
                LastReloadTurn = module.LastReloadTurn,
                IsReloaded = module.IsReloaded,
                ReloadTime = module.ReloadTime,
                Reloading  = module.Reloading,
            };

            return dto;
        }
    }
}
