using NUnit.Framework.Internal;
using Spreadsheet_Engine;

namespace EvaluationTreeTest
{
    /// <summary>
    /// Tests for EvaluationTree Class Methods.
    /// </summary>
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }



        /// <summary>
        /// Tests EvaluationTree Constructor to Ensure it throws error on invalid operator input.
        /// </summary>
        [Test]
        public void Test_InvalidOperator()
        {
            Assert.Throws<InvalidOperationException>(() => new EvaluationTree("3 ^ 3 + 5"));
        }

        /// <summary>
        /// Tests EvaluationTree.Parse() on Normal Input.
        /// </summary>
        [Test]
        public void TestParse()
        {
            string expression = "3 + X * 5";
            string[] expected = { "3", "X", "5", "*", "+" };
            string[] actual = EvaluationTree.Parse(expression);
            CollectionAssert.AreEqual(expected, actual);
        }


        /// <summary>
        /// Tests EvaluationTree.Parse() On an input that contains extra spaces.
        /// </summary>
        [Test]
        public void TestParse_WithSpaces()
        {
            string expression = " 3 + X * 5 ";
            string[] expected = { "3", "X", "5", "*", "+" };
            string[] actual = EvaluationTree.Parse(expression);
            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests EvaluationTree.Parse() On an input that contains extra spaces.
        /// </summary>
        [Test]
        public void TestParse_WithoutSpaces()
        {
            string expression = "3+X*5";
            string[] expected = { "3", "X", "5", "*", "+" };
            string[] actual = EvaluationTree.Parse(expression);
            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests EvaluationTree.Evaluate() on normal input.
        /// </summary>
        [Test]
        public void TestEvaluate()
        {
            EvaluationTree test = new EvaluationTree("3 + 3 + 5");
            Assert.That(test.Evaluate(), Is.EqualTo(11));
        }

        /// <summary>
        /// Tests edge case - divide by sero.
        /// </summary>
        [Test]
        public void TestEvaluate_DivideByZero()
        {
            EvaluationTree test = new EvaluationTree("3 + 5 / 0");
            Assert.Throws<DivideByZeroException>(() => test.Evaluate());
        }

        /// <summary>
        /// Tests Parse() function on a more complicated input.
        /// </summary>
        [Test]
        public void TestParse_ComplicatedInput()
        {
            string expression = " 9 * (4 + (2 * 5)) / 5 * (3 + 6) ";
            string[] expected = { "9", "4", "2", "5", "*", "+", "*", "5", "/", "3", "6", "+", "*" };
            string[] actual = EvaluationTree.Parse(expression);
            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests evaluate on a more complicated input.
        /// </summary>
        [Test]
        public void TestEvaluate_ComplicatedInput()
        {
            EvaluationTree test = new EvaluationTree("9 / (2 + (3 * 7) / 4) + 9");
            Assert.That(test.Evaluate(), Is.EqualTo(10.241379310344827d));
        }

    }
}