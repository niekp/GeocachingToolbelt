using System;
using GeocachingToolbelt.Models;

namespace GeocachingToolbelt.Utils
{
    public class Projection
    {
        private Coordinate _coordinate;
        public Projection(Coordinate coordinate)
        {
            _coordinate = coordinate;
        }

        public Coordinate Project(double distance, double bearing) {
            distance /= 1000;

            double lat1 = _coordinate.Latitude.ToRadians();
            double lon1 = _coordinate.Longitude.ToRadians();
            double brng = bearing.ToRadians();
            
            double dist = distance / 6371.01; //Earth's radius in km

            double lat2 = Math.Asin(
                Math.Sin(lat1) * Math.Cos(dist)
                    + Math.Cos(lat1) * Math.Sin(dist) * Math.Cos(brng));

            double lon2 = lon1 + Math.Atan2(
                    Math.Sin(brng) * Math.Sin(dist) * Math.Cos(lat1),
                    Math.Cos(dist) - Math.Sin(lat1) * Math.Sin(lat2)
                );
            lon2 = (lon2 + 3 * Math.PI) % ((2 * Math.PI) - Math.PI);

            return new Coordinate(Math.Round(lat2.ToDegrees(), 5), Math.Round(lon2.ToDegrees(), 5));
        }

        // Another method
        private Coordinate ProjectX(double distance, double bearing)
        {
            var radius = 6371000;
            double lat1 = _coordinate.Latitude.ToRadians();
            double lon1 = _coordinate.Longitude.ToRadians();
            double brng = bearing.ToRadians();
            double lat2 = Math.Asin(Math.Sin(lat1) * Math.Cos(distance / radius) + Math.Cos(lat1) * Math.Sin(distance / radius) * Math.Cos(brng));
            double lon2 = lon1 + Math.Atan2(Math.Sin(brng) * Math.Sin(distance / radius) * Math.Cos(lat1), Math.Cos(distance / radius) - Math.Sin(lat1) * Math.Sin(lat2));
            return new Coordinate(Math.Round(lat2.ToDegrees(), 5), Math.Round(lon2.ToDegrees(), 5));
        }

    }
}
