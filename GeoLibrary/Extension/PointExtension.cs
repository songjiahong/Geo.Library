using GeoLibrary.Model;

namespace GeoLibrary.Extension
{
    internal static class PointExtension
    {
        public static bool IsBetweenLinear(this Point point, Point p1, Point p2)
        {
            if (point == p1 || point == p2)
                return true;

            if (p1 == p2)
                return false;

            var slopeDelta = (point.Latitude - p1.Latitude) * (p2.Longitude - point.Longitude) - (p2.Latitude - point.Latitude) * (point.Longitude - p1.Longitude);
            if (slopeDelta.AlmostEqual(0) == false)
                return false;

            if (!p1.Longitude.AlmostEqual(p2.Longitude))
                return point.Longitude.GreaterThan(p1.Longitude) == point.Longitude.LessThan(p2.Longitude);

            return point.Latitude.GreaterThan(p1.Latitude) == point.Latitude.LessThan(p2.Latitude);
        }
    }
}
