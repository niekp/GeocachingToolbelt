using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeocachingToolbelt.Models;
using GeocachingToolbelt.Utils;
using Microsoft.AspNetCore.Mvc;

namespace GeocachingToolbelt.Controllers
{
    [Route("bogus")]
    public class BogusProjectionController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string coord, double meter = 3218.688)
        {
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

            ViewBag.Meter = meter;
            ViewBag.Nord = coordinate.Project(meter, 0);
            ViewBag.East = coordinate.Project(meter, 90);
            ViewBag.South = coordinate.Project(meter, 180);
            ViewBag.West = coordinate.Project(meter, 270);
            ViewBag.Center = coordinate;

            return View();
        }


    }
}
