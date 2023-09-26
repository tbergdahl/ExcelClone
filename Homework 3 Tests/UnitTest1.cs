using Homework_3;
using System.Numerics;

namespace Homework_3_Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {

        }



        /// <summary>
        /// Tests Constructor to ensure that it throws exception when inputting invalid number of lines
        /// </summary>
        [Test]
        public void FibTestInvalid()
        {
           
            Assert.Throws<ArgumentException>(() =>
            {
                FibonacciTextReader test = new FibonacciTextReader(-10);
            });
        }

        /// <summary>
        /// Tests ReadLine() overload to make sure it reads all 30 first fibonacci numbers, but doesnt go any further
        /// </summary>
        [Test]
        public void FibTestNull()
        {
            FibonacciTextReader test = new FibonacciTextReader(30);
            string? result = "";
            int count = 0;
            while (result != null)
            {
                count++;
                result = test.ReadLine();
            }

            Assert.That(count, Is.EqualTo(30));
        }

        /// <summary>
        /// Tests ReadLine() overload to make sure it returns the correct number in the sequence (in this case, the 30th)
        /// </summary>
        [Test]
        public void FibTestNormal()
        {
            FibonacciTextReader test = new FibonacciTextReader(31);
            string ?result = "";
            int count = 0;
            while (count++ < 31)
            {
                result = test.ReadLine();
            }

            Assert.That(result, Is.EqualTo("832040"));
        }


    }
}