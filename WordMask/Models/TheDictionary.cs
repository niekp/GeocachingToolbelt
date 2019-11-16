using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WordMask.Models
{
    public class TheDictionary
    {
        private IEnumerable<string> dictionary;

        /// <summary>
        /// Read the dictionary file to load in all posible words
        /// </summary>
        public TheDictionary()
        {
            dictionary = File.ReadLines("dict.txt");
        }

        /// <summary>
        /// Look for dictionary matches given a ruleset
        /// </summary>
        /// <param name="ruleset"></param>
        /// <returns></returns>
        public List<string> FindMatches(Ruleset ruleset)
        {
            return dictionary.Where(ruleset.IsMatch).ToList();
        }

        
    }
}
