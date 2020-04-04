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
                        && (bracketOpen > 0))
                    {
                        coordinate.Remove(pos, 1);
                        coordinate.Insert(pos, letter.Value);
                        pos += letter.Value.Length - 1; // Move the cursor along if the value is longer then 1 position
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
            var sums = GetSums(Formula);
            var characters = "";
            foreach (var sum in sums)
            {
                characters += sum;
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
