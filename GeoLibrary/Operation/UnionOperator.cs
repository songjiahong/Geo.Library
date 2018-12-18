using System.Linq;
using GeoLibrary.Model;
using System.Collections.Generic;

namespace GeoLibrary.Operation
{
    internal class UnionOperator
    {
        public static Geometry Union(Point point1, Point point2)
        {
            if (point1?.IsValid != true)
            {
                return point2?.IsValid == true ? point2.Clone() : null;
            }

            return point1.Equals(point2) || point2.IsValid == false ? point1.Clone() : new MultiPoint(new[] { point1, point2 });
        }

        public static Geometry Union(Point point, MultiPoint multiPoint)
        {
            if (IntersectCheckOperator.IsIntersects(point, multiPoint))
            {
                return multiPoint.Clone();
            }

            if (multiPoint?.IsValid != true)
                return point?.IsValid == true ? point.Clone() : null;

            var nMultiPoint = multiPoint.Clone() as MultiPoint;
            if (point?.IsValid == true)
            {
                nMultiPoint.Geometries.Add(point.Clone());
            }

            return nMultiPoint;
        }

        public static Geometry Union(MultiPoint multiPoint1, MultiPoint multiPoint2)
        {
            if (multiPoint1?.IsValid != true)
                return multiPoint2?.IsValid == true ? multiPoint2.Clone() : null;

            if (multiPoint2?.IsValid != true)
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
