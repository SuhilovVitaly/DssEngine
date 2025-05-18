namespace DeepSpaceSaga.Common.Geometry;

public class SpaceMapVector
{
    public SpaceMapPoint PointFrom { get; set; }
    public SpaceMapPoint PointTo { get; set; }
    public double Direction { get; set; }

    public SpaceMapVector(SpaceMapPoint pointFrom, SpaceMapPoint pointTo, double direction)
    {
        PointFrom = pointFrom;
        PointTo = pointTo;
        Direction = direction;
    }
}
