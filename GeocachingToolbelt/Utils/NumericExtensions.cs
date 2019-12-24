using System;
namespace GeocachingToolbelt.Utils
{
    public static class NumericExtensions
    {
        public static double ToRadians(this double val)
        {
            return val * (Math.PI / 180);
        }

        public static double ToDegrees(this double angle)
        {
            return angle * (180.0 / Math.PI);
        }
    }
}
