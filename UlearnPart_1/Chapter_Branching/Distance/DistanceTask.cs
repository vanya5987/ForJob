using System;

namespace DistanceTask
{
    public static class DistanceTask
    {
        public static double GetDistanceToSegment(double ax, double ay, double bx, double by, double x, double y)
        {
            if ((ax == x) && (ay == y))
                return 0;

            if ((bx == x) && (by == y))
                return 0;

            double distanceA = CountDistanceA(ax, ay, x, y);
            double distanceB = CountDistanceA(bx, by, x, y);
            double distanceC = CountDistanceA(ax, ay, bx, by);

            if ((ax == bx) && (ay == by))
                return distanceA;

            double distanceLine = CountPerpendicular(distanceA, AngleValue(distanceB, distanceA, distanceC));

            if ((AngleValue(distanceA, distanceB, distanceC) * 180 / Math.PI > 90)
                || (AngleValue(distanceB, distanceA, distanceC) * 180 / Math.PI > 90))
                return Math.Min(distanceA, distanceB);

            return distanceLine;
        }

        public static double AngleValue(double a, double b, double c)
        {
            var distance = (b * b + c * c - a * a) / (2 * b * c);
            distance = Math.Acos(distance);
            return distance;
        }

        public static double CountDistanceA(double ax, double ay, double x, double y)
        {
            return Math.Sqrt((ax - x) * (ax - x) + (ay - y) * (ay - y));
        }

        public static double CountPerpendicular(double distanceA, double cornerA)
        {
            return distanceA * Math.Sin(cornerA);
        }
    }
}