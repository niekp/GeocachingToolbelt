using System.Collections.Generic;
using GeocachingToolbelt.Utils;
using Microsoft.AspNetCore.Mvc;

namespace GeocachingToolbelt.Controllers
{
    [Route("formule")]
    public class FormulaController : Controller
    {
        FormulaSolver formulaSolver;

        public FormulaController()
        {
            formulaSolver = new FormulaSolver();
        }
        
        [HttpGet]
        [Route("Waardes")]
        public IActionResult Values()
        {
            return View();
        }


        [HttpPost]
        [Route("Add")]
        public IActionResult Add(string name)
        {

            return RedirectToAction(nameof(Values));
        }

        [HttpPost]
        [Route("GetLetters")]
        public IActionResult GetLetters(string Formula)
        {
            return Json(formulaSolver.GetLetters(Formula));
        }

        [HttpPost]
        [Route("SolveFormula")]
        public string SolveFormula(string Formula, Dictionary<string, string> Letters = null)
        {
            return formulaSolver.SolveFormula(Formula, Letters);
        }
    }
}
