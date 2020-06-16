using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeocachingToolbelt.Models;
using Microsoft.AspNetCore.Mvc;
using GeocachingToolbelt.Utils;

namespace GeocachingToolbelt.Controllers
{
    public class GeoartController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Distance = 100;
            return View();
        }

        [HttpPost]
        public IActionResult Index(string CSV, string coord, int distance = 100)
        {
            ViewBag.CSV = CSV;
            ViewBag.Coordinate = coord;
            ViewBag.Distance = distance;

            Coordinate coordinate;
            try
            {
                coordinate = new Coordinate(coord);
            }
            catch (ArgumentException)
            {
                ViewBag.Error = "Coordinaat niet herkend";
                return View();
            }

            var coordinates = new List<Coordinate>();

            var nord = coordinate;

            foreach (string row in CSV.Split(Environment.NewLine))
            {
                var east = nord;
                foreach (var column in row.Split(';'))
                {
                    if (column.Trim().Length > 0)
                    {
                        coordinates.Add(new Coordinate(nord.Latitude, east.Longitude));
                    }
                    east = east.Project(distance, 90);
                }

                nord = nord.Project(distance, 180);
            }

            ViewBag.Coordinates = coordinates;

            return View();
        }
    }
}
