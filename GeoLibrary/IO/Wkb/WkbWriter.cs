using GeoLibrary.Model;
using System;
using System.IO;
using System.Text;

namespace GeoLibrary.IO.Wkb
{
    public static class WkbWriter
    {
        public static byte[] Write(Geometry geometry)
        {
            if (geometry?.IsValid != true)
            {
                throw new ArgumentException("Invalid geometry");
            }

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(memoryStream))
                {
                    WriteGeometry(geometry, writer);

                    return memoryStream.ToArray();
                }
            }
        }

        public static string WriteHex(Geometry geometry)
        {
            return ToHex(Write(geometry));
        }

        public static byte[] ToWkb(this Geometry geometry)
        {
            return Write(geometry);
        }

        public static string ToWkbHex(this Geometry geometry)
        {
            return WriteHex(geometry);
        }

        private static string ToHex(byte[] bytes)
        {
            var builder = new StringBuilder();
            foreach (var b in bytes)
            {
                builder.Append(b.ToString("X2"));
            }

            return builder.ToString();
        }

        private static GeometryType GetGeometryType(Geometry geometry)
        {
            if (geometry is Point)
                return GeometryType.Point;
            if (geometry is MultiPoint)
                return GeometryType.MultiPoint;
            if (geometry is LineString)
                return GeometryType.LineString;
            if (geometry is Polygon)
                return GeometryType.Polygon;
            if (geometry is MultiPolygon)
                return GeometryType.MultiPolygon;

            throw new ArgumentException($"Not supported geometry {geometry.GetType().Name}");
        }

        private static void WriteGeometry(Geometry geometry, BinaryWriter writer)
        {
            writer.Write(true);

            // assume xy dimension only and no srid now
            var geoType = GetGeometryType(geometry);
            writer.Write((UInt32)Dimension.Xy + (UInt32)geoType);

            switch (geometry)
            {
                case Point point:
                    WritePoint(point, writer);
                    break;
                case MultiPoint multiPoint:
                    WriteMultiPoint(multiPoint, writer);
                    break;
                case LineString lineString:
                    WriteLineString(lineString, writer);
                    break;
                case Polygon polygon:
                    WritePolygon(polygon, writer);
                    break;
                case MultiPolygon multiPolygon:
                    WriteMultiPolygon(multiPolygon, writer);
                    break;
                default:
                    break;
            }
        }

        private static void WritePoint(Point point, BinaryWriter writer)
        {
            writer.Write(point.Longitude);
            writer.Write(point.Latitude);
        }

        private static void WriteMultiPoint(MultiPoint multiPoint, BinaryWriter writer)
        {
            writer.Write((UInt32)multiPoint.Count);
            for (int i = 0; i < multiPoint.Count; i++)
            {
                WriteGeometry(multiPoint[i] as Point, writer);
            }
        }

        private static void WriteLineString(LineString lineString, BinaryWriter writer)
        {
            writer.Write((UInt32)lineString.Count);
            for (int i = 0; i < lineString.Count; i++)
            {
                WritePoint(lineString[i], writer);
            }
        }

        private static void WritePolygon(Polygon polygon, BinaryWriter writer)
        {
            writer.Write((UInt32)polygon.Count);
            for (int i = 0; i < polygon.Count; i++)
            {
                WriteLineString(polygon[i], writer);
            }
        }

        private static void WriteMultiPolygon(MultiPolygon multiPolygon, BinaryWriter writer)
        {
            writer.Write((UInt32)multiPolygon.Count);
            for (int i = 0; i < multiPolygon.Count; i++)
            {
                WriteGeometry(multiPolygon[i], writer);
            }
        }
    }
}
