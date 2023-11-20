using Spreadsheet_Engine;
using System.Data;
namespace Spreadsheet_Engine
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
            object? test = spreadsheet.GetCellAtPos(1, 1);
            Assert.That(test, Is.InstanceOf<Spreadsheet_Engine.Spreadsheet.SpreadsheetCell>());
        }

        [Test]
        /// <summary>
        /// Tests Spreadsheet.getCell() to ensure that it returns null if attempting to access index that isnt in cells
        /// </summary>
        public void Test_GetCell()
        {
            Spreadsheet_Engine.Spreadsheet spreadsheet = new Spreadsheet_Engine.Spreadsheet(3, 3);
            Assert.That(spreadsheet.GetCellAtPos(4, 1), Is.Null);
        }

        /// <summary>
        /// Tests a cell building a tree with an invalid operator.
        /// </summary>
        [Test]
        public void Test_InvalidOperator()
        {
            Spreadsheet_Engine.Spreadsheet.SpreadsheetCell cell = new Spreadsheet_Engine.Spreadsheet.SpreadsheetCell(1, 1);
            Spreadsheet_Engine.Spreadsheet sheet = new Spreadsheet(28, 51);
            GetCellDelegate del = sheet.GetCellAtPos;
            Assert.Throws<InvalidOperationException>(() => cell.BuildNewTree("=9 ^ 6", del));
        }

        /// <summary>
        /// Tests EvaluationTree.Evaluate() on normal input.
        /// </summary>
        [Test]
        public void TestEvaluate()
        {
            Spreadsheet_Engine.Spreadsheet.SpreadsheetCell cell = new Spreadsheet_Engine.Spreadsheet.SpreadsheetCell(1, 1);
            Spreadsheet_Engine.Spreadsheet sheet = new Spreadsheet(28, 51);
            GetCellDelegate del = sheet.GetCellAtPos;
            cell.BuildNewTree("=(9 + 6) * 5", del);
            if (cell.tree != null)
            {
                Assert.That(cell.tree.Evaluate(), Is.EqualTo(75));
            }
        }

        /// <summary>
        /// Tests edge case - divide by zero.
        /// </summary>
        [Test]
        public void Test_DivideByZero()
        {
            Spreadsheet_Engine.Spreadsheet.SpreadsheetCell cell = new Spreadsheet_Engine.Spreadsheet.SpreadsheetCell(1, 1);
            Spreadsheet_Engine.Spreadsheet sheet = new Spreadsheet(28, 51);
            GetCellDelegate del = sheet.GetCellAtPos;
            Assert.Throws<DivideByZeroException>(() => cell.BuildNewTree("=9 / 0", del));
        }

        /// <summary>
        /// Tests evaluate on a more complicated input.
        /// </summary>
        [Test]
        public void TestEvaluate_ComplicatedInput()
        {
            Spreadsheet_Engine.Spreadsheet.SpreadsheetCell cell = new Spreadsheet_Engine.Spreadsheet.SpreadsheetCell(1, 1);
            Spreadsheet_Engine.Spreadsheet sheet = new Spreadsheet(28, 51);
            GetCellDelegate del = sheet.GetCellAtPos;
            cell.BuildNewTree("=((((9 + 8) * 6 / (8 * 7) - 2)+ 6) * 5) / 3", del);
            if (cell.tree != null)
            {
                Assert.That(cell.tree.Evaluate(), Is.EqualTo(9.7023809523809508));
            }
        }


        /// <summary>
        /// Tests inputs when there aren't matching parenthesis.
        /// </summary>
        [Test]

        public void Test_Parenthesis()
        {
            Spreadsheet_Engine.Spreadsheet.SpreadsheetCell cell = new Spreadsheet_Engine.Spreadsheet.SpreadsheetCell(1, 1);
            Spreadsheet_Engine.Spreadsheet sheet = new Spreadsheet(28, 51);
            GetCellDelegate del = sheet.GetCellAtPos;
            Assert.Throws<InvalidExpressionException>(() => cell.BuildNewTree("=(9 + 6", del));
            Assert.Throws<InvalidExpressionException>(() => cell.BuildNewTree("=9 + 6)", del));
            Assert.Throws<InvalidExpressionException>(() => cell.BuildNewTree("=(9 + 6))", del));
            Assert.Throws<InvalidExpressionException>(() => cell.BuildNewTree("=(9 + 6) + (5 * 3", del));
        }


        /// <summary>
        /// Tests invlaid operators in expression.
        /// </summary>
        [Test]

        public void Test_Incorrect_Operators()
        {
            Spreadsheet_Engine.Spreadsheet.SpreadsheetCell cell = new Spreadsheet_Engine.Spreadsheet.SpreadsheetCell(1, 1);
            Spreadsheet_Engine.Spreadsheet sheet = new Spreadsheet(28, 51);
            GetCellDelegate del = sheet.GetCellAtPos;
            Assert.Throws<InvalidExpressionException>(() => cell.BuildNewTree("=9 + + 6", del));
        }


        //Homework 7 Tests

        /// <summary>
        /// Tests cell class to accurately update its value based on other cells.
        /// </summary>
        [Test]
        public void Test_ReferencedCells()
        {
            Spreadsheet sheet = new Spreadsheet(51, 27);
            Spreadsheet.SpreadsheetCell cell1 = sheet.GetCell(1, 1);
            Spreadsheet.SpreadsheetCell cell2 = sheet.GetCell(2, 2);
            cell1.Text = "=3 + 89";
            cell2.Text = "=A1 + 5";
            Assert.That(cell2.Value, Is.EqualTo("97"));
            cell1.Text = "=1 + 89";
            Assert.That(cell2.Value, Is.EqualTo("95"));
        }


        /// <summary>
        /// Tests exception handling for cells that reference cells that are out of bounds.
        /// </summary>
        [Test]

        public void Test_ReferencedCells_OutOfBounds()
        {
            Spreadsheet sheet = new Spreadsheet(51, 27);
            Spreadsheet.SpreadsheetCell cell1 = sheet.GetCell(1, 1);
            Spreadsheet.SpreadsheetCell cell2 = sheet.GetCell(2, 2);
            cell1.Text = "=3 + 89";
            Assert.Throws<InvalidExpressionException>(() => cell2.Text = "=A51 + 5");
        }


        /// <summary>
        /// Tests exception handling for when a cell references a cell that doesn't have a value.
        /// </summary>
        [Test]

        public void Test_ReferencedCells_NoCellValue()
        {
            Spreadsheet sheet = new Spreadsheet(51, 27);
            Spreadsheet.SpreadsheetCell cell1 = sheet.GetCell(1, 1);
            Spreadsheet.SpreadsheetCell cell2 = sheet.GetCell(2, 2);
            cell1.Text = "=3 + 89";
            Assert.Throws<InvalidExpressionException>(() => cell2.Text = "=B3 + 5");
        }




        //Homework 8 Tests

        /// <summary>
        /// Tests the text change command undo and redo functionality
        /// </summary>
        [Test]

        public void Test_TextChangeCommand() 
        {
            Spreadsheet.SpreadsheetCell cell = new Spreadsheet.SpreadsheetCell(1, 1);
            ChangeCellTextCommand command = new ChangeCellTextCommand(cell, "change");
            command.Execute();
            Assert.That(cell.Text, Is.EqualTo("change"));
            command.Undo();
            Assert.That(cell.Text, Is.EqualTo(null));
        }

        /// <summary>
        /// Tests the background color change command undo and redo functionality
        /// </summary>
        [Test]

        public void Test_BackgroundColorChangeCommand()
        {
            Spreadsheet.SpreadsheetCell cell = new Spreadsheet.SpreadsheetCell(1, 1);
            ChangeCellBackgroundColorCommand command = new ChangeCellBackgroundColorCommand(0x5A8D2F);
            command.AddChangedCell(cell);
            command.Execute();
            Assert.That(cell.BGColor, Is.EqualTo(0x5A8D2F));
            command.Undo();
            Assert.That(cell.BGColor, Is.EqualTo(4294967295));
        }




    }
}