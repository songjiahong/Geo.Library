using FluentAssertions;
using GeoLibrary.Model;
using System;
using Xunit;

namespace GeoLibrary.Unit.Operation.Facts
{
    public class MultiPointFacts
    {
        [Fact]
        public void Union_invalid_multipoints_should_get_invalid_multipoint()
        {
            var multiPoint = new MultiPoint();
            var multiPoint1 = new MultiPoint();

            var result = multiPoint.Union(multiPoint1);
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Union_multipoint_with_linestring_should_throw_exception()
        {
            var multiPoint = new MultiPoint();
            var linestring = new LineString();

            Assert.Throws<Exception>(() => multiPoint.Union(linestring)).Message.Should().Be("Not supported type!");
        }

        [Fact]
        public void Union_valid_multipoints_should_get_valid_multipoints()
        {
            var multiPoint1 = new MultiPoint(new[] { new Point(100, 50), new Point(120, 60) });
            var multiPoint2 = new MultiPoint(new[] { new Point(100, 50), new Point(120, -70) });

            var expectedMultiPoint = new MultiPoint(new[] { new Point(120, 60), new Point(100, 50), new Point(120, -70) });

            var result = multiPoint1.Union(multiPoint2);
            result.Equals(expectedMultiPoint).Should().BeTrue();
        }

        [Fact]
        public void Empty_multipoint_should_not_intersect_with_point()
        {
            var multiPoint = new MultiPoint();
            var point = new Point(100, 50);

            multiPoint.IsIntersects(point).Should().BeFalse();
            multiPoint.Intersection(point).Should().BeNull();
        }

        [Fact]
        public void Not_empty_multipoint_but_no_overlap_should_not_intersect_with_point()
        {
            var multiPoint = new MultiPoint(new[] { new Point(110, 50), new Point(120, 60) });
            var point = new Point(100, 50);

            multiPoint.IsIntersects(point).Should().BeFalse();
            multiPoint.Intersection(point).Should().BeNull();
        }

        [Fact]
        public void Not_empty_multipoint_with_overlap_should_get_point()
        {
            var multiPoint1 = new MultiPoint(new[] { new Point(110, 50), new Point(120, 60) });
            var multiPoint2 = new MultiPoint(new[] { new Point(110, 50), new Point(160, 60) });

            var expectedResult = new Point(110, 50);

            multiPoint1.IsIntersects(multiPoint2).Should().BeTrue();

            var result = multiPoint1.Intersection(multiPoint2);
            result.Equals(expectedResult).Should().BeTrue();
        }
    }
}
