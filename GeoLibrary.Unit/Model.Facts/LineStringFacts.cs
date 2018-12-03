using FluentAssertions;
using GeoLibrary.Model;
using Xunit;

namespace GeoLibrary.Unit.Model.Facts
{
    public class LineStringFacts
    {
        [Fact]
        public void If_linestring_is_empty_then_it_should_be_invalid()
        {
            var lineString = new LineString();

            lineString.IsEmpty.Should().BeTrue();
            lineString.IsValid.Should().BeFalse();
        }

        [Fact]
        public void If_linestring_with_invalid_point_then_it_should_be_invalid()
        {
            var lineString = new LineString(new [] { new Point(-180.1, 90) });

            lineString.IsValid.Should().BeFalse();
        }

        [Fact]
        public void If_linestring_with_valid_point_then_it_should_be_valid()
        {
            var lineString = new LineString(new [] { new Point(-120, 30) });

            lineString.IsValid.Should().BeTrue();
        }

        [Fact]
        public void If_linestring_with_one_point_then_it_should_be_close()
        {
            var lineString = new LineString(new[] { new Point(-120, 30) });

            lineString.IsClosed.Should().BeTrue();
        }

        [Fact]
        public void If_linestring_with_none_overlap_point_then_it_should_not_be_close()
        {
            var lineString = new LineString(new[] { new Point(-120, 30), new Point(20, 70) });

            lineString.IsClosed.Should().BeFalse();
        }

        [Fact]
        public void If_linestring_with_last_overlap_point_then_it_should_be_close()
        {
            var lineString = new LineString(new[] { new Point(-120, 30), new Point(20, 70), new Point(0, 0), new Point(-120, 30) });

            lineString.IsClosed.Should().BeTrue();
        }

        [Fact]
        public void If_linestring_with_some_overlap_point_then_simplify_should_remove_some_point()
        {
            var lineString = new LineString(new[] { new Point(-120, 30), new Point(20, 70), new Point(20, 70), new Point(0, 0), new Point(-120, 30), new Point(-120, 30) });
            var expectedLineString = new LineString(new[] { new Point(-120, 30), new Point(20, 70), new Point(0, 0), new Point(-120, 30) });

            (lineString == expectedLineString).Should().BeFalse();
            lineString.Simplify();
            (lineString == expectedLineString).Should().BeTrue();
        }

        [Fact]
        public void A_linestring_should_not_equal_to_a_none_linestring()
        {
            var lineString = new LineString();
            var other = new Point();

            lineString.Equals(other).Should().BeFalse();
        }

        [Fact]
        public void A_valid_linestring_should_not_equal_an_invalid_linestring()
        {
            var lineString = new LineString(new [] { new Point(-120, 30) });
            var other = new LineString();

            lineString.Equals(other).Should().BeFalse();
            (lineString != other).Should().BeTrue();
        }

        [Fact]
        public void Two_valid_linestrings_with_different_points_should_not_equal()
        {
            var lineString = new LineString(new [] { new Point(-120, 30) });
            var other = new LineString(new [] { new Point(120, 50) });

            lineString.Equals(other).Should().BeFalse();
            (lineString == other).Should().BeFalse();
        }

        [Fact]
        public void Two_valid_linestrings_with_different_length_should_not_equal()
        {
            var lineString = new LineString(new [] { new Point(120, 50) });
            var other = new LineString(new [] { new Point(120, 50), new Point(100, 30) });

            lineString.Equals(other).Should().BeFalse();
            (lineString == other).Should().BeFalse();
        }

        [Fact]
        public void Two_valid_linestrings_with_same_points_should_equal()
        {
            var lineString = new LineString(new [] { new Point(120, 50) });
            var other = new LineString(new [] { new Point(120, 50) });

            lineString.Equals(other).Should().BeTrue();
            (lineString == other).Should().BeTrue();
        }

        [Fact]
        public void One_valid_linestring_should_equal_to_itself()
        {
            var lineString = new LineString(new[] { new Point(120, 50) });

            lineString.Equals(lineString).Should().BeTrue();
        }

        [Fact]
        public void Two_null_linestrings_should_equal()
        {
            LineString lineString = null, other = null;

            (lineString == other).Should().BeTrue();
        }

        [Fact]
        public void One_null_linestring_should_not_equal_a_none_null_linestring()
        {
            LineString lineString = new LineString(), other = null;

            (lineString == other).Should().BeFalse();
        }

        [Fact]
        public void One_none_null_linestring_should_not_equal_a_null_linestring()
        {
            LineString lineString = null, other = new LineString();

            (lineString == other).Should().BeFalse();
        }

        [Fact]
        public void Clone_an_invalid_linestring_should_still_invalid()
        {
            var lineString = new LineString();

            lineString.Clone().IsValid.Should().BeFalse();
        }

        [Fact]
        public void Clone_a_valid_linestring_should_equal()
        {
            var lineString = new LineString(new[] { new Point(120, 50) });

            lineString.Clone().Equals(lineString).Should().BeTrue();
        }
    }
}
