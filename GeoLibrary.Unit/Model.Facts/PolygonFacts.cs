using FluentAssertions;
using GeoLibrary.Model;
using Xunit;

namespace GeoLibrary.Unit.Model.Facts
{
    public class PolygonFacts
    {
        [Fact]
        public void If_polygon_is_empty_then_it_should_be_invalid()
        {
            var polygon = new Polygon();

            polygon.Count.Should().Be(0);
            polygon.IsEmpty.Should().BeTrue();
            polygon.IsValid.Should().BeFalse();
            polygon.HasHole.Should().BeFalse();
        }

        [Fact]
        public void If_polygon_with_invalid_linestring_then_it_should_be_invalid()
        {
            var polygon = new Polygon(new[] { new LineString(new[] { new Point(-180.1, 90) }) });

            polygon.Count.Should().Be(1);
            polygon.IsValid.Should().BeFalse();
            polygon.HasHole.Should().BeFalse();
        }

        [Fact]
        public void If_polygon_with_not_close_linestring_then_it_should_be_invalid()
        {
            var polygon = new Polygon(new[] { new LineString(new[] { new Point(-120, 30), new Point(120, 30) }) });

            polygon.IsValid.Should().BeFalse();
        }

        [Fact]
        public void If_polygon_with_close_linestring_then_it_should_be_valid()
        {
            var polygon = new Polygon(new[] { new LineString(new[] { new Point(-120, 30), new Point(0, 0), new Point(120, 30), new Point(-120, 30) }) });

            polygon.IsValid.Should().BeTrue();
        }

        [Fact]
        public void A_polygon_should_not_equal_to_a_none_polygon()
        {
            var polygon = new Polygon();
            var other = new Point();

            polygon.Equals(other).Should().BeFalse();
        }

        [Fact]
        public void A_valid_polygon_should_not_equal_an_invalid_polygon()
        {
            var polygon = new Polygon(new[] { new LineString(new[] { new Point(-120, 30), new Point(0, 0), new Point(120, 30), new Point(-120, 30) }) });
            var other = new Polygon();

            polygon.Equals(other).Should().BeFalse();
            (polygon != other).Should().BeTrue();
        }

        [Fact]
        public void If_polygon_with_same_linestring_then_it_should_equal()
        {
            var polygon = new Polygon(new[] { new LineString(new[] { new Point(-120, 30), new Point(0, 0), new Point(120, 30), new Point(-120, 30) }) });
            var other = new Polygon(new[] { new LineString(new[] { new Point(-120, 30), new Point(0, 0), new Point(120, 30), new Point(-120, 30) }) });

            polygon.Equals(other).Should().BeTrue();
            (polygon != other).Should().BeFalse();
        }
    }
}
