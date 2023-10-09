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
        /// Tests EvaluationTree.Parse() on Normal Input.
        /// </summary>
        [Test]
        public void TestParse()
        {
            string expression = "3 + X * 5";
            string[] expected = { "3", "X", "+", "*", "5" };
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
            string[] expected = { "3", "+", "X", "*", "5" };
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
            string[] expected = { "3", "+", "X", "*", "5" };
            string[] actual = EvaluationTree.Parse(expression);
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}