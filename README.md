# Geo.Library - a 2D geospatial library for .NET

[![Build Status](https://revitapp.visualstudio.com/GeoLibrary/_apis/build/status/GeoLibrary-ASP.NET%20Core-CI?branchName=master)](https://revitapp.visualstudio.com/GeoLibrary/_build/latest?definitionId=1)

Geo 2D library to read/write to wkt/wkb/geojson and do intersection, union, difference etc calculation. Support point, mulitpoint, polygon, multipolygon.

## Nuget
```
Install-Package GeoLibrary -Version 1.1.0
```

## Support Geometry Types
* Point
* MultiPoint
* LineString
* Polygon
* MultiPolygon

## Features
* WKT Support
  * Read from WKT string
  * Write to WKT string
* GeoJson Support
  * Read from GeoJson string
  * Write to GeoJson string
* WKB Support
  * Read from WKB hex string
  * Write to WKB hex string
* Geometry Operations
  * Intersection Check
    * Point & Point
    * Point & MultiPoint
    * MultiPoint & MultiPoint
    * Point & Polygon (Whether a point is inside a polygon)
    * MultiPoint & Polygon (Whether any point is inside a polygon)
  * Intersects
    * Point & Point
    * Point & MultiPoint
    * MultiPoint & MultiPoint
  * Union
    * Point & Point
    * Point & MultiPoint
    * MultiPoint & MultiPoint
* Point Special Functions
  * IsBetweenLinear: check whether point lies between two points
  * DistanceTo: calculate Euclidean distance to another point
  * HaversineDistanceTo: calculate Haversine distance to another point
* Polygon Special Functions
  * IsPointInside: check whether a point is inside a polygon
  * CalculateCentroid: calculate centroid of the polygon
  * Area: get the area of the polygon
    
## Usage

```csharp
string wkt = "POINT (10 20)";
var point = Geometry.FromWkt(wkt);
var pointWkt = point.ToWkt();

string geoJson = "{\"type\": \"LineString\", \"coordinates\": [[30, 10], [10, 30], [40, 40]] }"
var lineString = Geometry.FromGeoJson(geoJson);
var lineStringGeoJson = lineString.ToGeoJson();
```