using System.Linq;
using GeoLibrary.Extension;
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

        public static bool IsIntersects(Point point, Polygon polygon)
        {
            if (point?.IsValid != true || polygon?.IsValid != true)
                return false;

            int wn = 0;    // the  winding number counter

            foreach (var lineString in polygon.LineStrings)
            {
                for (int i = 0; i < lineString.Count - 1; i++)
                {
                    if (IsOnSegment(point, lineString[i], lineString[i + 1]))
                        return true;

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
            }

            return wn != 0;
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
            var val = ((p1.Longitude - p0.Longitude) * (p2.Latitude - p0.Latitude)
                - (p2.Longitude - p0.Longitude) * (p1.Latitude - p0.Latitude));

            if (val.LessThan(0))
                return -1;

            if (val.GreaterThan(0))
                return 1;

            return 0;
        }

        /// <summary>
        /// Check whether a point p is on the segment between point p0 and p1
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <returns></returns>
        private static bool IsOnSegment(Point p, Point p0, Point p1)
        {
            var val = (p.Latitude - p0.Latitude) * (p1.Longitude - p0.Longitude) - (p.Longitude - p0.Longitude) * (p1.Latitude - p0.Latitude);
            if (val.AlmostEqual(0) == false)
                return false;

            val = (p.Longitude - p0.Longitude) * (p1.Longitude - p0.Longitude) + (p.Latitude - p0.Latitude) * (p1.Latitude - p0.Latitude);
            if (val.LessThan(0))
                return false;

            var sqLength = (p1.Longitude - p0.Longitude) * (p1.Longitude - p0.Longitude) + (p1.Latitude - p0.Latitude) * (p1.Latitude - p0.Latitude);

            return val.LessOrEqual(sqLength);
        }
    }
}
