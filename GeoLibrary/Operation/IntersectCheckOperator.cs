using System.Linq;
using GeoLibrary.Model;

namespace GeoLibrary.Operation
{
    internal class IntersectCheckOperator
    {
        public static bool IsIntersects(Point point1, Point point2)
        {
            return point2.Equals(point1);
        }

        public static bool IsIntersects(Point point, MultiPoint multiPoint)
        {
            return multiPoint.Geometries.Any(x => x.Equals(point));
        }
    }
}
