using System;

namespace GeoLibrary.Model
{
    public abstract class Geometry
    {
        public abstract bool IsValid { get; }

        public virtual Geometry Union(Geometry other)
        {
            throw new Exception("Not supported type!");
        }

        public virtual Geometry IsIntersects(Geometry other)
        {
            throw new Exception("Not supported type!");
        }

        public virtual Geometry Intersection(Geometry other)
        {
            throw new Exception("Not supported type!");
        }

        public virtual Geometry Difference(Geometry other)
        {
            throw new Exception("Not supported type!");
        }

        public virtual Geometry SymDifference(Geometry other)
        {
            throw new Exception("Not supported type!");
        }
    }
}
