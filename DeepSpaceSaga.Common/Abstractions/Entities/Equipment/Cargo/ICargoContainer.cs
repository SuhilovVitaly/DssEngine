using DeepSpaceSaga.Common.Abstractions.Entities.Equipment;
using DeepSpaceSaga.Common.Abstractions.Entities.Items;

namespace DeepSpaceSaga.Common.Abstractions.Entities.Equipment.Cargo;

public interface ICargoContainer : IModule
{
    double Power { get; set; }
    double Capacity { get; set; }
    double MaxCapacity { get; set; }
    Command Show();
    List<ICoreItem> Items { get; }
    void AddItem(ICoreItem item);
    void AddItems(List<ICoreItem> items);
    void RemoveItem(ICoreItem item);
    ICoreItem GetItemById(int itemId);
}
