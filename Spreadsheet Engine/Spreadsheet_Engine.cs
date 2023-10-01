using System.ComponentModel;

namespace Spreadsheet_Engine
{


        public abstract class Cell : INotifyPropertyChanged
        {
            private int rowIndex, colIndex;
            private string? text, evaluated;
            public event PropertyChangedEventHandler PropertyChanged = delegate { };

            protected Cell(int rindex, int cindex)
            {
                rowIndex = rindex;
                colIndex = cindex;
            }
            public int ColIndex
            {
                get { return colIndex; }
            }
            public int RowIndex
            {
                get { return rowIndex; }
            }
            public string Text
            {
                get { return text; }
                set
                {

                    if (text == value)
                    {
                        return;
                    }
                    text = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(Text));
                }
            }
            protected string Value
            {
                get { return evaluated; }
            }
        }
    

    public class Spreadsheet
    {

        /// <summary>
        /// Class that Allows only Spreadsheet to make Cells.
        /// </summary>
        public class SpreadsheetCell : Cell
        {
            /// <summary>
            /// Uses Cell constructor as SpreadsheetCell constructor
            /// </summary>
            /// <param name="rindex"></param>
            /// <param name="cindex"></param>
            public SpreadsheetCell(int rindex, int cindex) : base(rindex, cindex)
            {

            }
        }     
        private int numRows, numCols;

        /// <summary>
        /// cells contains all of the SpreadSheet cells.
        /// </summary>
        private SpreadsheetCell[,] cells;

        /// <summary>
        /// Spreadsheet constructor - Initializes cells array with new SpreadsheetCells
        /// </summary>
        /// <param name="nRows"></param>
        /// <param name="nCols"></param>
        public Spreadsheet(int nRows, int nCols)
        {
            numRows = nRows;
            numCols = nCols;
            if (nRows > 0 && nCols > 0)
            {
                cells = new SpreadsheetCell[nRows, nCols];
                for (int row = 0; row < nRows; row++)
                {
                    for (int col = 0; col < nCols; col++)
                    {
                        cells[row, col] = new SpreadsheetCell(row, col);
                    }
                }
            }
            else
            {
                throw new ArgumentException("Please Provide a Value Greater Than 0.\n");
            }
        }

        public SpreadsheetCell GetCell(int row, int col)
        {
            if (row < numRows && col < numCols && row >= 0 && col >= 0)
            {
                return cells[row, col];
            }
            else
            {
                throw new ArgumentOutOfRangeException("Invalid Index Input.\n");
            }
        }

    }





}