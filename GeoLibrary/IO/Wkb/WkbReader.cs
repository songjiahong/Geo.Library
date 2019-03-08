using GeoLibrary.Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace GeoLibrary.IO.Wkb
{
    public class WkbReader
    {
        const UInt32 SridFlag = 0x20000000;

        public static Geometry Read(string wkbHex)
        {
            if (string.IsNullOrEmpty(wkbHex) || wkbHex.Length % 2 != 0)
            {
                throw new ArgumentException("Invalid hex wkb string");
            }

            using (var stream = GetBinaryStreamFromHex(wkbHex))
            {
                using (var reader = new EndianBinaryReader(stream))
                {
                    reader.IsBigEndian = !reader.ReadBoolean();
                    uint type = reader.ReadUInt32();
                    var hasSrid = (type & SridFlag) == SridFlag;
                    type &= 0xFF;
                    var geomType = (GeometryType)(type % 1000);
                    var dimension = (Dimension)(type - type % 1000);
                    UInt32 srid = 0;
                    if (hasSrid)
                    {
                        srid = reader.ReadUInt32();
                    }

                    switch (geomType)
                    {
                        case GeometryType.Point:
                            return ReadPoint(reader);
                        case GeometryType.LineString:
                            return ReadLineString(reader);
                        case GeometryType.Polygon:
                            return ReadPolygon(reader);
                        case GeometryType.MultiPoint:
                        case GeometryType.MultiLineString:
                        case GeometryType.MultiPolygon:
                        case GeometryType.Geometry:
                        case GeometryType.GeometryCollection:
                        default:
                            throw new NotSupportedException($"Not supported geometry type: {geomType}");
                    }
                }
            }
        }

        private static Stream GetBinaryStreamFromHex(string hex)
        {
            using (var reader = new StringReader(hex))
            {
                var buffer = new byte[hex.Length / 2];
                var i = 0;
                while (reader.Peek() != -1)
                {
                    buffer[i++] = Convert.ToByte($"{(char)reader.Read()}{(char)reader.Read()}", 16);
                }

                return new MemoryStream(buffer);
            }
        }

        private static Point ReadPoint(EndianBinaryReader reader)
        {
            return new Point(reader.ReadDouble(), reader.ReadDouble());
        }

        private static LineString ReadLineString(EndianBinaryReader reader)
        {
            var pointCount = reader.ReadUInt32();
            var points = new List<Point>();
            for (int i = 0; i < pointCount; i++)
            {
                points.Add(ReadPoint(reader));
            }

            return new LineString(points);
        }

        private static Polygon ReadPolygon(EndianBinaryReader reader)
        {
            var ringCount = reader.ReadUInt32();
            if (ringCount == 0)
                return null;

            var rings = new List<LineString>();
            for (int i = 0; i < ringCount; i++)
            {
                rings.Add(ReadLineString(reader));
            }

            return new Polygon(rings);
        }
    }
}
