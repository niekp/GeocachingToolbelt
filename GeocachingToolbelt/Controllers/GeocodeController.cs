using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeocachingToolbelt.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GeocachingToolbelt.Controllers
{
    public class GeocodeController : Controller
    {
		private readonly IConfiguration configuration;

		public GeocodeController(IConfiguration configuration) {
			this.configuration = configuration;
		}

		public async Task<IActionResult> Index()
        {
			var geocoder = new Geocoder(configuration);
			var x = await geocoder.Search("Kloekhorststraat 6, Assen");
            return View();
        }
    }
}