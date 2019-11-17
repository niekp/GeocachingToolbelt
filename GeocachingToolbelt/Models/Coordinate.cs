using System;
namespace GeocachingToolbelt.Models
{
    public class Coordinate
    {
        public string WSG84;
        public decimal Nord, East;

        public Coordinate(string wsg84)
        {
            WSG84 = Normalize(wsg84);

            if ((wsg84.Substring(0, 1)) != "N" || !wsg84.Contains("E"))
            {
                throw new ArgumentException("Only nord/east coordinates are suppported");
            }

            var nord = wsg84.Split("E")[0].Replace("N", "").Trim();
            var east = wsg84.Split("E")[1].Replace("E", "").Trim();
            var wsg_nord = GetWSGData(nord);
            var wsg_east = GetWSGData(east);

            Nord = DMToDD(wsg_nord.Degrees, wsg_nord.Minutes);
            East = DMToDD(wsg_east.Degrees, wsg_east.Minutes);
        }

        public Coordinate(decimal nord, decimal east)
        {
            Nord = nord;
            East = east;

            WSG84 = String.Format("N {0} E {1}", DDToDM(Nord), DDToDM(East));
        }

        public string GetDecimal()
        {
            return String.Format("{0}, {1}", Decimal.Round(Nord, 5), Decimal.Round(East, 5));
        }

        private WSG GetWSGData(string coord_part)
        {
            var parts = coord_part.Split(" ");
            if (parts.Length != 2)
            {
                throw new ArgumentException("Invalid coordinate");
            }

            var degrees = parts[0];
            var minute = parts[1];
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

            return wsg;
        }

        private struct WSG
        {
            public double Degrees;
            public double Minutes;
        }

        private string Normalize(string input)
        {
            return input.Trim().ToUpper().Replace(",", ".");
        }

        private decimal DMToDD(double d, double m = 0)
        {
            return Convert.ToDecimal((d + (m / 60)) * (d < 0 ? -1 : 1));
        }

        private string DDToDM(decimal d)
        {
            decimal fraction = d - Math.Floor(d);
            return String.Format("{0} {1}", Math.Floor(d), Math.Round(fraction * 60, 3));
        }
    }
}
