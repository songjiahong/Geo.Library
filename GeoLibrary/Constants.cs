using System;

namespace GeoLibrary
{
    internal static class Constants
    {
        public const double Eps = 1e-8;
        public const double MinLongitude = -180;
        public const double MaxLongitude = 180;
        public const double MinLatitude = -90;
        public const double MaxLatitude = 90;
        public const double EarthMeanRadius = 6371000;  // in meters
        public const UInt32 SridFlag = 0x20000000;
    }
}
