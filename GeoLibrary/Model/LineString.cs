using System.Collections.Generic;
using System.Linq;
using GeoLibrary.Interface;

namespace GeoLibrary.Model
{
    public class LineString : Geometry, ICurve
    {
        public IList<Point> Coordinates { get; private set; }

        public int Count => Coordinates?.Count ?? 0;

        public bool IsEmpty => Count == 0;

        public override bool IsValid => !IsEmpty && Coordinates.All(x => x.IsValid);

        public bool IsClosed => !IsEmpty && Coordinates[Count - 1] == Coordinates[0];

        public Point this[int index] => Coordinates[index];

        public LineString(IEnumerable<Point> points)
        {
            Coordinates = (points ?? new Point[0]).ToList();
        }

        public LineString() : this(null)
        {
        }

        public override bool Equals(object obj)
        {
            if (!(obj is LineString other)) return false;

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

        public static bool operator ==(LineString left, LineString right)
        {
            return left?.Equals(right) ?? ReferenceEquals(right, null);
        }

        public static bool operator !=(LineString left, LineString right)
        {
            return !(left == right);
        }
    }
}
