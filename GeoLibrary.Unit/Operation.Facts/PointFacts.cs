using FluentAssertions;
using GeoLibrary.Model;
using System;
using System.Collections.Generic;
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
        public void Invalid_point_union_invalid_multipoint_should_return_null()
        {
            var point = new Point();
            var multiPoint = new MultiPoint();

            var unionGeo = point.Union(multiPoint);
            unionGeo.Should().BeNull();
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

        [Theory]
        [InlineData(1, 1, true)]
        [InlineData(0, 0, true)]
        [InlineData(0, 3, true)]
        [InlineData(0, 5, true)]
        [InlineData(5, 5, true)]
        [InlineData(5, 0, true)]
        [InlineData(.5, 4.5, true)]
        [InlineData(4.5, 4.5, true)]
        [InlineData(-1, 0, false)]
        [InlineData(-1, -1, false)]
        [InlineData(-1, 4, false)]
        [InlineData(-1, 5, false)]
        [InlineData(2, 5, false)]
        [InlineData(2, 4.5, false)]
        [InlineData(2, 6, false)]
        [InlineData(6, 6, false)]
        public void If_point_with_a_simple_polygon_then_should_return_intersect_check_correctly(double longitude, double latitude, bool inside)
        {
            var point = new Point(longitude, latitude);
            var polygon = new Polygon(new List<Point>
            {
                new Point(0, 0),
                new Point(5, 0),
                new Point(5, 5),
                new Point(3, 5),
                new Point(3, 3),
                new Point(1, 3),
                new Point(1, 5),
                new Point(0, 5),
                new Point(0, 0),
            });

            point.IsIntersects(polygon).Should().Equals(inside);
        }

        [Theory]
        [InlineData(30, 17, true)]
        [InlineData(20, 23.9, false)]
        public void If_point_with_a_polygon_with_whole_then_should_return_intersect_check_correctly(double longitude, double latitude, bool inside)
        {
            var point = new Point(longitude, latitude);
            var polygon = new Polygon(new[]
            {
                new LineString(new[] { new Point(20, 35), new Point(10, 30), new Point(10, 10), new Point(30, 5), new Point(45, 20), new Point(20, 35) }),
                new LineString(new[] { new Point(30, 20), new Point(20, 15), new Point(20, 25), new Point(30, 20) })
            });

            point.IsIntersects(polygon).Should().Equals(inside);
        }
    }
}
