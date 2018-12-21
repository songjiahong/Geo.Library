using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoLibrary.Model;

namespace GeoLibrary.IO.GeoJson
{
    internal static class GeoJsonWriter
    {
        public static string Write(Geometry geometry)
        {
            if (geometry?.IsValid != true)
            {
                throw new ArgumentException("Invalid geometry");
            }

            var builder = new StringBuilder();
            Build(builder, geometry);

            return builder.ToString();
        }

        public static string ToGeoJson(this Geometry geometry)
        {
            return Write(geometry);
        }

        private static void Build(StringBuilder builder, Geometry geometry)
        {
            switch (geometry)
            {
                case Point point:
                    BuildPoint(builder, point);
                    return;
                case MultiPoint multiPoint:
                    BuildMultiPoint(builder, multiPoint);
                    return;
                case LineString lineString:
                    BuildLineString(builder, lineString);
                    return;
                case Polygon polygon:
                    BuildPolygon(builder, polygon);
                    return;
                case MultiPolygon multiPolygon:
                    BuildMultiPolygon(builder, multiPolygon);
                    return;
                default:
                    throw new ArgumentException($"Not supported geometry type: {geometry.GetType()}");
            }
        }

        private static void BuildPoint(StringBuilder builder, Point point)
        {
            AppendType(builder, GeoJsonTypes.Point);
            AppendOnePoint(builder, point);
            builder.Append("}");
        }

        private static void BuildMultiPoint(StringBuilder builder, MultiPoint multiPoint)
        {
            AppendType(builder, GeoJsonTypes.MultiPoint);
            AppendMultiPoints(builder, multiPoint.Geometries.Select(x => x as Point).ToList());
            builder.Append("}");
        }

        private static void BuildLineString(StringBuilder builder, LineString lineString)
        {
            AppendType(builder, GeoJsonTypes.LineString);
            AppendMultiPoints(builder, lineString.Coordinates);
            builder.Append("}");
        }

        private static void BuildPolygon(StringBuilder builder, Polygon polygon)
        {
            AppendType(builder, GeoJsonTypes.Polygon);
            BuildPolygonInternal(builder, polygon);
            builder.Append("}");
        }

        private static void BuildMultiPolygon(StringBuilder builder, MultiPolygon multiPolygon)
        {
            AppendType(builder, GeoJsonTypes.MultiPolygon);
            var polygons = multiPolygon.Geometries.Select(x => x as Polygon).ToList();
            for (var i = 0; i < polygons.Count; i++)
            {
                if (i > 0)
                    builder.Append(", ");

                BuildPolygonInternal(builder, polygons[i]);
            }

            builder.Append("]");
            builder.Append("}");
        }

        private static void BuildPolygonInternal(StringBuilder builder, Polygon polygon)
        {
            builder.Append("[");
            for (var idxRing = 0; idxRing < polygon.Count; idxRing++)
            {
                if (idxRing > 0)
                    builder.Append(", ");

                AppendMultiPoints(builder, polygon[idxRing].Coordinates);
            }
            builder.Append("]");
        }

        private static void AppendType(StringBuilder builder, string type)
        {
            builder.Append("{");
            builder.Append($"\"type\": \"{type}\"");
            builder.Append(",");
            AppendCoordinatesHead(builder);
        }

        private static void AppendCoordinatesHead(StringBuilder builder)
        {
            builder.Append("\"coordinates\": ");
        }

        private static void AppendOnePoint(StringBuilder builder, Point point)
        {
            builder.Append($"[{point.Longitude}, {point.Latitude}]");
        }

        private static void AppendMultiPoints(StringBuilder builder, IList<Point> points)
        {
            builder.Append("[");
            for (var i = 0; i < points.Count; i++)
            {
                if (i > 0)
                    builder.Append(", ");
                AppendOnePoint(builder, points[i]);
            }

            builder.Append("]");
        }
    }
}
