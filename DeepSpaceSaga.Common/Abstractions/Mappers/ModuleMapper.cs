namespace DeepSpaceSaga.Common.Abstractions.Mappers;

public static class ModuleMapper
{
    public static IModule ToGameObject(ModuleDto moduleDto)
    {
        IModule module = null;

        switch ((Category)moduleDto.Category)
        {
            case Category.MiningLaser:
                module = MiningModulesGenerator.CreateMiningLaser(new GenerationTool(), moduleDto.OwnerId, moduleDto.Code);
                break;
        }

        module.TargetId = moduleDto.TargetId;
        module.ReloadTime = moduleDto.ReloadTime;
        module.Reloading = module.Reloading;
        module.IsActive = module.IsActive;
        module.LastReloadTurn = module.LastReloadTurn;
        module.IsReloaded = module.IsReloaded;
        module.IsCalculated = module.IsCalculated;

        return module;
    }

    public static ModuleDto ToDto(IModule module)
    {
        var dto = new ModuleDto
        {
            Code = module.Code,
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
