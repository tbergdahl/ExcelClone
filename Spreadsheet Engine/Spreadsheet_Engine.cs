using System.ComponentModel;
using static System.Net.Mime.MediaTypeNames;

namespace Spreadsheet_Engine
{


        public abstract class Cell : INotifyPropertyChanged
        {
            private int rowIndex, colIndex;
            protected string? text, evaluated;
            public event PropertyChangedEventHandler? PropertyChanged = delegate { };

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
            public string? Text
            {
                get => text;
                set
                {

                    if (text == value)
                    {
                        return;
                    }
                    text = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Text));
                    }
                }
            }
            public string? Value
            {
                get { return evaluated; }
            }
        }


    public class Spreadsheet
    {
        /// <summary>
        /// private data members to specify the max number of rows and columns in the spreadsheet instance.
        /// </summary>
        private int numRows, numCols;

        /// <summary>
        /// cells contains all of the SpreadSheet cells.
        /// </summary>
        private SpreadsheetCell[,] cells;


        public event PropertyChangedEventHandler CellPropertyChanged = delegate { };

        public int ColumnCount
        {
            get { return numCols; }
        }

        public int RowCount
        {
            get { return numRows; }
        }


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

            public new string? Value
            {
                get { return evaluated; }
                set { evaluated = value; }
            }
        }     
        

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
                        cells[row, col] = new SpreadsheetCell(row, col); //initialize cells
                        cells[row, col].PropertyChanged += Cell_PropertyChanged; //subscribe to cells' property changed event
                    }
                }
            }
            else
            {
                throw new ArgumentException("Please Provide a Value Greater Than 0.\n");
            }
        }

        
        /// <summary>
        /// Broadcasts that a cell has changed, and sends the coordinates of the triggering cell as a string.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cell_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is SpreadsheetCell cell)
            {
                if (cell.Text != cell.Value)// there is a change in text
                {
                    
                    if (cell.Text?.StartsWith('=') == true)//we have a formula
                    {
                        if(cell.Text?.Length > 2 && int.TryParse(cell.Text.AsSpan(1), out int rowIndex))//verify proper index format held after '='
                        {
                            int colIndex = cell.Text[1] - 'A';
                            var target = this.GetCell(rowIndex, colIndex);
                            if(target != null)
                            {
                                cell.Value = target.Value;
                                cell.Text = cell.Value;
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                    else
                    {
                        cell.Value = cell.Text;
                    }
                }
                string str = cell.RowIndex.ToString() + " " + cell.ColIndex.ToString();
                CellPropertyChanged?.Invoke(this, new PropertyChangedEventArgs(str));
            }
            return;
        }


        /// <summary>
        /// Returns the cell at the inputted index.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns> cell </returns>
        public SpreadsheetCell? GetCell(int row, int col)
        {
            if (row < numRows && col < numCols && row >= 0 && col >= 0)
            {
                return cells[row, col];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Performs HW4 demo.
        /// </summary>
        public void Demo()
        {
            //cells[50, 26].Text = "YUP";


            Random num = new Random();
            int row, col;
            string text = "yup";
            for (int i = 0; i < 50; i++)// generate 50 random indeces
            {
                row = num.Next(1, 51);
                col = num.Next(1, 27);
                cells[row, col].Text = text;
            }

            for(int i = 1; i < 51; i++)
            {
                cells[i, 2].Text = "This is cell B" + i + ".";
                cells[i, 1].Text = cells[i, 2].Text; 
            }
        }
    }
}