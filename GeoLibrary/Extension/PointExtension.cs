using System;

namespace GeoLibrary.Model
{
    public static class PointExtension
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

        public static double DistanceTo(this Point point, Point otherPoint)
        {
            return Math.Sqrt(Math.Pow(otherPoint.Longitude - point.Longitude, 2) + Math.Pow(otherPoint.Latitude - point.Latitude, 2));
        }

        /// <summary>
        /// Haversine distance from one point to another one.
        /// </summary>
        /// <param name="point">Current point</param>
        /// <param name="otherPoint">Targer point</param>
        /// <returns>Return the distance in Kilometers</returns>
        public static double HaversineDistanceTo(this Point point, Point otherPoint)
        {
            var dLat = (otherPoint.Latitude - point.Latitude).ToRadian();
            var dLon = (otherPoint.Longitude - point.Longitude).ToRadian();
            var lat1 = point.Latitude.ToRadian();
            var lat2 = otherPoint.Latitude.ToRadian();

            var a = Math.Sin(dLat / 2.0) * Math.Sin(dLat / 2.0) +
                    Math.Sin(dLon / 2.0) * Math.Sin(dLon / 2.0) * Math.Cos(lat1) * Math.Cos(lat2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return Constants.EarthMeanRadius * c;
        }
    }
}
