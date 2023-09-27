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
    

    abstract class Spreadsheet
    {
        private int numRows, numCols;
        private Cell[,] cells;
        public Spreadsheet(int nRows, int nCols)
        {
           

            cells = new Cell[nRows, nCols];
            for(int row = 0; row < nRows; row++)
            {
                for(int col = 0; col < nCols; col++)
                {
                    cells[row, col] = new Cell();
                }
            }
        }



    }





}