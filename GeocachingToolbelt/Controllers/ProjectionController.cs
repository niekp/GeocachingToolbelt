using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeocachingToolbelt.Models;
using GeocachingToolbelt.Utils;
using Microsoft.AspNetCore.Mvc;

namespace GeocachingToolbelt.Controllers
{
    [Route("projectie")]
    [Route("projecteren")]
    public class ProjectionController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string coord, double distance, double angle)
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
            ViewBag.Distance = distance;
            ViewBag.Angle = angle;
            ViewBag.Coordinate = coordinate;
            ViewBag.Projected = coordinate.Project(distance, angle);

            return View();
        }
    }
}
