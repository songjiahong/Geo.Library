using FluentAssertions;
using GeoLibrary.IO.Wkb;
using GeoLibrary.Model;
using System;
using System.Collections.Generic;
using Xunit;

namespace GeoLibrary.Unit.IO.Facts.Wkb
{
    public class WkbWriterFacts
    {
        [Fact]
        public void If_input_an_invalid_geometry_then_should_throw_exception()
        {
            var point = new Point();

            Assert.Throws<ArgumentException>(() => WkbWriter.Write(point)).Message.Should().Be("Invalid geometry");
        }

        [Fact]
        public void If_input_a_valid_point_then_should_get_correct_wkb()
        {
            var point = new Point(30, 10);
            var expectWkbHex = "01010000000000000000003E400000000000002440";

            var hex = point.ToWkbHex();
            hex.Should().BeEquivalentTo(expectWkbHex);
        }

        [Fact]
        public void If_input_a_valid_multipoint_then_should_get_correct_wkb()
        {
            var multiPoint = new MultiPoint(new List<Point>
            {
                new Point(10, 40),
                new Point(40, 30),
                new Point(20, 20),
                new Point(30, 10)
            });

            var expectWkbHex = "010400000004000000010100000000000000000024400000000000004440010100000000000000000044400000000000003E4001010000000000000000003440000000000000344001010000000000000000003E400000000000002440";

            var hex = multiPoint.ToWkbHex();
            hex.Should().BeEquivalentTo(expectWkbHex);
        }

        [Fact]
        public void If_input_a_valid_linestring_then_should_get_correct_wkb()
        {
            var lineString = new LineString(new List<Point>
            {
                new Point(12.4632485, 50.3614601),
                new Point(12.4633608, 50.3614619),
                new Point(12.4634537, 50.361464),
                new Point(12.4635197, 50.3614252)
            });

            var expectWkbHex = "01020000000400000095D74AE82EED28408A271653442E49402E7F74A03DED284001A02F62442E49409F6BA9CD49ED28403657CD73442E49407D1A417452ED2840A2F9522E432E4940";

            var hex = lineString.ToWkbHex();
            hex.Should().BeEquivalentTo(expectWkbHex);
        }

        [Fact]
        public void If_input_a_valid_polygon_then_should_get_correct_wkb()
        {
            var polygon = new Polygon(new List<Point>
            {
                new Point(30, 10),
                new Point(40, 40),
                new Point(20, 40),
                new Point(10, 20),
                new Point(30, 10)
            });

            var expectWkbHex = "010300000001000000050000000000000000003E4000000000000024400000000000004440000000000000444000000000000034400000000000004440000000000000244000000000000034400000000000003E400000000000002440";

            var hex = polygon.ToWkbHex();
            hex.Should().BeEquivalentTo(expectWkbHex);
        }

        [Fact]
        public void If_input_a_valid_multipolygon_then_should_get_correct_wkb()
        {
            var multiPolygon = new MultiPolygon(new List<Polygon>
            {
                new Polygon(new List<Point>
                {
                    new Point(30, 20),
                    new Point(45, 40),
                    new Point(10, 40),
                    new Point(30, 20)
                }),
                new Polygon(new List<Point>
                {
                    new Point(15, 5),
                    new Point(40, 10),
                    new Point(10, 20),
                    new Point(5, 10),
                    new Point(15, 5)
                })
            });
            var expectWkbHex = "010600000002000000010300000001000000040000000000000000003E40000000000000344000000000008046400000000000004440000000000000244000000000000044400000000000003E400000000000003440010300000001000000050000000000000000002E4000000000000014400000000000004440000000000000244000000000000024400000000000003440000000000000144000000000000024400000000000002E400000000000001440";

            var hex = multiPolygon.ToWkbHex();
            hex.Should().BeEquivalentTo(expectWkbHex);
        }
    }
}
