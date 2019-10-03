using GeoLibrary.Operation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeoLibrary.Model
{
    public class Polygon : Geometry
    {
        /// <summary>
        /// The first LineString is the outer one, all others are inner ones
        /// </summary>
        public IList<LineString> LineStrings { get; private set; }

        public int Count => LineStrings?.Count ?? 0;

        public bool IsEmpty => Count == 0;

        public override bool IsValid => !IsEmpty && LineStrings.All(x => x.IsValid && x.IsClosed);

        public bool HasHole => Count > 1;

        public LineString this[int index] => LineStrings[index];

        public Polygon(IEnumerable<LineString> lineStrings)
        {
            LineStrings = (lineStrings ?? new LineString[0]).ToList();
        }

        public Polygon(IEnumerable<Point> points)
        {
            LineStrings = new List<LineString> { new LineString(points) };
        }

        public Polygon()
        {
            LineStrings = new List<LineString>();
        }

        /// <summary>
        /// Simplify the polygon by removing duplicate points or collinear edges for all LineStrings unless the first and the last one
        /// </summary>
        /// <param name="removeCollinearEdges">Merge collinear edges if true, otherwise not</param>
        public void Simplify(bool removeCollinearEdges = false)
        {
            if (IsEmpty) return;

            foreach (var lineString in LineStrings)
            {
                lineString.Simplify(removeCollinearEdges);
            }
        }

        /// <summary>
        /// Check whether the polygon is self intersection
        /// </summary>
        /// <returns>true is self intersection, false otherwise</returns>
        public bool IsSelfIntersection()
        {
            foreach (var ring in LineStrings)
            {
                if (ring.IsSelfIntersection())
                {
                    return true;
                }
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Polygon other)) return false;

            if (ReferenceEquals(this, other)) return true;

            if (IsValid != other.IsValid)
                return false;

            if (!IsValid)
                return true;

            if (Count != other.Count)
                return false;

            for (var i = 0; i < Count; i++)
            {
                if (this[i] != other[i])
                    return false;
            }

            return true;

        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Polygon left, Polygon right)
        {
            return left?.Equals(right) ?? ReferenceEquals(right, null);
        }

        public static bool operator !=(Polygon left, Polygon right)
        {
            return !(left == right);
        }

        public override Geometry Clone()
        {
            return new Polygon(LineStrings.Select(x => x.Clone() as LineString));
        }

        public bool IsPointInside(Point point)
        {
            return IsIntersects(point);
        }

        public override bool IsIntersects(Geometry other)
        {
            switch (other)
            {
                case Point point:
                    return IntersectCheckOperator.IsIntersects(point, this);
                case MultiPoint multiPoint:
                    foreach (Point pt in multiPoint.Geometries)
                    {
                        if (IntersectCheckOperator.IsIntersects(pt, this))
                        {
                            return true;
                        }
                    }
                    return false;
                case Polygon polygon:
                default:
                    throw new Exception("Not supported type!");
            }
        }

        /// <summary>
        /// Calculate centroid of polygon.
        /// Algorithm comes from https://en.wikipedia.org/wiki/Centroid
        /// </summary>
        /// <returns></returns>
        public Point CalculateCentroid()
        {
            if (IsValid == false)
                return null;

            var areaTotal = 0d;
            var latCalc = 0d;
            var lngCalc = 0d;

            foreach (var ring in LineStrings)
            {
                var n = ring.Count - 1;
                var area = 0d;
                var lng = 0d;
                var lat = 0d;
                for (int i = 0; i < n; i++)
                {
                    var p1 = ring[i];
                    var p2 = ring[(i + 1) % n];
                    area += p1.Longitude * p2.Latitude - p2.Longitude * p1.Latitude;
                    lng += (p1.Longitude + p2.Longitude) * (p1.Longitude * p2.Latitude - p2.Longitude * p1.Latitude);
                    lat += (p1.Latitude + p2.Latitude) * (p1.Longitude * p2.Latitude - p2.Longitude * p1.Latitude);
                }
                area /= 2;
                lng /= 6 * area;
                lat /= 6 * area;

                areaTotal += area;
                latCalc += lat * area;
                lngCalc += lng * area;
            }

            return new Point(lngCalc / areaTotal, latCalc / areaTotal);
        }

        public double Area
        {
            get
            {
                if (IsValid == false)
                    return 0;

                var areaTotal = 0d;
                foreach (var ring in LineStrings)
                {
                    var n = ring.Count - 1;
                    var area = 0d;
                    for (int i = 0; i < n; i++)
                    {
                        var p1 = ring[i];
                        var p2 = ring[(i + 1) % n];
                        area += p1.Longitude * p2.Latitude - p2.Longitude * p1.Latitude;
                    }
                    area /= 2;

                    areaTotal += area;
                }

                return areaTotal;
            }
        }
    }
}
