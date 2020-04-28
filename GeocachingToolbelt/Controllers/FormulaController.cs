using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GeocachingToolbelt.Data;
using GeocachingToolbelt.Models;
using GeocachingToolbelt.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GeocachingToolbelt.Controllers
{
    [Route("formule")]
    public class FormulaController : Controller
    {
        FormulaSolver formulaSolver;
        ToolbeltContext db;

        public FormulaController(ToolbeltContext toolbeltContext)
        {
            formulaSolver = new FormulaSolver();
            db = toolbeltContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("Waardes/{guid}")]
        public IActionResult Values(string guid)
        {
            var multi = GetMulti(guid);
            if (!(multi is Multi))
            {
                return RedirectToAction("Index");
            }

            return View(multi);
        }

        [HttpGet]
        [Route("Waypoints/{guid}")]
        public IActionResult Waypoints(string guid)
        {
            var multi = GetMulti(guid);
            if (!(multi is Multi))
            {
                return RedirectToAction("Index");
            }

            return View(multi);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult Add(string name)
        {
            var multi = new Multi()
            {
                GUID = Guid.NewGuid().ToString(),
                Name = name
            };

            db.Add(multi);
            db.SaveChanges();

            return RedirectToAction("Waypoints", new { multi.GUID });
        }

        [HttpPost]
        [Route("SaveWaypoints")]
        public IActionResult SaveWaypoints(string guid, string name, string waypoints)
        {
            var multi = GetMulti(guid);
            if (!(multi is Multi))
            {
                return RedirectToAction("Index");
            }

            multi.Name = name;
            var newWaypoints = Regex.Split(Regex.Replace(waypoints, "^[,\r\n]+|[,\r\n]+$", ""), "[,\r\n]+");

            // Remove all waypoints and readd to lazily keep the order
            foreach (var wp in multi.Waypoints)
            {
                db.Remove(wp);
            }

            var number = 1;
            foreach (var wp in newWaypoints)
            {
                db.Add(new Waypoint()
                {
                    Number = number,
                    Coordinate = wp,
                    MultiId = multi.Id
                });
                number++;
            }

            db.SaveChanges();

            return RedirectToAction("Values", new { multi.GUID });
        }

        [HttpPost]
        [Route("SolveFormula")]
        public IActionResult SolveFormula(string Guid, int WP, Dictionary<string, string> Letters = null)
        {
            var multi = GetMulti(Guid);
            if (multi is Multi)
            {
                // Save letters
                foreach (var letter in Letters)
                {
                    var currentValue = multi.Variables.Where(v => v.Letter == letter.Key).FirstOrDefault();

                    if (currentValue is Variable)
                    {
                        currentValue.Value = letter.Value;
                        db.Update(currentValue);
                    }
                    else
                    {
                        db.Add(new Variable()
                        {
                            MultiId = multi.Id,
                            Letter = letter.Key,
                            Value = letter.Value
                        });
                    }
                }

                db.SaveChanges();
            }


            var result = new List<FormulaResult>();

            foreach (var waypoint in multi.Waypoints.Where(wp => wp.Number <= WP))
            {
                var solved = formulaSolver.SolveFormula(waypoint.Coordinate, Letters);

                Coordinate coordinate = null;

                try
                {
                    coordinate = new Coordinate(solved);
                }
                catch (ArgumentException) { }

                result.Add(new FormulaResult()
                {
                    Result = solved,
                    Latitude = coordinate?.Latitude,
                    Longitude = coordinate?.Longitude,
                    WSG = coordinate?.GetWSG84()
                });
            }

            return Json(result);
        }


        /**
         * Repo stuff
         */

        private Multi GetMulti(string guid)
        {
            return db.Multi.Where(m => m.GUID == guid)
                .Include(m => m.Waypoints)
                .Include(m => m.Variables)
                .FirstOrDefault();
        }

    }
}
