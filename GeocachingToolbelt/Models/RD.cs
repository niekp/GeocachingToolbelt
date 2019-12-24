using System;
namespace GeocachingToolbelt.Models
{
    public class RD
    {
        private double x;
        public double X { get => Math.Round(x); set => x = value; }

        private double y;
        public double Y { get => Math.Round(y); set => y = value; }


        public RD(double Latitude, double Longitude)
        {
            var g = 190066.98903;
            var a = -32.38360;
            var c = -0.00661;
            var e = -11830.85831;

            var b = -0.60639;
            var f = -114.19754;
            var h = 0.15774;
            var j = -2.34078;

            var i = -0.04158;
            var q = 3638.36193;
            var t = 0.09351;
            var w = 309020.31810;

            var x = -157.95222;
            var r = -0.05419;
            var o = 72.97141;
            var p = -6.43481;

            var v = 59.79734;
            var y = -0.07379;
            var s = -0.03444;

            var n = (Latitude - (-96.862 - 11.714 * (Latitude - 52) - 0.125 * (Longitude - 5)) / 100000 - 52.15616056) * 0.36;
            var m = (Longitude - (-37.902 + 0.329 * (Latitude - 52) - 14.667 * (Longitude - 5)) / 100000 - 5.38763889) * 0.36;
            var u = g * m + e * n * m + f * Math.Pow(n, 2) * m + a * Math.Pow(m, 3);
            u += j * Math.Pow(n, 3) * m + b * n * Math.Pow(m, 3) + h * Math.Pow(n, 2) * Math.Pow(m, 3);
            u += i * Math.Pow(n, 4) * m + c * Math.Pow(m, 5);
            var z = w * n + o * Math.Pow(n, 2) + q * Math.Pow(m, 2) + x * n * Math.Pow(m, 2);
            z += v * Math.Pow(n, 3) + p * Math.Pow(n, 2) * Math.Pow(m, 2) + s * Math.Pow(n, 4);
            z += t * Math.Pow(m, 4) + y * Math.Pow(n, 3) * Math.Pow(m, 2) + r * n * Math.Pow(m, 4);

            X = 155000 + u;
            Y = 463000 + z;
        }
    }
}
