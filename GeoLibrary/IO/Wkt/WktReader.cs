using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GeoLibrary.Model;

namespace GeoLibrary.IO.Wkt
{
    public static class WktReader
    {
        public static Geometry Read(string wkt)
        {
            var builder = new StringBuilder();

            using (var reader = new StringReader(wkt))
            {
                var type = ReadType(reader, builder);
                SkipWhiteSpaces(reader);
                switch (type)
                {
                    case WktTypes.Point:
                        return ReadPoint(reader, builder);
                    case WktTypes.MultiPoint:
                        return ReadMultiPoint(reader, builder);
                    case WktTypes.LineString:
                        return ReadLineString(reader, builder);
                    case WktTypes.Polygon:
                        return ReadPolygon(reader, builder);
                    default:
                        throw new ArgumentException($"Not supported WKT type: {type}");
                }
            }
        }

        private static string ReadType(TextReader reader, StringBuilder builder)
        {
            while (IsLetter((char)reader.Peek()))
            {
                builder.Append((char) reader.Read());
            }

            return builder.ToString().ToUpperInvariant();
        }

        private static Point ReadPoint(TextReader reader, StringBuilder builder)
        {
            if (reader.Read() != '(')
                throw new ArgumentException("Invalid Point WKT");

            return ReadPointInner(reader, builder);
        }

        private static MultiPoint ReadMultiPoint(TextReader reader, StringBuilder builder)
        {
            var points = new List<Point>();
            if (reader.Read() != '(')
                throw new ArgumentException("Invalid MultiPoint WKT");

            var isOneArray = reader.Peek() != '(';
            while (reader.Peek() != -1)
            {
                points.Add(isOneArray ? ReadPointInner(reader, builder) : ReadPoint(reader, builder));
                SkipWhiteSpaces(reader);
                if (isOneArray == false && reader.Read() != ')')
                    throw new ArgumentException("Invalid MultiPoint WKT");

                if (reader.Peek() == ')')
                    break;

                if (reader.Read() != ',')
                    throw new ArgumentException("Invalid MultiPoint WKT");

                SkipWhiteSpaces(reader);
            }

            return new MultiPoint(points);
        }

        private static LineString ReadLineString(TextReader reader, StringBuilder builder)
        {
            return new LineString(ReadPointsInner(reader, builder));
        }

        private static Polygon ReadPolygon(TextReader reader, StringBuilder builder)
        {
            var lineStrings = new List<LineString>();
            if (reader.Read() != '(')
                throw new ArgumentException("Invalid Polygon WKT");

            while (reader.Peek() != -1)
            {
                lineStrings.Add(new LineString(ReadPointsInner(reader, builder)));
                SkipWhiteSpaces(reader);
                if (reader.Peek() == ')')
                    break;
                
                if (reader.Read() != ',')
                    throw new ArgumentException("Invalid Polygon WKT");

                SkipWhiteSpaces(reader);
            }

            return new Polygon(lineStrings);
        }

        private static Point ReadPointInner(TextReader reader, StringBuilder builder)
        {
            return new Point(ReadDouble(reader, builder), ReadDouble(reader, builder));
        }

        private static IEnumerable<Point> ReadPointsInner(TextReader reader, StringBuilder builder)
        {
            var points = new List<Point>();
            if (reader.Read() != '(')
                throw new ArgumentException("Invalid WKT");

            while (reader.Peek() != -1)
            {
                points.Add(ReadPointInner(reader, builder));
                SkipWhiteSpaces(reader);

                if (reader.Peek() == ')')
                {
                    reader.Read();
                    break;
                }

                if (reader.Read() != ',')
                    throw new ArgumentException("Invalid WKT");

                SkipWhiteSpaces(reader);
            }

            return points;
        }

        private static void ClearStringBuilder(StringBuilder builder)
        {
            if (builder.Length > 0)
            {
                builder.Remove(0, builder.Length);
            }
        }

        private static double ReadDouble(TextReader reader, StringBuilder builder)
        {
            ClearStringBuilder(builder);
            // skip whitespaces in head
            SkipWhiteSpaces(reader);

            while (IsDigit((char)reader.Peek()))
                builder.Append((char)reader.Read());

            return builder.Length == 0 ? throw new ArgumentException("Invalid number") : double.Parse(builder.ToString());
        }

        private static void SkipWhiteSpaces(TextReader reader)
        {
            while (reader.Peek() == ' ')
            {
                reader.Read();
            }
        }

        private static bool IsDigit(char ch)
        {
            return ch != -1 && ch >= '0' && ch <= '9' || ch == '.' || ch == '-';
        }

        private static bool IsLetter(char ch)
        {
            return ch != -1 && ch >= 'a' && ch <= 'z' || ch >= 'A' && ch <= 'Z';
        }
    }
}
