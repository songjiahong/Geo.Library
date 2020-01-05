using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using GeoLibrary.Extension;
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
                    case WktTypes.MultiPolygon:
                        return ReadMultiPolygon(reader, builder);
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
            VerifyChar(reader, '(');

            return ReadPointInner(reader, builder);
        }

        private static MultiPoint ReadMultiPoint(TextReader reader, StringBuilder builder)
        {
            var points = new List<Point>();
            VerifyChar(reader, '(');

            var isOneArray = reader.Peek() != '(';
            while (reader.IsEOT() == false)
            {
                points.Add(isOneArray ? ReadPointInner(reader, builder) : ReadPoint(reader, builder));
                SkipWhiteSpaces(reader);
                if (isOneArray == false)
                    VerifyChar(reader, ')');

                if (reader.Peek() == ')')
                    break;

                VerifyChar(reader, ',');
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
            VerifyChar(reader, '(');

            while (reader.IsEOT() == false)
            {
                lineStrings.Add(new LineString(ReadPointsInner(reader, builder)));
                SkipWhiteSpaces(reader);
                if (reader.Peek() == ')')
                    break;

                VerifyChar(reader, ',');
                SkipWhiteSpaces(reader);
            }

            return new Polygon(lineStrings);
        }

        private static MultiPolygon ReadMultiPolygon(TextReader reader, StringBuilder builder)
        {
            var polygons = new List<Polygon>();
            VerifyChar(reader, '(');

            while (reader.IsEOT() == false)
            {
                polygons.Add(ReadPolygon(reader, builder));
                SkipWhiteSpaces(reader);
                VerifyChar(reader, ')');
                if (reader.Peek() == ')')
                    break;

                VerifyChar(reader, ',');
                SkipWhiteSpaces(reader);
            }

            return new MultiPolygon(polygons);
        }

        private static Point ReadPointInner(TextReader reader, StringBuilder builder)
        {
            return new Point(ReadDouble(reader, builder), ReadDouble(reader, builder));
        }

        private static IEnumerable<Point> ReadPointsInner(TextReader reader, StringBuilder builder)
        {
            var points = new List<Point>();
            VerifyChar(reader, '(');

            while (reader.IsEOT() == false)
            {
                points.Add(ReadPointInner(reader, builder));
                SkipWhiteSpaces(reader);

                if (reader.Peek() == ')')
                {
                    reader.Read();
                    break;
                }

                VerifyChar(reader, ',');
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

            return builder.Length == 0 ? throw new ArgumentException("Invalid number") : double.Parse(builder.ToString(), CultureInfo.InvariantCulture);
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
            return ch != -1 && ch >= '0' && ch <= '9' || ch == '.' || ch == '-' || ch == 'e' || ch == 'E';
        }

        private static bool IsLetter(char ch)
        {
            return ch != -1 && ch >= 'a' && ch <= 'z' || ch >= 'A' && ch <= 'Z';
        }

        private static void VerifyChar(TextReader reader, char @char)
        {
            if (reader.Peek() != @char)
                throw new ArgumentException($"Invalid WKT! Expect '{@char}' but '{reader.Peek()}'");

            reader.Read();
        }
    }
}
