using System.Collections.Generic;
using System.Linq;

namespace GeoLibrary.Model
{
    public class MultiPolygon : GeometryCollection
    {
        public MultiPolygon(IEnumerable<Polygon> polygons) : base(polygons)
        {
        }

        public MultiPolygon() : this(null)
        {
        }

        public override bool IsValid => base.IsValid && Geometries.All(x => x is Polygon);

        public override bool Equals(object obj)
        {
            if (!(obj is MultiPolygon other)) return false;

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

        public static bool operator ==(MultiPolygon left, MultiPolygon right)
        {
            return left?.Equals(right) ?? ReferenceEquals(right, null);
        }

        public static bool operator !=(MultiPolygon left, MultiPolygon right)
        {
            return !(left == right);
        }

        protected override GeometryCollection CreateNew()
        {
            return new MultiPolygon();
        }
    }
}
