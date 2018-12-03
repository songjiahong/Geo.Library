using FluentAssertions;
using GeoLibrary.Model;
using Xunit;

namespace GeoLibrary.Unit.Model.Facts
{
    public class MultiPointFacts
    {
        [Fact]
        public void If_multipoint_is_not_initial_then_it_should_be_invalid()
        {
            var multiPoint = new MultiPoint();

            multiPoint.IsValid.Should().BeFalse();
        }

        [Theory]
        [InlineData(-180.1, 90)]
        [InlineData(120, -91)]
        public void If_multipoint_with_out_of_range_latlng_then_it_should_be_invalid(double longitude, double latitude)
        {
            var multiPoint = new MultiPoint(new[] { new Point(longitude, latitude) });

            multiPoint.IsValid.Should().BeFalse();
        }

        [Fact]
        public void If_multipoint_with_valid_point_and_linestring_then_it_should_be_invalid()
        {
            var multiPoint = new MultiPoint(new[] { new Point(-170.1, 90) });
            multiPoint.Geometries.Add(new LineString(new[] { new Point(120, -70) }));

            multiPoint.IsValid.Should().BeFalse();
        }

        [Fact]
        public void If_multipoint_with_valid_latlng_then_it_should_be_valid()
        {
            var multiPoint = new MultiPoint(new[] { new Point(-170.1, 90), new Point(120, -70) });

            multiPoint.IsValid.Should().BeTrue();
        }

        [Fact]
        public void A_multipoint_should_not_equal_to_a_none_point()
        {
            var multiPoint = new MultiPoint();
            var other = 100;

            multiPoint.Equals(other).Should().BeFalse();
        }

        [Fact]
        public void A_valid_multipoint_should_not_equal_an_invalid_point()
        {
            var multiPoint = new MultiPoint(new[] { new Point(-170.1, 90) });
            var other = new MultiPoint();

            multiPoint.Equals(other).Should().BeFalse();
        }

        [Fact]
        public void Two_valid_multipoints_with_different_latlng_should_not_equal()
        {
            var multiPoint = new MultiPoint(new[] { new Point(-170.1, 90) });
            var other = new MultiPoint(new[] { new Point(-170.1, 100) });

            multiPoint.Equals(other).Should().BeFalse();
            (multiPoint != other).Should().BeTrue();
        }

        [Fact]
        public void Two_valid_multipoints_with_same_latlng_should_equal()
        {
            var multiPoint = new MultiPoint(new[] { new Point(100, 90) });
            var other = new MultiPoint(new[] { new Point(100, 90) });

            multiPoint.Equals(other).Should().BeTrue();
            (multiPoint == other).Should().BeTrue();
        }

        [Fact]
        public void One_valid_multipoint_should_equal_to_itself()
        {
            var multiPoint = new MultiPoint(new[] { new Point(100, 90) });

            multiPoint.Equals(multiPoint).Should().BeTrue();
        }

        [Fact]
        public void Two_null_multipoints_should_equal()
        {
            MultiPoint multiPoint = null, other = null;

            (multiPoint == other).Should().BeTrue();
        }

        [Fact]
        public void One_null_multipoint_should_not_equal_a_none_null_multipoint()
        {
            MultiPoint multiPoint = new MultiPoint(), other = null;

            (multiPoint == other).Should().BeFalse();
        }

        [Fact]
        public void One_none_null_multipoint_should_not_equal_a_null_multipoint()
        {
            MultiPoint multiPoint = null, other = new MultiPoint();

            (multiPoint == other).Should().BeFalse();
        }

        [Fact]
        public void Clone_an_invalid_multipoint_should_be_invalid()
        {
            var multiPoint = new MultiPoint();

            multiPoint.Clone().IsValid.Should().BeFalse();
        }

        [Fact]
        public void Clone_a_valid_multipoint_should_equal_but_not_same()
        {
            var multiPoint = new MultiPoint(new[] { new Point(-170.1, 90), new Point(120, -70) });

            var copiedMultiPoint = multiPoint.Clone();

            ReferenceEquals(multiPoint, copiedMultiPoint).Should().BeFalse();
            copiedMultiPoint.Equals(multiPoint).Should().BeTrue();
        }
    }
}
