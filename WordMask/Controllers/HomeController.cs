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

        private Dictionary<char, char> GetAssociations(string input)
        {
            Dictionary<char, char> output = new Dictionary<char, char>();
            foreach (var l in input.Split(",").Select(l => l.Trim().ToUpper()).ToList())
            {
                var association = l.Split("=");
                if (association.Length == 2)
                {
                    var a = char.Parse(association[0].Substring(0, 1));
                    var b = char.Parse(association[1].Substring(0, 1));
                    if (!output.ContainsKey(a))
                    {
                        output.Add(a, b);
                    }
                }
            }

            return output;
        }

        private string GetReplacedWord(string mask, Dictionary<char, char> associations)
        {
            string replaced = "";
            for (var i = 0; i < mask.Length; i++)
            {
                if (associations.ContainsKey(mask[i]))
                {
                    replaced += associations[mask[i]];
                }
                else
                {
                    replaced += "?";
                }
            }

            return replaced;
        }

        [HttpPost]
        public IActionResult Index(string mask, string notContains = "", string contains = "", string knownLetters = "")
        {
            notContains ??= "";
            contains ??= "";
            knownLetters ??= "";
            var associations = GetAssociations(knownLetters);

            ViewBag.Mask = mask;
            ViewBag.NotContains = notContains;
            ViewBag.Contains = contains;
            ViewBag.KnownLetters = knownLetters;
            ViewBag.Automode = Request.Form["notcontains-automode"] == "on";

            var ruleset = new Ruleset(
                mask,
                notContains.Split(",").Select(l => l.Trim().ToUpper()).ToList(),
                contains.Split(",").Select(l => l.Trim().ToUpper()).ToList(),
                associations
            );
            
            ViewBag.Matches = _dictionary.FindMatches(ruleset);
            ViewBag.ReplacedWord = GetReplacedWord(mask, associations);

            return View();
        }


    }
}
