using System;
using System.Collections.Generic;

namespace GeocachingToolbelt.Models
{
    public class MapData
    {
        public List<Coordinate> Coordinates = new List<Coordinate>();
        public List<Geocache> Geocaches = new List<Geocache>();
        public Coordinate Hightlight;
        public double Radius;
    }
}
