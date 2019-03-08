using FluentAssertions;
using GeoLibrary.IO.Wkb;
using GeoLibrary.Model;
using System;
using System.Collections.Generic;
using Xunit;

namespace GeoLibrary.Unit.IO.Facts.Wkb
{
    public class WkbReaderFacts
    {
        [Fact]
        public void If_input_a_null_string_then_should_throw_exception()
        {
            string wkbHex = null;

            Assert.Throws<ArgumentException>(() => WkbReader.Read(wkbHex)).Message.Should().Be("Invalid hex wkb string");
        }

        [Fact]
        public void If_input_valid_point_with_big_endian_then_should_return_correct_point()
        {
            const string hex = "000000000140000000000000004010000000000000";
            var expectGeo = new Point(2, 4);

            var geoResult = WkbReader.Read(hex);
            geoResult.Equals(expectGeo).Should().BeTrue();
        }

        [Fact]
        public void If_input_valid_point_with_lower_endian_then_should_return_correct_point()
        {
            const string hex = "0101000020E61000005DA450163E1A5D40C44FD2B2A4F64340";
            var expectGeo = new Point(116.4100395, 39.9269012);

            var geoResult = WkbReader.Read(hex);
            geoResult.Equals(expectGeo).Should().BeTrue();
        }

        [Fact]
        public void If_input_valid_linestring_with_lower_endian_then_should_return_correct_linestring()
        {
            const string hex = "0102000020E61000000400000095D74AE82EED28408A271653442E49402E7F74A03DED284001A02F62442E49409F6BA9CD49ED28403657CD73442E49407D1A417452ED2840A2F9522E432E4940";
            var expectGeo = new LineString(new List<Point>
            {
                new Point(12.4632485, 50.3614601),
                new Point(12.4633608, 50.3614619),
                new Point(12.4634537, 50.361464),
                new Point(12.4635197, 50.3614252)
            });

            var geoResult = WkbReader.Read(hex);
            geoResult.Equals(expectGeo).Should().BeTrue();
        }

        [Fact]
        public void If_input_valid_polygon_with_lower_endian_then_should_return_correct_polygon()
        {
            const string hex = "010300000001000000050000000000000000003e4000000000000024400000000000004440000000000000444000000000000034400000000000004440000000000000244000000000000034400000000000003e400000000000002440";
            var expectGeo = new Polygon(new List<Point>
            {
                new Point(30, 10),
                new Point(40, 40),
                new Point(20, 40),
                new Point(10, 20),
                new Point(30, 10)
            });

            var geoResult = WkbReader.Read(hex);
            geoResult.Equals(expectGeo).Should().BeTrue();
        }
    }
}
