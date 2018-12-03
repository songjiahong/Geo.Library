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
        /// Remove duplicate points for all LineStrings unless the first and the last one
        /// </summary>
        public void Simplify()
        {
            if (IsEmpty) return;

            foreach (var lineString in LineStrings)
            {
                lineString.Simplify();
            }
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
    }
}
