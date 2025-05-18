using System.Drawing;

namespace DeepSpaceSaga.Common.Geometry;

public class SpaceMapColor
{
    public SpaceMapColor(byte red, byte green, byte blue, byte alpha = 250)
    {
        Red = red;
        Green = green;
        Blue = blue;
        Alpha = alpha;  
    }
    public SpaceMapColor(Color color)
    {
        Red = color.R;
        Green = color.G;
        Blue = color.B;
        Alpha = color.A;
    }

    public byte Red { get; set; }
    public byte Green { get; set; }
    public byte Blue { get; set; }
    public byte Alpha { get; set; }

}
