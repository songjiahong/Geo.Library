using System;
using System.Runtime.Serialization;
using FluentAssertions;
using GeoLibrary.IO.Wkt;
using GeoLibrary.Model;
using Xunit;

namespace GeoLibrary.Unit.IO.Facts.Wkt.Facts
{
    public class WktWriterFacts
    {
        [Fact]
        public void If_geometry_is_invalid_then_should_throw_exception()
        {
            var point = new Point();

            Assert.Throws<ArgumentException>(() => WktWriter.Write(point)).Message.Should()
                .BeEquivalentTo("Invalid geometry");
        }

        [Fact]
        public void If_point_is_valid_then_should_return_wkt()
        {
            var point = new Point(10, 20);
            const string expectWkt = "POINT (10 20)";

            WktWriter.Write(point).Should().BeEquivalentTo(expectWkt);
        }

        [Fact]
        public void If_linestring_is_valid_then_should_return_wkt()
        {
            var lineString = new LineString(new [] { new Point(10, 20), new Point(10, 30), new Point(0, 0) });
            const string expectWkt = "LINESTRING (10 20, 10 30, 0 0)";

            WktWriter.Write(lineString).Should().BeEquivalentTo(expectWkt);
        }

        [Fact]
        public void If_polygon_with_one_ring_is_valid_then_should_return_wkt()
        {
            var polygon = new Polygon(new[] { new LineString(new[] { new Point(-120, 30), new Point(0, 0), new Point(120, 30), new Point(-120, 30) }) });
            const string expectWkt = "POLYGON ((-120 30, 0 0, 120 30, -120 30))";

            WktWriter.Write(polygon).Should().BeEquivalentTo(expectWkt);
        }
    }
}
