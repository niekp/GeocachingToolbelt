using System;
using GeocachingToolbelt.Models;
using Geodesy;

namespace GeocachingToolbelt.Utils
{
    public class Projection
    {
        private Coordinate _coordinate;
        public Projection(Coordinate coordinate)
        {
            _coordinate = coordinate;
        }

        public Coordinate Project(double distance, double bearing)
        {
            var p = new GeodeticCalculator(Ellipsoid.WGS84)
                .CalculateEndingGlobalCoordinates(
                    new GlobalCoordinates(
                        new Angle(_coordinate.Latitude),
                        new Angle(_coordinate.Longitude)
                    ),
                    new Angle(bearing),
                    distance
                );

            return new Coordinate(p.Latitude.Degrees, p.Longitude.Degrees);
        }
    }
}
