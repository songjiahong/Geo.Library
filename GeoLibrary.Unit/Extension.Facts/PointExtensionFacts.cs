using FluentAssertions;
using GeoLibrary.Model;
using Xunit;

namespace GeoLibrary.Unit.Extension.Facts
{
    public class PointExtensionFacts
    {
        [Fact]
        public void If_point_is_between_two_horizontal_points_then_should_return_correct_result()
        {
            var point = new Point(1, 0);
            var p1 = new Point(0, 0);
            var p2 = new Point(5, 0);

            point.IsBetweenLinear(p1, p2).Should().BeTrue();
        }

        [Fact]
        public void If_point_is_not_between_two_horizontal_points_then_should_return_correct_result()
        {
            var point = new Point(-1, 0);
            var p1 = new Point(0, 0);
            var p2 = new Point(5, 0);

            point.IsBetweenLinear(p1, p2).Should().BeFalse();
        }

        [Fact]
        public void If_point_is_between_two_vertical_points_then_should_return_correct_result()
        {
            var point = new Point(0, 1);
            var p1 = new Point(0, 0);
            var p2 = new Point(0, 3);

            point.IsBetweenLinear(p1, p2).Should().BeTrue();
        }

        [Fact]
        public void If_point_is_not_between_two_vertical_points_then_should_return_correct_result()
        {
            var point = new Point(0, -3);
            var p1 = new Point(0, 0);
            var p2 = new Point(0, 5);

            point.IsBetweenLinear(p1, p2).Should().BeFalse();
        }

        [Fact]
        public void If_point_is_same_as_one_of_the_two_points_then_should_return_correct_result()
        {
            var p1 = new Point(0, 0);
            var p2 = new Point(0, 3);

            p1.IsBetweenLinear(p1, p2).Should().BeTrue();
            p2.IsBetweenLinear(p1, p2).Should().BeTrue();
        }

        [Fact]
        public void If_point_is_between_two_slope_points_then_should_return_correct_result()
        {
            var point = new Point(1, 1);
            var p1 = new Point(0, 0);
            var p2 = new Point(3, 3);

            point.IsBetweenLinear(p1, p2).Should().BeTrue();
        }

        [Fact]
        public void If_point_is_not_between_two_slope_points_then_should_return_correct_result()
        {
            var point = new Point(-1, -1);
            var p1 = new Point(0, 0);
            var p2 = new Point(5, 5);

            point.IsBetweenLinear(p1, p2).Should().BeFalse();
        }

        [Fact]
        public void If_points_are_valid_then_haversine_distance_should_be_correct()
        {
            var p1 = new Point(-86.67, 36.12);
            var p2 = new Point(-118.40, 33.94);
            var expectDistance = 2886.44444283798329974715782394574671655;

            p1.HaversineDistanceTo(p2).Equals(expectDistance).Should().BeTrue();
            p2.HaversineDistanceTo(p1).Equals(expectDistance).Should().BeTrue();
        }
    }
}
