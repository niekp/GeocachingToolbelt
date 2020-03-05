using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using GeocachingToolbelt.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GeocachingToolbelt.Controllers
{
    [Route("delen")]
    public class GpxController : Controller
    {
        public IActionResult Index()
        {
            return View(new GpxViewModel());
        }

        private string GetFilename(string guid)
        {
            return string.Format(@"wwwroot/GPX/{0}.gpx", guid);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(GpxViewModel importModel)
        {
            string ext = Path.GetExtension(importModel.GpxFile.FileName);
            if (ext != ".gpx")
            {
                ModelState.AddModelError(nameof(importModel.GpxFile), "Het gekozen bestand is geen GPX bestand.");
                return View("Index");
            }

            try
            {
                if (importModel.GpxFile != null && importModel.GpxFile.Length > 0)
                {
                    var guid = Guid.NewGuid().ToString();

                    var fileName = string.Concat(guid, ".gpx");
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), GetFilename(guid));
                    using var fileStream = new FileStream(filePath, FileMode.Create);
                    await importModel.GpxFile.CopyToAsync(fileStream);
                    return RedirectToAction(nameof(List), new { guid });
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(nameof(importModel.GpxFile), "Er ging iets fout bij het uploaden van het bestand.");
                return View("Index");
            }

            return RedirectToAction("index");
        }

        private List<Geocache> GetGeocaches(string guid)
        {
            var caches = new List<Geocache>();

            XmlDocument gpxDoc = new XmlDocument();
            gpxDoc.Load(GetFilename(guid));

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(gpxDoc.NameTable);
            nsmgr.AddNamespace("x", "http://www.topografix.com/GPX/1/0");
            XmlNodeList wps = gpxDoc.SelectNodes("//x:wpt", nsmgr);

            foreach (XmlNode wp in wps)
            {
                var latitude = double.Parse(wp.Attributes["lat"].Value);
                var longitude = double.Parse(wp.Attributes["lon"].Value);
                var gccode = wp["name"].InnerText;
                var name = wp["desc"].InnerText;

                caches.Add(new Geocache()
                {
                    GCCode = gccode,
                    Title = name,
                    Coordinate = new Coordinate(latitude, longitude),
                });
            }

            return caches.OrderBy(c => c.Title).ToList();
        }

        [Route("/gpx/list/{guid}")]
        public IActionResult List(string guid)
        {
            try
            {
                return View(GetGeocaches(guid));
            }
            catch (Exception)
            {
                return View("Error");
            }
        }


        [Route("/gpx/download/{guid}")]
        public IActionResult Download(string guid)
        {
            try
            {
                return PhysicalFile(
                    Path.Combine(Directory.GetCurrentDirectory(), GetFilename(guid)),
                    "application/gpx+xml",
                    GetFilename(guid)
                );
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

    }
}
