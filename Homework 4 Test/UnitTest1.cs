using Spreadsheet_Engine;

namespace Homework_4_Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]


        /// <summary>
        /// Tests Spreadsheet Constructor with an invalid row number.
        /// </summary>
        public void TestInvalidRow()
        {
            Assert.Throws<ArgumentException>(() => new Spreadsheet_Engine.Spreadsheet(-3, 3));
        }


        [Test]


        /// <summary>
        /// Tests Spreadsheet Constructor with an invalid column number.
        /// </summary>
        public void TestInvalidColumn()
        {
            Assert.Throws<ArgumentException>(() => new Spreadsheet_Engine.Spreadsheet(3, -3));
        }

        [Test]
        /// <summary>
        /// Tests Spreadsheet to ensure a cell is the proper type
        /// </summary>
        public void TestType()
        {
            Spreadsheet_Engine.Spreadsheet spreadsheet = new Spreadsheet_Engine.Spreadsheet(2, 2);
            object? test = spreadsheet.GetCell(1, 1);
            Assert.That(test, Is.InstanceOf<Spreadsheet_Engine.Spreadsheet.SpreadsheetCell>());
        }

        [Test]
        /// <summary>
        /// Tests Spreadsheet.getCell() to ensure that it returns null if attempting to access index that isnt in cells
        /// </summary>
        public void Test_GetCell()
        {
            Spreadsheet_Engine.Spreadsheet spreadsheet = new Spreadsheet_Engine.Spreadsheet(3, 3);
            Assert.That(spreadsheet.GetCell(4, 1), Is.Null);
        }

    }
}