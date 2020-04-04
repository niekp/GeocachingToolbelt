using NUnit.Framework;
using GeocachingToolbelt.Controllers;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    public class FormulaControllerTest
    {
        FormulaController formulaController;

        [SetUp]
        public void Setup()
        {
            formulaController = new FormulaController();
        }

        [Test]
        public void SumFinder()
        {
            var geenformule = "1234";
            Assert.Zero(formulaController.GetSums(geenformule).Count());

            var simpeleformule = "(1-2)";
            Assert.AreEqual("(1-2)", formulaController.GetSums(simpeleformule).FirstOrDefault());

            var metletters = "(a-b)";
            Assert.AreEqual("(a-b)", formulaController.GetSums(metletters).FirstOrDefault());

            var formuleinmidden = "1234(1-2)5678";
            Assert.AreEqual(1, formulaController.GetSums(formuleinmidden).Count());
            Assert.AreEqual("(1-2)", formulaController.GetSums(formuleinmidden).FirstOrDefault());

            var tweeformules = "N(1-2)E(3-4)";
            Assert.AreEqual(2, formulaController.GetSums(tweeformules).Count());
            Assert.AreEqual("(1-2)", formulaController.GetSums(tweeformules).FirstOrDefault());
            Assert.AreEqual("(3-4)", formulaController.GetSums(tweeformules).LastOrDefault());

            var genesteformule = "9876(1-(2*5))1235";
            Assert.AreEqual(1, formulaController.GetSums(genesteformule).Count());
            Assert.AreEqual("(1-(2*5))", formulaController.GetSums(genesteformule).FirstOrDefault());

            var tweegenesteformule = "9876(1-(2*5))123(1+(2+(3+4)+5)+6)5";
            Assert.AreEqual(2, formulaController.GetSums(tweegenesteformule).Count());
            Assert.AreEqual("(1-(2*5))", formulaController.GetSums(tweegenesteformule).FirstOrDefault());
            Assert.AreEqual("(1+(2+(3+4)+5)+6)", formulaController.GetSums(tweegenesteformule).LastOrDefault());
        }

        [Test]
        public void SolveSom()
        {
            Assert.AreEqual("3", formulaController.SolveSum("1+2"));
            Assert.AreEqual("7", formulaController.SolveSum("(2+5)"));
            Assert.AreEqual("10", formulaController.SolveSum("(2*5)"));
        }

        [Test]
        public void GetLetters()
        {
            Assert.AreEqual(new char[] { 'A', 'B' }, formulaController.GetLetters("(a+b)"));
            Assert.AreEqual(new char[] { 'B', 'C', 'D', 'F' }, formulaController.GetLetters("A(B+C/D)E(F)"));
        }

        
        [Test]
        public void ReplaceLetters()
        {
            // Simpel
            Assert.AreEqual("N 00° 00.00(5)' E 0° 0.(Y)200'",
                formulaController.ReplaceLetters("N 00° 00.00(X)' E 0° 0.(Y)00'", new Dictionary<string, string>()
                {
                    { "X", "5" },
                    { "Y", "2" }
                }));

            // Behoud E buiten haakjes
            Assert.AreEqual("N 00° 00.005' E 0° 0.200'",
                formulaController.ReplaceLetters("N 00° 00.005' E 0° 0.200'", new Dictionary<string, string>()
                {
                    { "E", "5" },
                }));

            // Vervang E binnen haakjes
            Assert.AreEqual("N 00° 00.00(5)' E 0° 0.(5-3)00'",
                formulaController.ReplaceLetters("N 00° 00.00(E)' E 0° 0.(E-3)00'", new Dictionary<string, string>()
                {
                    { "E", "5" },
                }));
        }

        [Test]
        public void SolveFormula()
        {
            var coordinate = "N 00° 00.(100+X)' E 0° 0.(200-Y)'";

            var result = formulaController.SolveFormula(coordinate, new Dictionary<string, string>()
            {
                { "X", "5" },
                { "Y", "2" }
            });

            Assert.AreEqual("N 00° 00.105' E 0° 0.198'", result);

            coordinate = "N 00° 00.5(A+(B+(C+D)+E)+F)' E 0° 0.(A*(B+(C+D)*E)*F)'";

            result = formulaController.SolveFormula(coordinate, new Dictionary<string, string>()
            {
                { "A", "1" },
                { "B", "2" },
                { "C", "3" },
                { "D", "4" },
                { "E", "5" },
                { "F", "6" },
            });

            Assert.AreEqual("N 00° 00.521' E 0° 0.222'", result);

            result = formulaController.SolveFormula("123(B)678", new Dictionary<string, string>()
            {
                { "B", "5" },
            });

            Assert.AreEqual("1235678", result);
        }
    }
}