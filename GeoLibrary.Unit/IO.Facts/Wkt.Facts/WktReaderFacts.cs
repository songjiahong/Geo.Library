using System;
using FluentAssertions;
using GeoLibrary.IO.Wkt;
using GeoLibrary.Model;
using Xunit;

namespace GeoLibrary.Unit.IO.Facts.Wkt.Facts
{
    public class WktReaderFacts
    {
        [Theory]
        [InlineData("")]
        [InlineData("PT (10 20)")]
        [InlineData("MULTIP (10 10, 20 20)")]
        public void If_wkt_type_is_invalid_then_should_throw_exception(string wkt)
        {
            Assert.Throws<ArgumentException>(() => WktReader.Read(wkt)).Message.Should()
                .StartWith("Not supported WKT type: ");
        }

        [Fact]
        public void If_wkt_is_valid_point_then_should_return_correct_point()
        {
            const string wkt = "POINT (10 20)";
            var expectedPoint = new Point(10, 20);

            var resultPoint = WktReader.Read(wkt);

            resultPoint.Should().BeEquivalentTo(expectedPoint);
        }

        [Theory]
        [InlineData("MULTIPOINT ((10 40), (40 30), (20 20), (30 10))")]
        [InlineData("MULTIPOINT (10 40, 40 30, 20 20, 30 10)")]
        public void If_wkt_is_valid_multipoint_then_should_return_correct_multipoint(string wkt)
        {
            var expectedMultiPoint = new MultiPoint(new []{ new Point(10, 40), new Point(40, 30), new Point(20, 20), new Point(30, 10) });

            var resultMultiPoint = WktReader.Read(wkt);

            resultMultiPoint.Should().BeEquivalentTo(expectedMultiPoint);
        }
    }
}
