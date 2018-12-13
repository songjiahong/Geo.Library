using GeoLibrary.Model;
using System.Collections.Generic;

namespace GeoLibrary.Operation
{
    internal class IntersectionOperator
    {
        public static Geometry Intersection(Point point1, Point point2)
        {
            return point2.IsIntersects(point1) ? point1.Clone() : null;
        }

        public static Geometry Intersection(Point point, MultiPoint multiPoint)
        {
            if (point.IsValid == false)
                return null;

            return IntersectCheckOperator.IsIntersects(point, multiPoint) ? point.Clone() : null;
        }

        public static Geometry Intersection(MultiPoint multiPoint1, MultiPoint multiPoint2)
        {
            if (!multiPoint1.IsValid || !multiPoint2.IsValid)
                return null;

            var points = new List<Point>();
            foreach (Point point in multiPoint1.Geometries)
            {
                foreach (Point pt in multiPoint2.Geometries)
                {
                    if (point.Equals(pt))
                    {
                        points.Add(pt.Clone() as Point);
                        break;
                    }
                }
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
