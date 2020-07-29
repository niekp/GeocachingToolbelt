using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeocachingToolbelt.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GeocachingToolbelt.Controllers
{
    public class LeafletHelper : Controller
    {
        public IActionResult CoordinatePartial(double lat, double lng)
        {
            return View("/Views/Shared/_CoordinatePartial.cshtml", new Coordinate(lat, lng));
        }
    }
}
