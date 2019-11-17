using System;
namespace GeocachingToolbelt.Models
{
    public class Triangle
    {
        public readonly Coordinate Centroid;

        public Triangle(Coordinate a, Coordinate b, Coordinate c)
        {
            decimal centroidNord = (a.Nord + b.Nord + c.Nord) / 3;
            decimal centroidEast = (a.East + b.East + c.East) / 3;
            Centroid = new Coordinate(centroidNord, centroidEast);
        }
    }
}
