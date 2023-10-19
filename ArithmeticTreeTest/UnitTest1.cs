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
            string[] expected = { "3", "X", "+", "5", "*" };
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
            string[] expected = { "3", "X", "+", "5", "*" };
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
            string[] expected = { "3", "X", "+", "5", "*" };
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


        [Test]

        public void TestEvaluate_DivideByZero() 
        {
            EvaluationTree test = new EvaluationTree("3 + 5 / 0");
            Assert.Throws<DivideByZeroException>(() => test.Evaluate());
        }
    }
}