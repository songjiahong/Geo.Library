using GeoLibrary.Extension;
using GeoLibrary.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace GeoLibrary.IO.GeoJson
{
    public class GeoJsonReader
    {
        private const string SpaceCharacters = " \t\n\r\b\f";
        private const string KeyType = "type";
        private const string KeyCoordinates = "coordinates";

        public static Geometry Read(string json)
        {
            var jsonObj = ParseObject(json);
            if (jsonObj == null)
            {
                throw new ArgumentException("Invalid json object!");
            }

            if (jsonObj.ContainsKey(KeyType) == false || jsonObj.ContainsKey(KeyCoordinates) == false)
            {
                throw new ArgumentException("Missing required property!");
            }

            switch (jsonObj[KeyType])
            {
                case GeoJsonTypes.Point:
                    return jsonObj[KeyCoordinates] as Point;
                case GeoJsonTypes.LineString:
                    return new LineString(jsonObj[KeyCoordinates] as List<Point>);
                case GeoJsonTypes.Polygon:
                    return GetPolygon(jsonObj[KeyCoordinates] as List<List<Point>>);
                case GeoJsonTypes.MultiPoint:
                    return new MultiPoint(jsonObj[KeyCoordinates] as List<Point>);
                case GeoJsonTypes.MultiPolygon:
                    var points = jsonObj[KeyCoordinates] as List<List<List<Point>>>;
                    return new MultiPolygon(points.Select(GetPolygon).ToList());
                default:
                    throw new ArgumentException($"Not supported type: {jsonObj[KeyType]}!");
            }
        }

        private static JsonObject ParseObject(string json)
        {
            var builder = new StringBuilder();
            using (var reader = new StringReader(json))
            {
                if (SkipWhitespace(reader))
                    return null;

                VerifyChar(reader, '{');

                JsonObject jsonObject = new JsonObject();
                if (SkipWhitespace(reader))
                    return null;

                while (ParseJsonObject(reader, builder, ref jsonObject))
                {
                    if (SkipWhitespace(reader))
                        return null;

                    if (reader.Peek() == ',')
                    {
                        reader.Read();
                        continue;
                    }

                    if (reader.Peek() == '}')
                    {
                        return jsonObject;
                    }

                    throw new ArgumentException("Invalid json");
                }

                return null;
            }
        }

        private static Polygon GetPolygon(List<List<Point>> points)
        {
            if (points.Count == 1)
            {
                return new Polygon(points[0]);
            }
            else
            {
                var rings = new List<LineString>();
                foreach (var arr in points)
                {
                    rings.Add(new LineString(arr));
                }

                return new Polygon(rings);
            }
        }

        private static bool ParseJsonObject(TextReader reader, StringBuilder builder, ref JsonObject jsonObject)
        {
            if (SkipWhitespace(reader))
                return false;

            VerifyChar(reader, '"');
            var name = ReadString(reader, builder);
            if (SkipWhitespace(reader))
                return false;

            VerifyChar(reader, ':');
            if (SkipWhitespace(reader))
                return false;

            if (reader.Peek() == '"')
            {
                reader.Read();
                var val = ReadString(reader, builder);

                jsonObject.Add(name, val);

                return true;
            }

            if (reader.Peek() == '[')
            {
                reader.Read();
                if (SkipWhitespace(reader))
                    return false;

                if (IsDigit((char)reader.Peek()))
                {
                    jsonObject.Add(name, ReadPoint(reader, builder));

                    return true;
                }

                if (reader.Peek() == '[')
                {
                    reader.Read();
                    if (SkipWhitespace(reader))
                        return false;

                    if (IsDigit((char)reader.Peek()))
                    {
                        jsonObject.Add(name, ReadPointArray(reader, builder));

                        return true;
                    }

                    if (reader.Peek() == '[')
                    {
                        reader.Read();
                        if (SkipWhitespace(reader))
                            return false;

                        if (IsDigit((char)reader.Peek()))
                        {
                            jsonObject.Add(name, ReadPointArrayArray(reader, builder));

                            return true;
                        }

                        if (reader.Peek() == '[')
                        {
                            reader.Read();
                            if (SkipWhitespace(reader))
                                return false;

                            if (IsDigit((char)reader.Peek()))
                            {
                                jsonObject.Add(name, ReadPointTripleArray(reader, builder));

                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private static string ReadString(TextReader reader, StringBuilder builder)
        {
            ClearStringBuilder(builder);

            while (reader.IsEOT() == false && reader.Peek() != '"')
            {
                builder.Append((char)reader.Read());
            }

            VerifyChar(reader, '"');

            return builder.ToString();
        }

        /// <summary>
        /// Skip white space and escape characters
        /// </summary>
        /// <param name="reader">source text reader</param>
        /// <returns>true if reach end of text, false otherwise</returns>
        private static bool SkipWhitespace(TextReader reader)
        {
            while (reader.IsEOT() == false && SpaceCharacters.IndexOf((char)reader.Peek()) != -1)
            {
                reader.Read();
            }

            return reader.IsEOT();
        }

        private static void VerifyChar(TextReader reader, char @char)
        {
            if (reader.Peek() != @char)
                throw new ArgumentException($"Invalid Json! Expect '{@char}' but '{reader.Peek()}'");

            reader.Read();
        }

        private static void ClearStringBuilder(StringBuilder builder)
        {
            if (builder.Length > 0)
            {
                builder.Remove(0, builder.Length);
            }
        }

        private static bool IsDigit(char ch)
        {
            return ch != -1 && ch >= '0' && ch <= '9' || ch == '.' || ch == '-' || ch == 'e' || ch == 'E';
        }

        private static double ReadDouble(TextReader reader, StringBuilder builder)
        {
            ClearStringBuilder(builder);
            // skip whitespaces in head
            SkipWhitespace(reader);

            while (IsDigit((char)reader.Peek()))
                builder.Append((char)reader.Read());

            return builder.Length == 0 ? throw new ArgumentException("Invalid number") : double.Parse(builder.ToString(), CultureInfo.InvariantCulture);
        }

        private static Point ReadPoint(TextReader reader, StringBuilder builder, bool verifyBeginning = false)
        {
            if (verifyBeginning)
            {
                VerifyChar(reader, '[');
            }

            double longitude = ReadDouble(reader, builder);
            SkipWhitespace(reader);
            VerifyChar(reader, ',');
            SkipWhitespace(reader);
            double latitude = ReadDouble(reader, builder);
            SkipWhitespace(reader);
            VerifyChar(reader, ']');

            return new Point(longitude, latitude);
        }

        private static List<T> ReadPoints<T>(TextReader reader, StringBuilder builder, Func<TextReader, StringBuilder, bool, T> readFunc, bool verifyBeginning = false)
        {
            if (verifyBeginning)
            {
                VerifyChar(reader, '[');
            }

            var points = new List<T>();
            var isFirst = true;
            while (true)
            {
                if (SkipWhitespace(reader))
                    return null;

                points.Add(readFunc(reader, builder, verifyBeginning || !isFirst));
                if (isFirst)
                    isFirst = false;

                if (SkipWhitespace(reader))
                    return null;

                if (reader.Peek() == ',')
                {
                    reader.Read();
                    continue;
                }

                if (reader.Peek() == ']')
                {
                    reader.Read();
                    break;
                }
            }

            return points;
        }

        private static List<Point> ReadPointArray(TextReader reader, StringBuilder builder, bool verifyBeginning = false)
        {
            return ReadPoints(reader, builder, ReadPoint, verifyBeginning);
        }

        private static List<List<Point>> ReadPointArrayArray(TextReader reader, StringBuilder builder, bool verifyBeginning = false)
        {
            return ReadPoints(reader, builder, ReadPointArray, verifyBeginning);
        }

        private static List<List<List<Point>>> ReadPointTripleArray(TextReader reader, StringBuilder builder, bool verifyBeginning = false)
        {
            return ReadPoints(reader, builder, ReadPointArrayArray, verifyBeginning);
        }
    }
}
