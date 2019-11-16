using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WordMask.Models;

namespace WordMask.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TheDictionary _dictionary;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _dictionary = new TheDictionary();
        }

        public IActionResult Index()
        {
            return View();
        }

        private Dictionary<char, char> GetLetterMatches(string input)
        {
            Dictionary<char, char> output = new Dictionary<char, char>();
            foreach (var l in input.Split(",").Select(l => l.Trim().ToUpper()).ToList())
            {
                var x = l.Split("=");
                if (x.Length == 2)
                {
                    output.Add(char.Parse(x[0].Substring(0, 1)), char.Parse(x[1].Substring(0, 1)));
                }
            }

            return output;
        }

        [HttpPost]
        public IActionResult Index(string mask, string notcontain = "", string contains = "", string knownletters = "")
        {
            if (notcontain == null)
            {
                notcontain = "";
            }
            if (contains == null)
            {
                contains = "";
            }
            if (knownletters == null)
            {
                knownletters = "";
            }

            ViewBag.Mask = mask;
            ViewBag.NotContain = notcontain;
            ViewBag.Contains = contains;
            ViewBag.KnownLetters = knownletters;

            var ruleset = new Ruleset(
                mask,
                notcontain.Split(",").Select(l => l.Trim().ToUpper()).ToList(),
                contains.Split(",").Select(l => l.Trim().ToUpper()).ToList(),
                GetLetterMatches(knownletters)
            );
            
            ViewBag.Matches = _dictionary.FindMatches(ruleset);
            return View();
        }


    }
}
