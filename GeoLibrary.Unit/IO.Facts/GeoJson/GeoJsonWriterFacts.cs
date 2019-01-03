using System;
using FluentAssertions;
using GeoLibrary.IO.GeoJson;
using GeoLibrary.Model;
using Xunit;

namespace GeoLibrary.Unit.IO.Facts.GeoJson
{
    public class GeoJsonWriterFacts
    {
        [Fact]
        public void If_geometry_is_invalid_then_should_throw_exception()
        {
            var point = new Point();

            Assert.Throws<ArgumentException>(() => GeoJsonWriter.Write(point)).Message.Should()
                .BeEquivalentTo("Invalid geometry");
        }

        [Fact]
        public void If_point_is_valid_then_should_return_geojson()
        {
            var point = new Point(10, 20);
            const string expectGeojson = "{\"type\": \"Point\",\"coordinates\": [10, 20]}";

            var resultWkt = GeoJsonWriter.Write(point);
            resultWkt.Should().BeEquivalentTo(expectGeojson);
            point.ToGeoJson().Should().BeEquivalentTo(expectGeojson);
        }

        [Fact]
        public void If_multipoint_is_valid_then_should_return_geojson()
        {
            var multiPoint = new MultiPoint(new[] { new Point(10, 20), new Point(20, 30), new Point(30, 60) });
            const string expectGeojson = "{\"type\": \"MultiPoint\",\"coordinates\": [[10, 20], [20, 30], [30, 60]]}";

            GeoJsonWriter.Write(multiPoint).Should().BeEquivalentTo(expectGeojson);
        }

        [Fact]
        public void If_linestring_is_valid_then_should_return_geojson()
        {
            var lineString = new LineString(new[] { new Point(10, 20), new Point(10, 30), new Point(0, 0) });
            const string expectGeojson = "{\"type\": \"LineString\",\"coordinates\": [[10, 20], [10, 30], [0, 0]]}";

            GeoJsonWriter.Write(lineString).Should().BeEquivalentTo(expectGeojson);
        }

        [Fact]
        public void If_polygon_with_one_ring_is_valid_then_should_return_geojson()
        {
            var polygon = new Polygon(new[] { new LineString(new[] { new Point(-120, 30), new Point(0, 0), new Point(120, 30), new Point(-120, 30) }) });
            const string expectGeojson = "{\"type\": \"Polygon\",\"coordinates\": [[[-120, 30], [0, 0], [120, 30], [-120, 30]]]}";

            GeoJsonWriter.Write(polygon).Should().BeEquivalentTo(expectGeojson);
        }

        [Fact]
        public void If_multipolygon_is_valid_then_should_return_geojson()
        {
            var multiPolygon = new MultiPolygon(new[]
            {
                new Polygon(new[] { new LineString(new[] { new Point(30, 20), new Point(45, 40), new Point(10, 40), new Point(30, 20) }) }),
                new Polygon(new[] { new LineString(new[] { new Point(15, 5), new Point(40, 10), new Point(10, 20), new Point(5, 10), new Point(15, 5) }) })
            });

            const string expectGeojson = "{\"type\": \"MultiPolygon\",\"coordinates\": [[[[30, 20], [45, 40], [10, 40], [30, 20]]], [[[15, 5], [40, 10], [10, 20], [5, 10], [15, 5]]]]}";

            GeoJsonWriter.Write(multiPolygon).Should().BeEquivalentTo(expectGeojson);
        }
    }
}
