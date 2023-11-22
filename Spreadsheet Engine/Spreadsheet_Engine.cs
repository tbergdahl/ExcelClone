using System.ComponentModel;
using System.Data;
using System.Linq.Expressions;
using static System.Net.Mime.MediaTypeNames;



namespace Spreadsheet_Engine
{
    using OperatorNodeStuff;
    using System.Threading.Tasks.Sources;
    using System.Xml;


    /// <summary>
    /// Cell class that abstracts a cell.
    /// </summary>
    public abstract class Cell : INotifyPropertyChanged
        {
            private int rowIndex, colIndex;
            protected string? text, evaluated;
            public event PropertyChangedEventHandler? PropertyChanged = delegate { };
            public event PropertyChangedEventHandler? BackgroundColorChanged = delegate { };
            private uint background_color;

            protected Cell(int rindex, int cindex)
            {
                rowIndex = rindex;
                colIndex = cindex;
                background_color = 0xFFFFFFFF;
            }
            public int ColIndex
            {
                get { return colIndex; }
            }
            public int RowIndex
            {
                get { return rowIndex; }
            }

            public uint BGColor
            {
                get { return background_color; }
                set 
                { 
                    if (background_color == value) { return; }
                    background_color = value;
                    BackgroundColorChanged?.Invoke(this, new PropertyChangedEventArgs(BGColor.ToString()));
            }
            }

            public void SendNotification()
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Text));
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
                        SendNotification();
                    }
                }
            }
            public string? Value
            {
                get { return evaluated; }
                set { }
            }
        }

    public delegate Spreadsheet.SpreadsheetCell? GetCellDelegate(int row, int col);

    public class Spreadsheet
    {
        private int numRows, numCols;
        public GetCellDelegate? GetCell;
        private SpreadsheetCell[,] cells;
        public event PropertyChangedEventHandler CellBackgroundColorChanged = delegate { };
        public event PropertyChangedEventHandler CellPropertyChanged = delegate { };
        readonly private Stack<UndoRedoCommand> undoStack;
        readonly private Stack<UndoRedoCommand> redoStack;

        public int ColumnCount
        {
            get { return numCols; }
        }

        public int RowCount
        {
            get { return numRows; }
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
            undoStack = new Stack<UndoRedoCommand>();
            redoStack = new Stack<UndoRedoCommand>();
            GetCell = GetCellAtPos;
            if (nRows > 0 && nCols > 0)
            {
                cells = new SpreadsheetCell[nRows, nCols];
                for (int row = 0; row < nRows; row++)
                {
                    for (int col = 0; col < nCols; col++)
                    {
                        cells[row, col] = new SpreadsheetCell(row, col); //initialize cells
                        cells[row, col].PropertyChanged += Cell_PropertyChanged; //subscribe to cells' property changed event
                        cells[row, col].BackgroundColorChanged += Cell_BackgroundColorChanged;
                    }
                }
            }
            else
            {
                throw new ArgumentException("Please Provide a Value Greater Than 0.\n");
            }
        }

        /// <summary>
        /// Function to determine if a cell has been changed or not
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private static bool IsNotDefault(SpreadsheetCell cell)
        {
            return cell.Text != null || cell.Value != null || cell.BGColor != 0xFFFFFFFF;
        }


        /// <summary>
        /// saves the spreadsheet in XML format.
        /// </summary>
        /// <param name="filename"></param>
        public void Save(string filename)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("Spreadsheet");
            

            for(int row = 0; row < numRows; row++)
            {
                for(int col = 0; col < numCols; col++)
                {
                    SpreadsheetCell cell = cells[row, col];
                    //create the xml entry for a cell only if necessary
                    if (IsNotDefault(cell))
                    {
                        XmlElement cellElem = doc.CreateElement("Cell");

                        XmlElement rowElem = doc.CreateElement("RowIndex");
                        rowElem.InnerText = cell.RowIndex.ToString();
                        cellElem.AppendChild(rowElem);

                        XmlElement colElem = doc.CreateElement("ColIndex");
                        colElem.InnerText = cell.ColIndex.ToString();
                        cellElem.AppendChild(colElem);

                        XmlElement textElem = doc.CreateElement("Text");
                        textElem.InnerText = cell.Text ?? "";
                        cellElem.AppendChild(textElem);

                        XmlElement valueElem = doc.CreateElement("Value");
                        valueElem.InnerText = cell.Value ?? "";
                        cellElem.AppendChild(valueElem);

                        XmlElement colorElem = doc.CreateElement("BGColor");
                        colorElem.InnerText = cell.BGColor.ToString();
                        cellElem.AppendChild(colorElem);

                        root.AppendChild(cellElem);
                    }
                }
            }
            doc.AppendChild(root);
            doc.Save(filename);
        }

        /// <summary>
        /// function to load a spreadsheet in XML format
        /// </summary>
        /// <param name="filename"></param>
        public void Load(string filename)
        {
            XmlDocument xmlDocument = new XmlDocument();
            try
            {
                xmlDocument.Load(filename);
                if (xmlDocument != null)
                {
                    XmlNodeList? cellNodes = xmlDocument.SelectNodes("//Cell");
                    if (cellNodes != null)
                    {
                        foreach (XmlNode cellNode in cellNodes)
                        {
                            if (cellNode != null)
                            {
                                int rowIndex = int.Parse(cellNode.SelectSingleNode("RowIndex").InnerText);
                                int colIndex = int.Parse(cellNode.SelectSingleNode("ColIndex").InnerText);
                                string? value = cellNode.SelectSingleNode("Value").InnerText;
                                uint bgColor = uint.Parse(cellNode.SelectSingleNode("BGColor").InnerText);
                                cells[rowIndex, colIndex].Value = value;
                                cells[rowIndex, colIndex].BGColor = bgColor;
                            }
                        }

                        foreach (XmlNode cellNode in cellNodes)// need to change text (formula) after giving every cell a value so that way 
                                                               //the program doesn't incorrectly throw a no value error
                        {
                            int rowIndex = int.Parse(cellNode.SelectSingleNode("RowIndex").InnerText);
                            int colIndex = int.Parse(cellNode.SelectSingleNode("ColIndex").InnerText);
                            string? text = cellNode.SelectSingleNode("Text").InnerText;
                            cells[rowIndex, colIndex].Text = text;
                        }
                    }
                }
            }
            catch
            {
                throw new DataException("Cannot Load XML File.");
            }
        }


        /// <summary>
        /// Broadcasts that a celll's background color has changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cell_BackgroundColorChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is SpreadsheetCell cell)
            {
                string str = cell.RowIndex.ToString() + " " + cell.ColIndex.ToString();
                CellBackgroundColorChanged?.Invoke(this, new PropertyChangedEventArgs(str));
            }
            return;
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
                if (cell.Text?.StartsWith('=') == true)//we have a formula
                {
                    if (GetCell != null)
                    {
                        cell.BuildNewTree(cell.Text, GetCell);
                    }
                }
                else
                {
                    cell.Value = cell.Text;
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
        public SpreadsheetCell? GetCellAtPos(int row, int col)
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
        /// adds a command to the undo stack.
        /// </summary>
        /// <param name="command"></param>
        public void AddCommand(UndoRedoCommand command)
        {
            undoStack.Push(command);
        }

        /// <summary>
        /// performs undo operation and moves to redo stack.
        /// </summary>
        public void Undo()
        {
            undoStack.Peek().Undo();
            redoStack.Push(undoStack.Pop());
        }

        /// <summary>
        /// performs undo operation and moves to undo stack.
        /// </summary>
        public void Redo()
        {
            redoStack.Peek().Execute();
            undoStack.Push(redoStack.Pop());
        }

        public bool UndoStackEmpty()
        {
            return undoStack.Count == 0;
        }

        public bool RedoStackEmpty()
        {
            return redoStack.Count == 0;
        }

        public string GetNextUndoCommandName()
        {
            if (undoStack.Count != 0)
            {
                return undoStack.Peek().GetDescription();
            }
            return string.Empty;
        }

        public string GetNextRedoCommandName()
        {
            if (redoStack.Count != 0)
            {
                return redoStack.Peek().GetDescription();
            }
            return string.Empty;
        }


        /// <summary>
        /// Class that Allows only Spreadsheet to make Cells.
        /// </summary>
        public class SpreadsheetCell : Cell
        {
            public EvaluationTree? tree;
            public event PropertyChangedEventHandler ValueChanged = delegate { };
            private bool evaluate = true, readyToCompile = true; // for undo/redo, and save/load

            /// <summary>
            /// Uses Cell constructor as SpreadsheetCell constructor
            /// </summary>
            /// <param name="rindex"></param>
            /// <param name="cindex"></param>
            public SpreadsheetCell(int rindex, int cindex) : base(rindex, cindex)
            {

            }

            /// <summary>
            /// Event handler that updates the Value of the cell and sends a notification to the WinForms spreadsheet.
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void TreeValueChanged(object? sender, PropertyChangedEventArgs e)
            {
                if (sender is EvaluationTree tree)
                {
                    if (evaluate)
                    {
                        this.Value = tree.Evaluate().ToString();
                        this.SendNotification();
                    }
                }
            }

            public void SetEvaluate(bool eval)
            {
                evaluate = eval;
            }

            public void SetReady(bool ready)
            {
                readyToCompile = ready;
            }

            public void SetText(string? newString)
            {
                Text = newString;
            }


            public void SetBGColor(uint newColor)
            {
                BGColor = newColor;
            }

            /// <summary>
            /// Builds a new tree with the given expression and subscribes to it's variable changed event
            /// </summary>
            /// <param name="expression"></param>
            /// <param name="func"></param>
            public void BuildNewTree(string expression, GetCellDelegate func)
            {
                if (readyToCompile)
                {
                    if(tree != null)
                    {
                        this.tree.VariableChanged -= this.TreeValueChanged;
                        this.tree = null;
                    }
                    this.tree = new EvaluationTree(func, this, expression.Substring(1));
                    this.tree.VariableChanged += this.TreeValueChanged; // subscribe the cell to it's tree's variable changed event so it can revaluate the tree

                    if (this.Value != "!(self reference)")
                    {
                        if (evaluate)
                        {
                            if (!this.tree.TreeHasCircularReference())
                            {
                                double val = this.tree.Evaluate();
                                if (val == -1)
                                {
                                    this.Value = "!(bad reference)";
                                }
                                else
                                {
                                    this.Value = val.ToString();
                                }
                            }
                            else
                            {
                                this.Value = "!(circular reference)";
                            }
                        }
                    }
                   
                }
            }


            public new string? Value
            {
                get => evaluated;
                set
                {

                    if (evaluated == value)
                    {
                        return;
                    }
                    evaluated = value;
                    ValueChanged.Invoke(this, new PropertyChangedEventArgs(Value));

                }
            }
        }





    }
}