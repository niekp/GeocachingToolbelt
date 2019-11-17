using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeocachingToolbelt.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GeocachingToolbelt.Controllers
{
    public class Centroid : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string coordA, string coordB, string coordC)
        {
            ViewBag.CoordA = coordA;
            ViewBag.CoordB = coordB;
            ViewBag.CoordC = coordC;

            Coordinate a, b, c;

            try
            {
                a = new Coordinate(coordA);
            }
            catch (ArgumentException)
            {
                ViewBag.Error = "Coordinaat A is niet juist ingevoerd";
                return View();
            }

            try
            {
                b = new Coordinate(coordB);
            }
            catch (ArgumentException)
            {
                ViewBag.Error = "Coordinaat B is niet juist ingevoerd";
                return View();
            }

            try
            {
                c = new Coordinate(coordC);
            }
            catch (ArgumentException)
            {
                ViewBag.Error = "Coordinaat C is niet juist ingevoerd";
                return View();
            }

            var triangle = new Triangle(a, b, c);
            ViewBag.Centroid = triangle.Centroid.WSG84;

            return View();
        }
    }
}
