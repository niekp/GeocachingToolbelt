using System;
using GeocachingToolbelt.Models;
using Geodesy;

namespace GeocachingToolbelt.Utils
{
    public static class CoordinateExtensions
    {
        public static double Distance(this Coordinate coord1, Coordinate coord2)
        {
            return new GeodeticCalculator(Ellipsoid.WGS84)
                .CalculateGeodeticMeasurement(
                    new GlobalPosition(
                    new GlobalCoordinates(
                        new Angle(coord1.Latitude),
                        new Angle(coord1.Longitude)
                    )),
                    new GlobalPosition(
                        new GlobalCoordinates(
                        new Angle(coord2.Latitude),
                        new Angle(coord2.Longitude)
                    ))
                ).PointToPointDistance;
        }


        public static Coordinate Project(this Coordinate coordinate, double distance, double bearing)
        {
            var p = new GeodeticCalculator(Ellipsoid.WGS84)
                .CalculateEndingGlobalCoordinates(
                    new GlobalCoordinates(
                        new Angle(coordinate.Latitude),
                        new Angle(coordinate.Longitude)
                    ),
                    new Angle(bearing),
                    distance
                );

            return new Coordinate(p.Latitude.Degrees, p.Longitude.Degrees);
        }
    }
}
