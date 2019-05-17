using System;
using GeoLibrary.IO.GeoJson;
using GeoLibrary.IO.Wkb;
using GeoLibrary.IO.Wkt;

namespace GeoLibrary.Model
{
    public abstract class Geometry
    {
        public abstract bool IsValid { get; }
        public abstract Geometry Clone();

        public static Geometry FromWkt(string wkt)
        {
            return WktReader.Read(wkt);
        }

        public static Geometry FromWkbHex(string wkbHex)
        {
            return WkbReader.Read(wkbHex);
        }

        public static Geometry FromGeoJson(string json)
        {
            return GeoJsonReader.Read(json);
        }

        public virtual Geometry Union(Geometry other)
        {
            throw new Exception("Not supported type!");
        }

        public virtual bool IsIntersects(Geometry other)
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
