using System;
using System.Collections.Generic;

namespace WordMask.Models
{
    public class Ruleset
    {
        private readonly int _length;
        private readonly Dictionary<int, List<int>> _matchingLetters = new Dictionary<int, List<int>>();
        private readonly Dictionary<int, List<int>> _nonMatchingLetters = new Dictionary<int, List<int>>();
        private readonly List<string> _notContain;
        private readonly List<string> _contains;
        private readonly Dictionary<char, char> _knownLetters;
        private readonly string _mask;

        /// <summary>
        /// Create a ruleset to match a word against
        /// </summary>
        /// <param name="mask">The masked word</param>
        /// <param name="notContain">List of letters the word may not contain</param>
        /// <param name="contains">List of letters the word must contain</param>
        /// <param name="knownLetters">List of known matches (a=b, c=d, ..)</param>
        public Ruleset(string mask, List<string> notContain = null, List<string> contains = null, Dictionary<char, char> knownLetters = null)
        {
            _mask = mask.ToUpper();
            _notContain = notContain ?? _notContain;
            _contains = contains ?? _contains;
            _knownLetters = knownLetters ?? _knownLetters;
            _length = mask.Length;

            SetMatchingLetters();
            SetNonMatchingLetters();
        }

        /// <summary>
        /// Does a word match the ruleset
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public bool IsMatch(string word)
        {
            // Match on length
            if (word.Length != _length)
            {
                return false;
            }

            // Find matching letters on the same positions as the mask
            foreach (KeyValuePair<int, List<int>> entry in _matchingLetters)
            {
                foreach (int i in entry.Value)
                {
                    if (word[entry.Key] != word[i])
                    {
                        return false;
                    }
                }
            }

            // Find non matching letters on the same positions as the mask
            foreach (KeyValuePair<int, List<int>> entry in _nonMatchingLetters)
            {
                foreach (int i in entry.Value)
                {
                    if (word[entry.Key] == word[i])
                    {
                        return false;
                    }
                }
            }

            // Check if the word contains letters that are listed in 'not contain'
            if (_notContain.Find(l => !string.IsNullOrEmpty(l) && word.Contains(l)) != null)
            {
                return false;
            }

            // Check if the word is missing letters listed in 'contains'
            if (_contains.Find(l => !string.IsNullOrEmpty(l) && !word.Contains(l)) != null)
            {
                return false;
            }
            
            // Check if the known letter matches match up
            foreach (var letter in _knownLetters)
            {
                int position = _mask.IndexOf(letter.Key);
                if (position >= 0 && word[position] != letter.Value)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Internal helper function to fill the 'matchingLetters' dictionary based on the mask
        /// </summary>
        private void SetMatchingLetters()
        {
            int indexA = 0;
            foreach (var letter in _mask)
            {
                int indexB = 0;
                foreach (var letter2 in _mask)
                {
                    if (letter == letter2 && indexA != indexB)
                    {
                        if (!_matchingLetters.ContainsKey(indexA))
                        {
                            _matchingLetters.Add(indexA, new List<int>());
                        }
                        _matchingLetters[indexA].Add(indexB);
                    }
                    indexB++;
                }
                indexA++;
            }
        }

        /// <summary>
        /// Internal helper function to fill the 'nonMatchingLetters' dictionary based on the mask
        /// </summary>
        private void SetNonMatchingLetters()
        {
            int indexA = 0;
            foreach (var letter in _mask)
            {
                int indexB = 0;
                foreach (var letter2 in _mask)
                {
                    if (letter != letter2)
                    {
                        if (!_nonMatchingLetters.ContainsKey(indexA))
                        {
                            _nonMatchingLetters.Add(indexA, new List<int>());
                        }
                        _nonMatchingLetters[indexA].Add(indexB);
                    }
                    indexB++;
                }
                indexA++;
            }
        }

    }
}
