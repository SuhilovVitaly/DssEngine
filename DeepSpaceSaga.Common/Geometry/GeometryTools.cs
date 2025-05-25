using DeepSpaceSaga.Common.Extensions;
using DeepSpaceSaga.Common.Geometry;

namespace DeepSpaceSaga.Common.Tools;

public class GeometryTools
{
    private static double ToDegrees(double angle) => (angle * 180 / Math.PI);

    public static double Azimuth(SpaceMapPoint destination, SpaceMapPoint center)
    {
        var deltaX = destination.X - center.X;
        var deltaY = destination.Y - center.Y;

        var angle = Math.Atan2(deltaY, deltaX) * (180 / Math.PI);

        if (angle < 0)
        {
            angle += 360;
        }

        return angle;
    }

    public static SpaceMapPoint Move(SpaceMapPoint currentLocation, double speed, double angleInDegrees)
    {
        var angleInRadians = angleInDegrees * (Math.PI) / 180;

        var x = (float)(currentLocation.X + speed * Math.Cos(angleInRadians));
        var y = (float)(currentLocation.Y + speed * Math.Sin(angleInRadians));

        return new SpaceMapPoint(x, y);
    }

    public static double Distance(SpaceMapPoint p1, SpaceMapPoint p2)
    {
        double xDelta = p1.X - p2.X;
        double yDelta = p1.Y - p2.Y;

        return Math.Sqrt(Math.Pow(xDelta, 2) + Math.Pow(yDelta, 2));
    }

    public static double DirectionsDelta(float p1, float p2)
    {
        return Math.Abs(p1 - p2).To360Degrees();
    }

    public static SpaceMapPoint GetCentreLine(SpaceMapPoint from, SpaceMapPoint to)
    {
        return new SpaceMapPoint((to.X + from.X) / 2, (to.Y + from.Y) / 2);
    }

    public static (SpaceMapPoint, SpaceMapPoint) CalculateTangents(SpaceMapPoint externalPoint, SpaceMapPoint circleCenter, float radius)
    {
        // Вычисляем расстояние от внешней точки до центра окружности
        double distanceToCenter = Distance(externalPoint, circleCenter);

        // Проверяем, что внешняя точка находится за пределами окружности
        if (distanceToCenter <= radius)
        {
            throw new ArgumentException("External point must be outside the circle.");
        }

        // Угол между центром окружности и внешней точкой
        double angleToPoint = Math.Atan2(externalPoint.Y - circleCenter.Y, externalPoint.X - circleCenter.X);

        // Угол между радиусом и касательной (правый угол в треугольнике)
        double tangentAngle = Math.Acos(radius / distanceToCenter);

        // Вычисляем углы для двух касательных
        double angle1 = angleToPoint + tangentAngle;
        double angle2 = angleToPoint - tangentAngle;

        // Вычисляем координаты точек касания
        var tangentPoint1 = new SpaceMapPoint(
            (float)(circleCenter.X + radius * Math.Cos(angle1)),
            (float)(circleCenter.Y + radius * Math.Sin(angle1))
        );

        var tangentPoint2 = new SpaceMapPoint(
            (float)(circleCenter.X + radius * Math.Cos(angle2)),
            (float)(circleCenter.Y + radius * Math.Sin(angle2))
        );

        return (tangentPoint1, tangentPoint2);
    }
}
