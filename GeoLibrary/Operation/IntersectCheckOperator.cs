using System;
using System.Linq;
using GeoLibrary.Model;

namespace GeoLibrary.Operation
{
    internal class IntersectCheckOperator
    {
        public static bool IsIntersects(Point point1, Point point2)
        {
            if (point2?.IsValid != true)
                return false;

            return point2.Equals(point1);
        }

        public static bool IsIntersects(Point point, MultiPoint multiPoint)
        {
            if (point?.IsValid != true || multiPoint?.IsValid != true)
                return false;

            return multiPoint.Geometries.Any(x => x.Equals(point));
        }

        public static bool IsIntersects(MultiPoint multiPoint1, MultiPoint multiPoint2)
        {
            if (multiPoint1?.IsValid != true || multiPoint2?.IsValid != true)
                return false;

            foreach (Point point in multiPoint1.Geometries)
            {
                if (multiPoint2.Geometries.Any(x => x.Equals(point)))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Use the Winding number algorithm, refer to: http://geomalgorithms.com/a03-_inclusion.html
        /// </summary>
        /// <param name="point">The point to check</param>
        /// <param name="polygon">The polygon to check</param>
        /// <returns>true if intersect, false otherwise</returns>
        public static bool IsIntersects(Point point, Polygon polygon)
        {
            if (point?.IsValid != true || polygon?.IsValid != true)
                return false;

            var isIntersect = IsInside(point, polygon[0]);
            if (isIntersect == -1)
                return false;
            if (isIntersect == 0)
                return true;

            if (polygon.Count > 1)
            {
                // check the holes, if the point is inside one hole, it's outside the polygon
                for (int i = 1; i < polygon.Count; i++)
                {
                    isIntersect = IsInside(point, polygon[i]);
                    if (isIntersect == 0)
                        return true;
                    if (isIntersect == 1)
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Check whether a point is inside a line string.
        /// Assume the last point in the line string is same as the first point
        /// </summary>
        /// <param name="point">The point to check</param>
        /// <param name="lineString">The line ring to check</param>
        /// <returns>1 if inside, 0 if on edge, -1 if outside</returns>
        private static int IsInside(Point point, LineString lineString)
        {
            int wn = 0;    // the  winding number counter
            for (int i = 0; i < lineString.Count - 1; i++)
            {
                if (IsOnSegment(point, lineString[i], lineString[i + 1]))
                    return 0;

                if (lineString[i].Latitude.LessOrEqual(point.Latitude))
                {
                    if (lineString[i + 1].Latitude.GreaterThan(point.Latitude))     // an upward crossing
                        if (IsLeft(lineString[i], lineString[i + 1], point) > 0)    // P left of  edge
                            ++wn;
                }
                else
                {
                    if (lineString[i + 1].Latitude.LessOrEqual(point.Latitude))     // a downward crossing
                        if (IsLeft(lineString[i], lineString[i + 1], point) < 0)    // P right of  edge
                            --wn;
                }
            }

            return wn == 0 ? -1 : 1;
        }

        /// <summary>
        /// tests if a point is Left|On|Right of an infinite line
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns> >0 for p2 left of the line through p0 and p1
        //            =0 for p2  on the line
        //            <0 for p2  right of the line</returns>
        private static int IsLeft(Point p0, Point p1, Point p2)
        {
            const double DECTIMES = 1e7;
            var p0x = (long)(p0.Longitude * DECTIMES);
            var p0y = (long)(p0.Latitude * DECTIMES);
            var p1x = (long)(p1.Longitude * DECTIMES);
            var p1y = (long)(p1.Latitude * DECTIMES);
            var p2x = (long)(p2.Longitude * DECTIMES);
            var p2y = (long)(p2.Latitude * DECTIMES);

            var val = (p1x - p0x) * (p2y - p0y) - (p2x - p0x) * (p1y - p0y);
            if (val < 0)
                return -1;

            if (val > 0)
                return 1;

            return 0;
        }

        /// <summary>
        /// Check whether a point p is on the segment between point p0 and p1
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <returns>true if on the segment</returns>
        private static bool IsOnSegment(Point p, Point p0, Point p1)
        {
            if (p.Longitude.AlmostEqual(p0.Longitude))
            {
                if (p.Longitude.AlmostEqual(p1.Longitude))
                {
                    var minVal = Math.Min(p0.Latitude, p1.Latitude);
                    var maxVal = Math.Max(p0.Latitude, p1.Latitude);
                    if (p.Latitude.LessOrEqual(maxVal) && p.Latitude.GreaterOrEqual(minVal))
                        return true;
                }

                return false;
            }

            var val = (p.Latitude - p0.Latitude) * (p1.Longitude - p0.Longitude) - (p.Longitude - p0.Longitude) * (p1.Latitude - p0.Latitude);
            if (val.AlmostEqual(0) == false)
                return false;

            var minX = Math.Min(p0.Longitude, p1.Longitude);
            var maxX = Math.Max(p0.Longitude, p1.Longitude);

            return p.Longitude.LessOrEqual(maxX) && p.Longitude.GreaterOrEqual(minX);
        }
    }
}
