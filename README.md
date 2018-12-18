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
* Read from WKT string
* Write to WKT string
* Check intersection between points and mulitpoints
* Calculate intersection between points and mulitpoints
* Calculate union of points and mulitpoints

## Usage

```csharp
string wkt = "POINT (10 20)";
var point = WktReader.Read(wkt);

var pointWkt = WktWriter.Write(point);
```