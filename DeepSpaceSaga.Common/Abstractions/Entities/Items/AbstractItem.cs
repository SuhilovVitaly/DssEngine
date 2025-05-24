using DeepSpaceSaga.Common.Abstractions.Entities.Equipment;
using DeepSpaceSaga.Common.Tools;

namespace DeepSpaceSaga.Common.Abstractions.Entities.Items;

[Serializable]
public class AbstractItem
{
    public int Id { get; set; } = new GenerationTool().GetId();
    public string Name { get; set; }
    public string Image { get; set; }
    public long OwnerId { get; set; }
    public Category Category { get; set; }
    public bool IsPacked { get; set; }
    public double Volume { get; set; } = 0;
}
