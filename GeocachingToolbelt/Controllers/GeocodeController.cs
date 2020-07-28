using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeocachingToolbelt.Models;
using GeocachingToolbelt.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GeocachingToolbelt.Controllers
{
	[Route("Adres")]
    public class GeocodeController : Controller
    {
		private readonly IConfiguration configuration;

		public GeocodeController(IConfiguration configuration) {
			this.configuration = configuration;
		}

		public IActionResult Index()
        {
            return View();
        }

		[HttpPost]
		public async Task<IActionResult> Index(string Zoek) {
			var geocoder = new Geocoder(configuration);
			var result = await geocoder.Search(Zoek);
			ViewBag.Zoek = Zoek;
			if (!(result is GeocodeResult)) {
				ViewBag.Error = "Geen resultaten";
			} else {
				ViewBag.Result = result;
			}

			return View();
		}
    }
}