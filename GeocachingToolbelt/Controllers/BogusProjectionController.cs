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
        public IActionResult Index(string coord)
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

            ViewBag.Nord = new Projection(coordinate).Project(3218.688, 0);
            ViewBag.East = new Projection(coordinate).Project(3218.688, 90);
            ViewBag.South = new Projection(coordinate).Project(3218.688, 180);
            ViewBag.West = new Projection(coordinate).Project(3218.688, 270);
            ViewBag.Center = coordinate;

            return View();
        }


    }
}
