using System.Linq;
using GeoLibrary.Model;

namespace GeoLibrary.Operation
{
    internal class UnionOperator
    {
        public static Geometry Union(Point point1, Point point2)
        {
            return point2.Equals(point1) || point2.IsValid == false ? point1.Clone() : point1.IsValid ? new MultiPoint(new[] { point1, point2 }) : point2.Clone();
        }

        public static Geometry Union(Point point, MultiPoint multiPoint)
        {
            var nMultiPoint = multiPoint.Clone() as MultiPoint;
            if (nMultiPoint.Geometries.All(x => x.Equals(point) == false) && point.IsValid)
            {
                nMultiPoint.Geometries.Add(point.Clone());
            }

            return nMultiPoint;
        }
    }
}
