using System;
using System.Collections.Generic;

namespace WordMask.Models
{
    public class Ruleset
    {
        private int length;
        private Dictionary<int, List<int>> matchingLetters = new Dictionary<int, List<int>>();
        private Dictionary<int, List<int>> nonMatchingLetters = new Dictionary<int, List<int>>();
        private List<string> notContain;
        private List<string> contains;
        private Dictionary<char, char> knownLetters;
        private string mask;

        public Ruleset(string mask, List<string> notContain = null, List<string> contains = null, Dictionary<char, char> knownLetters = null)
        {
            this.mask = mask.ToUpper();
            if (notContain != null)
            {
                this.notContain = notContain;
            }
            if (notContain != null)
            {
                this.contains = contains;
            }
            if (knownLetters != null)
            {
                this.knownLetters = knownLetters;
            }

            length = mask.Length;
            SetMatchingLetters();
            SetNonMatchingLetters();
            Console.WriteLine(nonMatchingLetters);
        }

        public bool IsMatch(string word)
        {
            if (word.Length != length)
            {
                return false;
            }

            foreach (KeyValuePair<int, List<int>> entry in matchingLetters)
            {
                foreach (int i in entry.Value)
                {
                    if (word[entry.Key] != word[i])
                    {
                        return false;
                    }
                }
            }

            foreach (KeyValuePair<int, List<int>> entry in nonMatchingLetters)
            {
                foreach (int i in entry.Value)
                {
                    if (word[entry.Key] == word[i])
                    {
                        return false;
                    }
                }
            }

            foreach (var letter in notContain)
            {
                if (letter != "" && word.Contains(letter))
                {
                    return false;
                }
            }

            foreach (var letter in contains)
            {
                if (letter != "" && !word.Contains(letter))
                {
                    return false;
                }
            }

            foreach (var letter in knownLetters)
            {
                int position = mask.IndexOf(letter.Key);
                if (position >= 0 && word[position] != letter.Value)
                {
                    return false;
                }
            }


            return true;
        }

        private void SetMatchingLetters()
        {
            int indexA = 0;
            foreach (var letter in mask)
            {
                int indexB = 0;
                foreach (var letter2 in mask)
                {
                    if (letter == letter2 && indexA != indexB)
                    {
                        if (!matchingLetters.ContainsKey(indexA))
                        {
                            matchingLetters.Add(indexA, new List<int>());
                        }
                        matchingLetters[indexA].Add(indexB);
                    }
                    indexB++;
                }
                indexA++;
            }
        }

        private void SetNonMatchingLetters()
        {
            int indexA = 0;
            foreach (var letter in mask)
            {
                int indexB = 0;
                foreach (var letter2 in mask)
                {
                    if (letter != letter2)
                    {
                        if (!nonMatchingLetters.ContainsKey(indexA))
                        {
                            nonMatchingLetters.Add(indexA, new List<int>());
                        }
                        nonMatchingLetters[indexA].Add(indexB);
                    }
                    indexB++;
                }
                indexA++;
            }
        }
    }
}
