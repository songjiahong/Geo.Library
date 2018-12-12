using GeoLibrary.Model;

namespace GeoLibrary.Operation
{
    internal class IntersectionOperator
    {
        public static Geometry Intersection(Point point1, Point point2)
        {
            return point2.Equals(point1) ? point1.Clone() : null;
        }

        public static Geometry Intersection(Point point, MultiPoint multiPoint)
        {
            return IntersectCheckOperator.IsIntersects(point, multiPoint) ? point.Clone() : null;
        }
    }
}
