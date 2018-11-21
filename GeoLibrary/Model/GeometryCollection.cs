using System.Collections.Generic;
using System.Linq;

namespace GeoLibrary.Model
{
    public abstract class GeometryCollection : Geometry
    {
        public int Count => Geometries?.Count ?? 0;

        public bool IsEmpty => Count == 0;

        public IList<Geometry> Geometries { get; private set; }

        public GeometryCollection(IEnumerable<Geometry> geometries)
        {
            Geometries = (geometries ?? new Geometry[0]).ToList();
        }

        public GeometryCollection() : this(null)
        {
        }

        public override bool IsValid => !IsEmpty && Geometries.All(x => x.IsValid);

        public Geometry this[int index] => Geometries[index];
    }
}
