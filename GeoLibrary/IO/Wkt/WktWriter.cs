using System;
using System.Globalization;
using System.Text;
using GeoLibrary.Model;

namespace GeoLibrary.IO.Wkt
{
    public static class WktWriter
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

        private static void Build(StringBuilder builder, Geometry geometry)
        {
            switch (geometry)
            {
                case Point point:
                    BuildPoint(builder, point);
                    return;
                case LineString lineString:
                    BuildLineString(builder, lineString);
                    return;
            }
        }

        private static void BuildPoint(StringBuilder builder, Point point)
        {
            builder.Append(WktTypes.Point);
            builder.Append(" (");
            BuildPointInner(builder, point);
            builder.Append(")");
        }

        private static void BuildPointInner(StringBuilder builder, Point point)
        {
            builder.Append(point.Longitude.ToString(CultureInfo.InvariantCulture));
            builder.Append(" ");
            builder.Append(point.Latitude.ToString(CultureInfo.InvariantCulture));
        }

        private static void BuildLineString(StringBuilder builder, LineString lineString)
        {
            builder.Append(WktTypes.LineString);
            builder.Append(" (");
            for (var i = 0; i < lineString.Count; i++)
            {
                if (i > 0) builder.Append(", ");

                BuildPointInner(builder, lineString[i]);
            }

            builder.Append(")");
        }
    }
}
