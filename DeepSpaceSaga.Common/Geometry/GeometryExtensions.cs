using DeepSpaceSaga.Common.Geometry;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace DeepSpaceSaga.Common.Extensions;

public static class GeometryExtensions
{
    public static SpaceMapPoint ToSpaceMapCoordinates(this PointF pointF)
    {
        return new SpaceMapPoint(pointF.X, pointF.Y);
    }

    public static SpaceMapPoint ToSpaceMapCoordinates(this Point point)
    {
        return new SpaceMapPoint(point.X, point.Y);
    }

    public static float To360Degrees(this float angle)
    {
        // Приводим к диапазону [0, 360) используя остаток от деления
        angle = angle % 360f;
        
        // Если отрицательный, прибавляем 360
        if (angle < 0)
            angle += 360f;
        
        return angle;
    }

    public static int ToInt(this double value)
    {
        return (int)value;
    }
    
    public static double To360Degrees(this double angle)
    {
        // Приводим к диапазону [0, 360) используя остаток от деления
        angle = angle % 360.0;
        
        // Если отрицательный, прибавляем 360
        if (angle < 0)
            angle += 360.0;
        
        return angle;
    }

    public static Vector2 ToVector2(this PointF point)
    {
        return new Vector2(point.X, point.Y);
    }

    public static Vector2 ToVector2(this Point point)
    {
        return new Vector2(point.X, point.Y);
    }

    public static Point ToPoint(this Vector2 point)
    {
        return new Point((int)point.X, (int)point.Y);
    }

    public static PointF ToPointF(this Vector2 point)
    {
        return new PointF(point.X, point.Y);
    }

    public static PointF ToPointF(this Point point)
    {
        return new PointF(point.X, point.Y);
    }
}
