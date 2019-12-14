﻿using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GeocachingToolbelt.Models
{
    public class Coordinate
    {
        public decimal Nord, East;

        public Coordinate(string input)
        {
            input = Regex.Replace(input, "[^a-zA-Z0-9 .,]", "");

            if ((new string[] { "N", "E", "S", "W" }).Any(input.Contains))
            {
                ParseWSG84(input);
            }
            else if (
                input.Contains(".")
                && input.Contains(",")
                && Regex.Matches(input, @"[a-zA-Z]").Count == 0
            )
            {
                ParseDecimal(input);
            }
            else
            {
                throw new ArgumentException("Input not recognized as coordinate");
            }
        }


        public Coordinate(decimal nord, decimal east)
        {
            Nord = nord;
            East = east;
        }

        public void ParseWSG84(string wsg84)
        {
            wsg84 = Normalize(wsg84);
            var wsg_parts = wsg84.Split("E");
            var ns = 1;
            var ew = 1;

            if (wsg_parts.Length == 1)
            {
                wsg_parts = wsg84.Split("W");
                ew = -1;
            }

            if (wsg84.Contains("S"))
            {
                ns = -1;
            }

            if (wsg_parts.Length != 2)
            {
                throw new ArgumentException("Invalid coordinates");
            }

            var wsg_1 = GetWSGData(wsg_parts[0].Replace("N", "").Replace("S", ""));
            var wsg_2 = GetWSGData(wsg_parts[1].Replace("E", "").Replace("W", ""));

            Nord = DmsToDD(wsg_1.Degrees, wsg_1.Minutes, wsg_1.Seconds) * ns;
            East = DmsToDD(wsg_2.Degrees, wsg_2.Minutes, wsg_2.Seconds) * ew;
        }


        public string GetWSG84()
        {
            return String.Format("{0} {1} {2} {3}",
                (Nord < 0 ? "S" : "N"),
                DDToDM(Math.Abs(Nord)),
                (East < 0 ? "W" : "E"),
                DDToDM(Math.Abs(East))
            );
        }

        public string GetDecimal()
        {
            return String.Format("{0}, {1}", Decimal.Round(Nord, 5), Decimal.Round(East, 5));
        }

        private void ParseDecimal(string d)
        {
            var parts = d.Split(",");
            if (parts.Length != 2)
            {
                throw new ArgumentException("Invalid coordinate");
            }

            if (decimal.TryParse(parts[0], out var n))
            {
                Nord = n;
            }
            else
            {
                throw new ArgumentException("Invalid coordinate");
            }

            if (decimal.TryParse(parts[1], out var e))
            {
                East = e;
            }
            else
            {
                throw new ArgumentException("Invalid coordinate");
            }
        }

        private WSG GetWSGData(string coord_part)
        {
            var parts = coord_part.Trim().Split(" ");
            if (parts.Length < 2)
            {
                throw new ArgumentException("Invalid coordinate");
            }

            var degrees = parts[0];
            var minute = parts[1];
            var seconds = parts.Length == 3 ? parts[2] : "0";

            var wsg = new WSG();

            if (degrees != "" && double.TryParse(degrees, out var d))
            {
                wsg.Degrees = d;
            }
            else
            {
                throw new ArgumentException("Invalid coordinate");
            }

            if (minute != "" && double.TryParse(minute, out var m))
            {
                wsg.Minutes = m;
            }
            else
            {
                throw new ArgumentException("Invalid coordinate");
            }

            if (seconds != "" && double.TryParse(seconds, out var s))
            {
                wsg.Seconds = s;
            }
            else
            {
                throw new ArgumentException("Invalid coordinate");
            }

            return wsg;
        }

        private struct WSG
        {
            public double Degrees;
            public double Minutes;
            public double Seconds;
        }

        private string Normalize(string input)
        {
            input = Regex.Replace(input, "[^a-zA-Z0-9 .,]", "");
            input = input.Trim()
                        .ToUpper()
                        .Replace(", E", "E")
                        .Replace(", W", "W")
                        .Replace(",E", "E")
                        .Replace(",W", "W")
                        .Replace(",", ".");
            return input;
        }

        private decimal DmsToDD(double d, double m = 0, double s = 0)
        {
            return Convert.ToDecimal((d + (m / 60) + (s / 3600)) * (d < 0 ? -1 : 1));
        }

        private string DDToDM(decimal d)
        {
            decimal fraction = d - Math.Floor(d);
            return String.Format("{0} {1}", Math.Floor(d), Math.Round(fraction * 60, 3));
        }
    }
}
