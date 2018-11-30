using FluentAssertions;
using GeoLibrary.Model;
using Xunit;

namespace GeoLibrary.Unit.Model.Facts
{
    public class MultiPolygonFacts
    {
        [Fact]
        public void If_multipolygon_is_not_initial_then_it_should_be_invalid()
        {
            var multiPolygon = new MultiPolygon();

            multiPolygon.IsValid.Should().BeFalse();
        }

        [Fact]
        public void If_multipolygon_with_valid_polygon_and_point_then_it_should_be_invalid()
        {
            var multiPolygon = new MultiPolygon(new[] { new Polygon(new[] { new LineString(new[] { new Point(30, 20), new Point(45, 40), new Point(10, 40), new Point(30, 20) }) }) });
            multiPolygon.Geometries.Add(new Point(120, -70));

            multiPolygon.IsValid.Should().BeFalse();
        }

        [Fact]
        public void If_multipolygon_with_valid_polygon_then_it_should_be_valid()
        {
            var multiPolygon = new MultiPolygon(new[] { new Polygon(new[] { new LineString(new[] { new Point(30, 20), new Point(45, 40), new Point(10, 40), new Point(30, 20) }) }) });

            multiPolygon.IsValid.Should().BeTrue();
        }

        [Fact]
        public void A_multipolygon_should_not_equal_to_a_none_point()
        {
            var multiPolygon = new MultiPolygon();
            var other = 100;

            multiPolygon.Equals(other).Should().BeFalse();
        }

        [Fact]
        public void A_valid_multipolygon_should_not_equal_an_invalid_point()
        {
            var multiPolygon = new MultiPolygon(new[] { new Polygon(new[] { new LineString(new[] { new Point(30, 20), new Point(45, 40), new Point(10, 40), new Point(30, 20) }) }) });
            var other = new MultiPolygon();

            multiPolygon.Equals(other).Should().BeFalse();
        }

        [Fact]
        public void Two_valid_multipolygons_with_different_latlng_should_not_equal()
        {
            var multiPolygon = new MultiPolygon(new[] { new Polygon(new[] { new LineString(new[] { new Point(30, 20), new Point(45, 40), new Point(10, 40), new Point(30, 20) }) }) });
            var other = new MultiPolygon(new[] { new Polygon(new[] { new LineString(new[] { new Point(30, 20), new Point(47, 40), new Point(10, 40), new Point(30, 20) }) }) });

            multiPolygon.Equals(other).Should().BeFalse();
            (multiPolygon != other).Should().BeTrue();
        }

        [Fact]
        public void Two_valid_multipolygons_with_same_polygon_should_equal()
        {
            var multiPolygon = new MultiPolygon(new[] { new Polygon(new[] { new LineString(new[] { new Point(30, 20), new Point(45, 40), new Point(10, 40), new Point(30, 20) }) }) });
            var other = new MultiPolygon(new[] { new Polygon(new[] { new LineString(new[] { new Point(30, 20), new Point(45, 40), new Point(10, 40), new Point(30, 20) }) }) });

            multiPolygon.Equals(other).Should().BeTrue();
            (multiPolygon == other).Should().BeTrue();
        }

        [Fact]
        public void One_valid_multipolygon_should_equal_to_itself()
        {
            var multiPolygon = new MultiPolygon(new[] { new Polygon(new[] { new LineString(new[] { new Point(30, 20), new Point(45, 40), new Point(10, 40), new Point(30, 20) }) }) });

            multiPolygon.Equals(multiPolygon).Should().BeTrue();
        }

        [Fact]
        public void Two_null_multipolygons_should_equal()
        {
            MultiPolygon multiPolygon = null, other = null;

            (multiPolygon == other).Should().BeTrue();
        }

        [Fact]
        public void One_null_multipolygon_should_not_equal_a_none_null_multipolygon()
        {
            MultiPolygon multiPolygon = new MultiPolygon(), other = null;

            (multiPolygon == other).Should().BeFalse();
        }

        [Fact]
        public void One_none_null_multipolygon_should_not_equal_a_null_multipolygon()
        {
            MultiPolygon multiPolygon = null, other = new MultiPolygon();

            (multiPolygon == other).Should().BeFalse();
        }
    }
}
