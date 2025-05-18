namespace DeepSpaceSaga.Common.Geometry;

public class SpaceMapLine
{
    public SpaceMapPoint From { get; set; }

    public SpaceMapPoint To { get; set; }

    public SpaceMapLine(SpaceMapPoint from, SpaceMapPoint to)
    {
        From = from;
        To = to;
    }
}
