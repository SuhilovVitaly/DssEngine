using DeepSpaceSaga.Common.Abstractions.Entities.Equipment;

namespace DeepSpaceSaga.Common.Abstractions.Entities.Items;

public interface ICoreItem
{
    int Id { get; set; }
    string Name { get; set; }
    string Image { get; set; }
    long OwnerId { get; set; }
    Category Category { get; set; }
    bool IsPacked { get; set; }
    double Volume { get; set; }
    double BasePrice {  get; set; }
}
