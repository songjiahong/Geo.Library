using FluentAssertions;
using GeoLibrary.Model;
using System;
using Xunit;

namespace GeoLibrary.Unit.Operation.Facts
{
    public class PointFacts
    {
        [Fact]
        public void Same_point_union_should_get_clone_of_itself()
        {
            var point = new Point(100, 50);

            var unionPoint = point.Union(new Point(100, 50));

            unionPoint.IsValid.Should().BeTrue();
            unionPoint.Equals(point).Should().BeTrue();
            ReferenceEquals(point, unionPoint).Should().BeFalse();
        }

        [Fact]
        public void Different_points_union_should_get_multipoint()
        {
            var point1 = new Point(100, 50);
            var point2 = new Point(120, 60);

            var unionGeo = point1.Union(point2);
            var unionGeo2 = point2.Union(point1);

            (unionGeo is MultiPoint).Should().BeTrue();
            (unionGeo2 is MultiPoint).Should().BeTrue();

            var multiPoint = unionGeo as MultiPoint;
            multiPoint.Count.Should().Be(2);
        }

        [Fact]
        public void Valid_point_union_invalid_one_should_get_clone_of_valid_one()
        {
            var point1 = new Point(100, 50);
            var point2 = new Point();

            var unionGeo = point1.Union(point2);
            var unionGeo2 = point2.Union(point1);

            (unionGeo is Point).Should().BeTrue();
            unionGeo2.Equals(unionGeo).Should().BeTrue();

            unionGeo.Equals(point1).Should().BeTrue();
        }

        [Fact]
        public void Point_union_multipoint_should_be_multipoint()
        {
            var point = new Point(100, 50);
            var multiPoint = new MultiPoint(new[] { new Point(100, 50), new Point(120, -70) });

            var unionGeo = point.Union(multiPoint);
            var unionGeo2 = multiPoint.Union(point);

            (unionGeo is MultiPoint).Should().BeTrue();
            unionGeo2.Equals(unionGeo).Should().BeTrue();

            unionGeo.Equals(multiPoint).Should().BeTrue();
        }

        [Fact]
        public void Point_union_linestring_should_not_support()
        {
            var point = new Point();
            var linestring = new LineString();

            Assert.Throws<Exception>(() => point.Union(linestring)).Message.Should().Be("Not supported type!");
        }

        [Fact]
        public void Invalid_points_should_not_be_intersects()
        {
            var point = new Point();
            var point2 = new Point();

            point.IsIntersects(point2).Should().BeFalse();

            var result = point.Intersection(point2);
            result.Should().Be(null);
        }

        [Fact]
        public void Invalid_point_should_not_intersect_valid_point()
        {
            var point1 = new Point(100, 50);
            var point2 = new Point();

            point1.IsIntersects(point2).Should().BeFalse();

            var result = point1.Intersection(point2);
            result.Should().Be(null);
        }

        [Fact]
        public void Different_points_should_not_intersect()
        {
            var point1 = new Point(100, 50);
            var point2 = new Point(120, 60);

            point1.IsIntersects(point2).Should().BeFalse();

            var result = point1.Intersection(point2);
            result.Should().Be(null);
        }

        [Fact]
        public void Same_points_should_intersect()
        {
            var point1 = new Point(100, 50);
            var point2 = new Point(100, 50);

            point1.IsIntersects(point2).Should().BeTrue();

            var result = point1.Intersection(point2);
            result.Should().NotBe(null);
            result.Equals(point1).Should().BeTrue();
        }

        [Fact]
        public void Valid_point_should_not_be_intersect_empty_multipoint()
        {
            var point1 = new Point(100, 50);
            var multiPoint = new MultiPoint();

            point1.IsIntersects(multiPoint).Should().BeFalse();

            var result = point1.Intersection(multiPoint);
            result.Should().Be(null);
        }

        [Fact]
        public void Invalid_point_should_not_be_intersect_valid_multipoint()
        {
            var point1 = new Point();
            var multiPoint = new MultiPoint(new[] { new Point(100, 50), new Point(120, -70) });

            point1.IsIntersects(multiPoint).Should().BeFalse();

            var result = point1.Intersection(multiPoint);
            result.Should().Be(null);
        }
    }
}
