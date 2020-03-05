using System;
namespace GeocachingToolbelt.Models
{
    public class Geocache
    {
        public string GCCode { get; set; }
        public string Title { get; set; }
        public Coordinate Coordinate { get; set; }
    }
}
