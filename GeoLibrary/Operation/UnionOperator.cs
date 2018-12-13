using System.Linq;
using GeoLibrary.Model;
using System.Collections.Generic;

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

        public static Geometry Union(MultiPoint multiPoint1, MultiPoint multiPoint2)
        {
            if (multiPoint1.IsValid == false)
                return multiPoint2.Clone();

            if (multiPoint2.IsValid == false)
                return multiPoint1.Clone();

            var points = new List<Point>();

            foreach (Point point in multiPoint1.Geometries)
            {
                if (multiPoint2.Geometries.All(x => x.Equals(point) == false))
                    points.Add(point.Clone() as Point);
            }

            foreach (Point point in multiPoint2.Geometries)
            {
                points.Add(point.Clone() as Point);
            }

            if (points.Count == 0)
            {
                return null;
            }
            else if (points.Count == 1)
            {
                return points[0];
            }
            else
            {
                return new MultiPoint(points);
            }

        }
    }
}
