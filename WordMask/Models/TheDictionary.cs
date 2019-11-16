using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WordMask.Models
{
    public class TheDictionary
    {
        private IEnumerable<string> dictionary;

        public TheDictionary()
        {
            dictionary = File.ReadLines("dict.txt");
        }

        public bool IsWord(string word)
        {
            word = word.ToUpper();
            var match = dictionary
                .SkipWhile(line => !line.Equals(word))
                .Take(1).FirstOrDefault();

            return match != null;
        }

        public List<string> FindMatches(Ruleset ruleset)
        {
            var matches = new List<string>();

            foreach (var word in dictionary)
            {
                if (ruleset.IsMatch(word))
                {
                    matches.Add(word);
                }
            }

            return matches;
        }

        
    }
}
