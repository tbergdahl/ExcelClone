// <copyright file="Tests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EvaluationTreeTest
{
    using NUnit.Framework.Internal;
    using Spreadsheet_Engine;

    /// <summary>
    /// Tests for EvaluationTree Class Methods.
    /// </summary>
    public class Tests
    {
        /// <summary>
        /// Preforms test setup.
        /// </summary>
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
        /// Tests evaluate on a more complicated input.
        /// </summary>
        [Test]
        public void TestEvaluate_ComplicatedInput()
        {
            EvaluationTree test = new EvaluationTree("9 / (2 + (3 * 7) / 4) + 9)");
            Assert.That(test.Evaluate(), Is.EqualTo(10.241379310344827d));
        }
    }
}