using GeoLibrary.Extension;

namespace GeoLibrary.Model
{
    public class Point : Geometry
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public override bool IsValid
        {
            get
            {
                if (double.IsNaN(Longitude) || double.IsNaN(Latitude))
                {
                    return false;
                }

                return Longitude.GreaterOrEqual(Constants.MinLongitude)
                       && Longitude.LessOrEqual(Constants.MaxLongitude)
                       && Latitude.GreaterOrEqual(Constants.MinLatitude)
                       && Latitude.LessOrEqual(Constants.MaxLatitude);
            }
        }

        public Point(double longitude, double latitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }

        public Point()
        {
            Longitude = double.NaN;
            Latitude = double.NaN;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Point other)) return false;

            if (ReferenceEquals(this, other)) return true;

            if (IsValid != other.IsValid)
                return false;

            if (!IsValid)
                return true;

            return Longitude.AlmostEqual(other.Longitude) && Latitude.AlmostEqual(other.Latitude);

        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Point left, Point right)
        {
            return left?.Equals(right) ?? ReferenceEquals(right, null);
        }

        public static bool operator !=(Point left, Point right)
        {
            return !(left == right);
        }
    }
}
