using FluentAssertions;
using GeoLibrary.Model;
using Xunit;

namespace GeoLibrary.Unit.Model.Facts
{
    public class PointFacts
    {
        [Fact]
        public void If_point_is_not_initial_then_it_should_be_invalid()
        {
            var point = new Point();

            point.IsValid.Should().BeFalse();
        }

        [Theory]
        [InlineData(-180.1, 90)]
        [InlineData(180.0001, -70)]
        [InlineData(-170, 100)]
        [InlineData(120, -91)]
        public void If_point_with_out_of_range_latlng_then_it_should_be_invalid(double longitude, double latitude)
        {
            var point = new Point(longitude, latitude);

            point.IsValid.Should().BeFalse();
        }

        [Theory]
        [InlineData(-170.1, 90)]
        [InlineData(180.000000001, -70)]
        [InlineData(120, -90.000000001)]
        public void If_point_with_valid_latlng_then_it_should_be_valid(double longitude, double latitude)
        {
            var point = new Point(longitude, latitude);

            point.IsValid.Should().BeTrue();
        }

        [Fact]
        public void A_point_should_not_equal_to_a_none_point()
        {
            var point = new Point();
            var other = 100;

            point.Equals(other).Should().BeFalse();
        }

        [Fact]
        public void A_valid_point_should_not_equal_an_invalid_point()
        {
            var point = new Point(100, 50);
            var other = new Point();

            point.Equals(other).Should().BeFalse();
        }

        [Fact]
        public void Two_valid_points_with_different_latlng_should_not_equal()
        {
            var point = new Point(100, 50);
            var other = new Point(100, 60);

            point.Equals(other).Should().BeFalse();
        }

        [Fact]
        public void Two_valid_points_with_same_latlng_should_equal()
        {
            var point = new Point(100, 50);
            var other = new Point(100, 50);

            point.Equals(other).Should().BeTrue();
            (point == other).Should().BeTrue();
        }

        [Fact]
        public void One_valid_point_should_equal_to_itself()
        {
            var point = new Point(100, 50);

            point.Equals(point).Should().BeTrue();
        }

        [Fact]
        public void Two_valid_points_with_very_close_latlng_should_equal()
        {
            var point = new Point(100.000000003, 50);
            var other = new Point(100, 50.000000003);

            point.Equals(other).Should().BeTrue();
            (point == other).Should().BeTrue();
        }

        [Fact]
        public void Two_null_points_should_equal()
        {
            Point point = null, other = null;

            (point == other).Should().BeTrue();
        }

        [Fact]
        public void One_null_point_should_not_equal_a_none_null_point()
        {
            Point point = new Point(), other = null;

            (point == other).Should().BeFalse();
        }

        [Fact]
        public void One_none_null_point_should_not_equal_a_null_point()
        {
            Point point = null, other = new Point();

            (point == other).Should().BeFalse();
        }
    }
}
