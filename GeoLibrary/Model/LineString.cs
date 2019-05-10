using System.Collections.Generic;
using System.Linq;
using GeoLibrary.Extension;
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

        /// <summary>
        /// Simplify the polygon by removing duplicate points or collinear edges for all LineStrings unless the first and the last one
        /// </summary>
        /// <param name="removeCollinearEdges">Merge collinear edges if true, otherwise not</param>
        public void Simplify(bool removeCollinearEdges = false)
        {
            if (Count < 2)
                return;

            var lastPoint = Coordinates[Count - 1];
            for (var index = Count - 2; index > 0; index--)
            {
                if (Coordinates[index] == lastPoint)
                {
                    Coordinates.RemoveAt(index);
                }
                else
                {
                    lastPoint = Coordinates[index];
                }
            }

            for (int index = Count - 2; index > 0; index--)
            {
                if (this[index].IsBetweenLinear(this[index - 1], this[index + 1]))
                {
                    Coordinates.RemoveAt(index);
                }
            }
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

        public override Geometry Clone()
        {
            return new LineString(Coordinates.Select(x => x.Clone() as Point));
        }
    }
}
