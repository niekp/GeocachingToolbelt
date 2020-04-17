using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NCalc;

namespace GeocachingToolbelt.Utils
{
    public class FormulaSolver
    {

        public List<string> GetSums(string Formula)
        {
            var bracketOpen = 0;
            int sumStart = 0;
            var sums = new List<string>();

            for (int pos = 0; pos < Formula.Length; pos++)
            {
                var character = Formula[pos];

                if (character == '(')
                {
                    if (bracketOpen == 0)
                    {
                        sumStart = pos;
                    }
                    bracketOpen++;
                }

                if (character == ')')
                {
                    bracketOpen--;
                    if (bracketOpen == 0)
                    {
                        sums.Add(Formula.Substring(sumStart, pos - sumStart + 1));
                    }
                }
            }

            return sums;
        }

        public string ReplaceLetters(string Formula, Dictionary<string, string> Letters)
        {
            var coordinate = new StringBuilder(Formula.ToUpper());
            var blacklist = new string[] { "N", "E", "S", "W" };

            // Replace the letters in the formula.
            foreach (var letter in Letters)
            {
                var bracketOpen = 0;

                int pos = 0;
                while (pos < coordinate.Length)
                {
                    var character = coordinate[pos];

                    if (character == Convert.ToChar(letter.Key)
                        && !string.IsNullOrEmpty(letter.Value)
                    ) {
                        var onBlacklist = blacklist.Contains(letter.Key);
                        if (onBlacklist)
                        {
                            // An N is only bad as first character
                            if ((letter.Key == "N" || letter.Key == "S") && pos > 0)
                            {
                                onBlacklist = false;
                            }
                            // Skip the E only if it's surrounded by spaces or space E 00
                            if (letter.Key == "E" || letter.Key == "W")
                            {
                                // No worries if its at the end.
                                if (pos + 2 > coordinate.Length || pos < 1)
                                {
                                    onBlacklist = false;
                                }

                                if (onBlacklist &&
                                    (coordinate[pos - 1] != ' ')
                                    && (
                                        (coordinate[pos + 1] != ' ')
                                        || (coordinate[pos + 1] != '0' && coordinate[pos + 2] != '0')
                                    )
                                )
                                {
                                    onBlacklist = false;
                                }
                            }
                        }

                        if (bracketOpen > 0 || !onBlacklist)
                        {
                            coordinate.Remove(pos, 1);
                            coordinate.Insert(pos, letter.Value);
                            pos += letter.Value.Length - 1; // Move the cursor along if the value is longer then 1 position
                        }
                    }

                    if (character == '(')
                    {
                        bracketOpen++;
                    }

                    if (character == ')')
                    {
                        bracketOpen--;
                    }

                    pos++;
                }

            }

            return coordinate.ToString();
        }

        public string SolveSum(string sum)
        {
            try
            {
                var expr = new Expression(sum);
                Func<int> f = expr.ToLambda<int>();
                return f().ToString();
            }
            catch (Exception)
            {
                return sum;
            }

        }

        public char[] GetLetters(string Formula)
        {
            if (string.IsNullOrEmpty(Formula))
            {
                return new char[0];
            }

            Formula = Formula.ToUpper();


            var characters = "";

            var blacklist = new char[] { 'N', 'E', 'S', 'W' };
            var bracketOpen = 0;
            int pos = 0;

            while (pos < Formula.Length)
            {
                var character = Formula[pos];

                var onBlacklist = blacklist.Contains(character);
                if (onBlacklist)
                {
                    // An N is only bad as first character
                    if ((character == 'N' || character == 'S') && pos > 0)
                    {
                        onBlacklist = false;
                    }
                    // Skip the E only if it's surrounded by spaces or space E 00
                    if (character == 'E' || character == 'W')
                    {
                        // No worries if its at the end.
                        if (pos + 2 > Formula.Length || pos < 1)
                        {
                            onBlacklist = false;
                        }

                        if (onBlacklist &&
                            (Formula[pos - 1] != ' ')
                            && (
                                (Formula[pos + 1] != ' ')
                                || (Formula[pos + 1] != '0' && Formula[pos + 2] != '0')
                            )
                        )
                        {
                            onBlacklist = false;
                        }
                    }
                }

                if (bracketOpen > 0 || !onBlacklist)
                {
                    characters += character;
                }

                if (character == '(')
                {
                    bracketOpen++;
                }

                if (character == ')')
                {
                    bracketOpen--;
                }

                pos++;
            }

            var unique = new string(characters.Distinct().ToArray()).ToUpper();

            return Regex.Replace(unique, "[^A-Z]", String.Empty).ToArray();
        }

        public string SolveFormula(string Formula, Dictionary<string, string> Letters)
        {
            Formula = Formula.ToUpper();
            Formula = ReplaceLetters(Formula, Letters);
            var sums = GetSums(Formula);
            var coordinate = new StringBuilder(Formula);

            foreach (var sum in sums)
            {
                coordinate.Replace(sum, SolveSum(sum));
            }

            return coordinate.ToString();
        }
    }
}
