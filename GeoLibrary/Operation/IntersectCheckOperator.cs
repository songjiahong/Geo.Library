using System.Linq;
using GeoLibrary.Model;

namespace GeoLibrary.Operation
{
    internal class IntersectCheckOperator
    {
        public static bool IsIntersects(Point point1, Point point2)
        {
            return point2.IsValid && point2.Equals(point1);
        }

        public static bool IsIntersects(Point point, MultiPoint multiPoint)
        {
            if (point.IsValid == false)
                return false;

            return multiPoint.Geometries.Any(x => x.Equals(point));
        }

        public static bool IsIntersects(MultiPoint multiPoint1, MultiPoint multiPoint2)
        {
            if (multiPoint1.IsValid && multiPoint2.IsValid)
            {
                foreach (Point point in multiPoint1.Geometries)
                {
                    if (multiPoint2.Geometries.Any(x => x.Equals(point)))
                    {
                        return true;
                    }
                }

                return false;
            }

            return multiPoint1.IsValid == multiPoint2.IsValid;
        }
    }
}
