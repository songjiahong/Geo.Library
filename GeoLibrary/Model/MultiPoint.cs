using System.Collections.Generic;
using System.Linq;

namespace GeoLibrary.Model
{
    public class MultiPoint : GeometryCollection
    {
        public MultiPoint(IEnumerable<Point> points) : base(points)
        {
        }

        public override bool IsValid => base.IsValid && Geometries.All(x => x is Point);

        public override bool Equals(object obj)
        {
            if (!(obj is MultiPoint other)) return false;

            if (ReferenceEquals(this, other)) return true;

            if (IsValid != other.IsValid)
                return false;

            if (!IsValid)
                return true;

            if (Count != other.Count)
                return false;

            for (var i = 0; i < Count; i++)
            {
                if (this[i].Equals(other[i]) == false)
                    return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(MultiPoint left, MultiPoint right)
        {
            return left?.Equals(right) ?? ReferenceEquals(right, null);
        }

        public static bool operator !=(MultiPoint left, MultiPoint right)
        {
            return !(left == right);
        }
    }
}
