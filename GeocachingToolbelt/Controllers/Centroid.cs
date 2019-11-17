using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeocachingToolbelt.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GeocachingToolbelt.Controllers
{
    [Route("zwaartepunt")]
    public class Centroid : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string coords)
        {
            coords = coords.Replace("\r", "");
            ViewBag.Coords = coords;
            Coordinate c;
            decimal x = 0;
            decimal y = 0;
            var count = 0;

            try
            {
                foreach (var coord in coords.Split(Environment.NewLine))
                {
                    if (String.IsNullOrEmpty(coord))
                    {
                        continue;
                    }

                    c = new Coordinate(coord);
                    x += c.Nord;
                    y += c.East;
                    count++;
                }
                if (count == 0)
                {
                    throw new ArgumentException("No coordinates given");
                }
            }
            catch (ArgumentException)
            {
                ViewBag.Error = "Coordinaat niet herkend";
                return View();
            }

            ViewBag.Centroid = new Coordinate(x / count, y / count);

            return View();
        }
    }
}
