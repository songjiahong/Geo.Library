# Geo.Library - a 2D geospatial library for .NET

[![Build Status](https://revitapp.visualstudio.com/GeoLibrary/_apis/build/status/GeoLibrary-ASP.NET%20Core-CI?branchName=master)](https://revitapp.visualstudio.com/GeoLibrary/_build/latest?definitionId=1)

Geo 2D library to read/write to wkt/geojson and do intersection, union, difference etc calculation. Support point, mulitpoint, polygon, multipolygon.

## Nuget
```
Install-Package GeoLibrary -Version 0.2.0
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
* Geometry Operations
  * Intersection Check
    * Point & Point
    * Point & MultiPoint
    * MultiPoint & MultiPoint
  * Intersects
    * Point & Point
    * Point & MultiPoint
    * MultiPoint & MultiPoint
  * Union
    * Point & Point
    * Point & MultiPoint
    * MultiPoint & MultiPoint

## Usage

```csharp
string wkt = "POINT (10 20)";
var point = WktReader.Read(wkt);

var pointWkt = WktWriter.Write(point);
```