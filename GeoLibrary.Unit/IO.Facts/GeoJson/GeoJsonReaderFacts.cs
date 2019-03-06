using FluentAssertions;
using GeoLibrary.IO.GeoJson;
using GeoLibrary.Model;
using System;
using System.Collections.Generic;
using Xunit;

namespace GeoLibrary.Unit.IO.Facts.GeoJson
{
    public class GeoJsonReaderFacts
    {
        [Fact]
        public void If_json_is_invalid_then_should_throw_exception()
        {
            const string geojson = "{\"type\": \"Point\",\"coordinates: [10, 20]}";

            Assert.Throws<ArgumentException>(() => GeoJsonReader.Read(geojson));
        }

        [Fact]
        public void If_json_is_invalid_ending_then_should_throw_exception()
        {
            const string geojson = "{\"type\": \"Point\",\"coordinates\": [10, 20]";

            Assert.Throws<ArgumentException>(() => GeoJsonReader.Read(geojson))
                .Message.Should().BeEquivalentTo("Invalid json object!");
        }

        [Fact]
        public void If_json_is_valid_point_then_should_return_correct_point()
        {
            const string geojson = "{\"type\": \"Point\",\"coordinates\": [10, 20]}";
            var expectGeo = new Point(10, 20);

            var resultGeo = GeoJsonReader.Read(geojson);
            resultGeo.Equals(expectGeo).Should().BeTrue();
        }

        [Fact]
        public void If_json_is_valid_point_and_in_different_order_then_should_return_correct_point()
        {
            const string geojson = "{ \"coordinates\" : [10, 20], \"type\" : \"Point\" }";
            var expectGeo = new Point(10, 20);

            var resultGeo = GeoJsonReader.Read(geojson);
            resultGeo.Equals(expectGeo).Should().BeTrue();
        }

        [Fact]
        public void If_json_is_valid_line_string_then_should_return_correct_line_string()
        {
            const string geojson = @"{
                ""type"": ""LineString"", 
                ""coordinates"": [
                    [30, 10], [10, 30], [40, 40]
                ]
            }";
            var expectGeo = new LineString(new List<Point> {
                new Point(30, 10), new Point(10, 30), new Point(40, 40)
            });

            var resultGeo = GeoJsonReader.Read(geojson);
            resultGeo.Equals(expectGeo).Should().BeTrue();
        }

        [Fact]
        public void If_json_is_valid_single_ring_polygon_then_should_return_correct_polygon()
        {
            const string geojson = @"{
                ""type"": ""Polygon"", 
                ""coordinates"": [
                    [[30, 10], [40, 40], [20, 40], [10, 20], [30, 10]]
                ]
            }";
            var expectGeo = new Polygon(new List<Point> {
                new Point(30, 10), new Point(40, 40), new Point(20, 40), new Point(10, 20), new Point(30, 10)
            });

            var resultGeo = GeoJsonReader.Read(geojson);
            resultGeo.Equals(expectGeo).Should().BeTrue();
        }

        [Fact]
        public void If_json_is_valid_multi_ring_polygon_then_should_return_correct_polygon()
        {
            const string geojson = @"{
                ""type"": ""Polygon"", 
                ""coordinates"": [
                    [[35, 10], [45, 45], [15, 40], [10, 20], [35, 10]], 
                    [[20, 30], [35, 35], [30, 20], [20, 30]]
                ]
            }";
            var expectGeo = new Polygon(new List<LineString>
            {
                new LineString(new List<Point> {
                    new Point(35, 10), new Point(45, 45), new Point(15, 40), new Point(10, 20), new Point(35, 10)
                }),
                new LineString(new List<Point> {
                    new Point(20, 30), new Point(35, 35), new Point(30, 20), new Point(20, 30)
                })
            });

            var resultGeo = GeoJsonReader.Read(geojson);
            resultGeo.Equals(expectGeo).Should().BeTrue();
        }

        [Fact]
        public void If_json_is_valid_multipolygon_then_should_return_correct_multipolygon()
        {
            const string geojson = @"{
                ""type"": ""MultiPolygon"", 
                ""coordinates"": [
                    [
                        [[30, 20], [45, 40], [10, 40], [30, 20]]
                    ], 
                    [
                        [[15, 5], [40, 10], [10, 20], [5, 10], [15, 5]]
                    ]
                ]
            }";
            var expectGeo = new MultiPolygon(new List<Polygon>
            {
                new Polygon(new List<Point> {
                    new Point(30, 20), new Point(45, 40), new Point(10, 40), new Point(30, 20)
                }),
                new Polygon(new List<Point> {
                    new Point(15, 5), new Point(40, 10), new Point(10, 20), new Point(5, 10), new Point(15, 5)
                })
            });

            var resultGeo = GeoJsonReader.Read(geojson);
            resultGeo.Equals(expectGeo).Should().BeTrue();
        }

        [Fact]
        public void If_json_is_valid_multipolygon_with_hole_then_should_return_correct_multipolygon()
        {
            const string geojson = @"{
                ""type"": ""MultiPolygon"", 
                ""coordinates"": [
                    [
                        [[40, 40], [20, 45], [45, 30], [40, 40]]
                    ], 
                    [
                        [[20, 35], [10, 30], [10, 10], [30, 5], [45, 20], [20, 35]], 
                        [[30, 20], [20, 15], [20, 25], [30, 20]]
                    ]
                ]
            }";
            var expectGeo = new MultiPolygon(new List<Polygon>
            {
                new Polygon(new List<Point> {
                    new Point(40, 40), new Point(20, 45), new Point(45, 30), new Point(40, 40)
                }),
                new Polygon(new List<LineString>
                {
                    new LineString(new List<Point> {
                        new Point(20, 35), new Point(10, 30), new Point(10, 10), new Point(30, 5), new Point(45, 20), new Point(20, 35)
                    }),
                    new LineString(new List<Point> {
                        new Point(30, 20), new Point(20, 15), new Point(20, 25), new Point(30, 20)
                    })
                })
            });

            var resultGeo = GeoJsonReader.Read(geojson);
            resultGeo.Equals(expectGeo).Should().BeTrue();
        }
    }
}
