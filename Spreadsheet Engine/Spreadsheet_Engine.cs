using System.ComponentModel;
using System.Data;
using System.Linq.Expressions;
using static System.Net.Mime.MediaTypeNames;



namespace Spreadsheet_Engine
{
    using OperatorNodeStuff;


        /// <summary>
        /// Cell class that abstracts a cell.
        /// </summary>
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
        /// <summary>
        /// private data members to specify the max number of rows and columns in the spreadsheet instance.
        /// </summary>
        private int numRows, numCols;


       

        public GetCellDelegate? GetCell;

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


            public EvaluationTree? tree;
            public event PropertyChangedEventHandler ValueChanged = delegate { };

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
                if(sender is EvaluationTree tree)
                {
                    this.Value = tree.Evaluate().ToString();
                    this.SendNotification();
                }
            }

            /// <summary>
            /// Builds a new tree with the given expression and subscribes to it's variable changed event
            /// </summary>
            /// <param name="expression"></param>
            /// <param name="func"></param>
            public void BuildNewTree(string expression, GetCellDelegate func)
            {
                this.tree = new EvaluationTree(func, expression.Substring(1));
                this.tree.VariableChanged += this.TreeValueChanged; // subscribe the cell to it's tree's variable changed event so it can revaluate the tree
                this.Value = this.tree.Evaluate().ToString();
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
        

        /// <summary>
        /// Spreadsheet constructor - Initializes cells array with new SpreadsheetCells
        /// </summary>
        /// <param name="nRows"></param>
        /// <param name="nCols"></param>
        public Spreadsheet(int nRows, int nCols)
        {
            numRows = nRows;
            numCols = nCols;
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

      
    }


    



    public class EvaluationTree
    {
       
        
        private Dictionary<string, Spreadsheet.SpreadsheetCell> variables;
        Node? root;
        private readonly OperatorNodeFactory factory;
        private readonly GetCellDelegate? GetCellDelegate;
        public event PropertyChangedEventHandler VariableChanged = delegate { };


        /// <summary>
        /// Constructor that builds the tree based off input expression.
        /// </summary>
        /// <param name="expression"></param>
        public EvaluationTree(GetCellDelegate del, string expression = "0 + 0")
        {
            variables = new Dictionary<string, Spreadsheet.SpreadsheetCell>();// initialize variables dictionary
            factory = new OperatorNodeFactory(); //initialize factory
            GetCellDelegate = del; // pass the delegate through the spreadsheet cell so it can initialize tree with it
            Compile(expression);
            
        }

        /// <summary>
        /// Function that subscribes the cells held in the variable list to each Cell's value changed event.
        /// </summary>
        private void SubscribeToReferencedCells()
        {
            foreach(KeyValuePair<string, Spreadsheet.SpreadsheetCell> variable in variables)
            {
                variables[variable.Key].ValueChanged += Reevaluate; // subscribe to every referenced cell's value changed event
            }
        }

        /// <summary>
        /// When a cell variable sends an update that its value changed, notify the cell so it can tell the tree to recompile.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Reevaluate(object? sender, PropertyChangedEventArgs e)
        {
            if(sender is Spreadsheet.SpreadsheetCell cell)
            {
                foreach(KeyValuePair<string, Spreadsheet.SpreadsheetCell> variable in variables) // go through variables to find match
                {
                    if(cell == variable.Value)
                    {
                        VariableChanged.Invoke(this, new PropertyChangedEventArgs(variable.Key));// notify the cell containing this tree that a varible updated
                    }
                }
            }
        }


        /// <summary>
        /// Builds the expression tree.
        /// </summary>
        /// <param name="expression"></param>
        /// <exception cref="Exception"></exception>
        public void Compile(string expression)
        {
            Stack<Node> stack = new Stack<Node>();// Using a stack to store the order of the nodes in the tree (first node is at bottom of tree)
            string[] expressions = Parse(expression);

            foreach (string exp in expressions)
            {

                if (char.IsDigit(exp[0]))
                {
                    stack.Push(new NumericNode(double.Parse(exp)));
                }
                else if (factory.IsOperator(exp))
                {
                    OperatorNode? op = factory.Create(exp);
                    if (op != null)
                    {
                        if (stack.Count >= 2)
                        {
                            op.Right = stack.Pop();
                            op.Left = stack.Pop(); // based off the format of the parsing (3, 3, +), the two preceeding nodes on the stack should
                                                   // contain 3, 3. Thus we make +'s children 3.
                            stack.Push(op); // if we have something like 3 + 3 * 8, makes 3 + 3 child of *
                        }
                        else
                        {
                            throw new InvalidExpressionException("Invalid Expression");
                        }
                    }
                }
                else if (exp[0] == '(' || exp[0] == ')')
                {
                    throw new InvalidExpressionException("Invalid Expression.");
                }
                else // variable
                {
                    if (exp.Length >= 2)
                    {
                        string secondPart = exp.Length == 2 ? exp[1].ToString() : exp.Substring(1);// handle A1, as well as something like A13

                        if (int.TryParse(secondPart, out int row))
                        {
                            if (GetCellDelegate != null)
                            {
                                int column = exp[0] - 'A' + 1;
                                Spreadsheet.SpreadsheetCell? varCell = GetCellDelegate(row, column);

                                if (varCell != null && varCell.Value != null)
                                {
                                    variables[exp] = varCell;
                                    stack.Push(new VariableNode(exp, variables));
                                }
                                else
                                {
                                    string cellReference = (exp[0]).ToString() + row.ToString();
                                    throw varCell?.Value == null
                                        ? new InvalidExpressionException("Referenced Cell " + cellReference + " Does Not Have a Value to Reference.")
                                        : new ArgumentOutOfRangeException(cellReference);
                                }
                            }
                        }
                    }


                }
            }

            if (stack.Count == 1)
            {
                root = stack.Pop();
                SubscribeToReferencedCells();
            }
            else
            {
                throw new Exception("Improper Expression Input.");
            }
        }


        /// <summary>
        /// Splits expression into subexpressions to build nodes
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public string[] Parse(string expression)
        {
            int left_parenthesis = 0, right_parenthesis = 0;
            for(int i = 0; i < expression.Length; i++)// parenthesis counting to ensure there are matching open and close parenthesis
            {
                if (expression[i] == '(')
                {
                    left_parenthesis++;
                }
                else if (expression[i] == ')')
                {
                    right_parenthesis++;
                }
            }
            if(left_parenthesis != right_parenthesis)
            {
                throw new InvalidExpressionException("Matching Parenthesis Not Found.");
            }




            List<string> tokens = new List<string>();
            string token = "";
            if (expression != null)
            {
                for (int i = 0; i < expression.Length; i++)
                {
                    if (expression[i] != ' ')
                    {
                        if (char.IsDigit(expression[i]) || expression[i] == '.')
                        {
                            while (i < expression.Length && (char.IsDigit(expression[i]) || expression[i] == '.'))
                            {
                                token += expression[i];
                                i++;
                            }
                            --i; // don't skip character
                            tokens.Add(token);
                            token = "";
                        }
                        else if (expression[i] == '+' || expression[i] == '-' || expression[i] == '*' || expression[i] == '/')
                        {
                            tokens.Add(expression[i].ToString());
                        }
                        else if (char.IsLetterOrDigit(expression[i]))// variable name
                        {
                            while (i < expression.Length && char.IsLetterOrDigit(expression[i]))
                            {
                                token += expression[i];
                                i++;
                            }
                            --i; //don't skip character
                            tokens.Add(token);
                            token = "";
                        }
                        else if (expression[i] == '(' || expression[i] == ')')
                        {
                            tokens.Add(expression[i].ToString());
                        }
                        else
                        {
                            throw new InvalidOperationException("Invalid Operator.");
                        }

                    }

                }
            }
             return ToPostfix(tokens);
        }


        /// <summary>
        /// Uses Shunting Yard Algorithm to convert expression to postfix notation.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string[] ToPostfix(List<string> input)
        {
            Stack<string> operators = new Stack<string>();
            Queue<string> output = new Queue<string>();          

            foreach (string token in input)
            {
                if (char.IsDigit(token[0]) || char.IsLetter(token[0]))
                {
                    output.Enqueue(token);
                }
                else if (token[0] == '(')
                {
                    operators.Push(token);
                }
                else if (token[0] == ')')
                {
                    while(operators.Count > 0 && operators.Peek()[0] != '(')
                    {
                        output.Enqueue(operators.Pop());
                    }
                    if (operators.Count != 0)
                    {
                        operators.Pop();
                    }
                }
                else if (factory.IsOperator(token))
                {
                    while(operators.Count > 0 && operators.Peek()[0] != '(' && factory.Precedence(token) <= factory.Precedence(operators.Peek()))
                    {
                        output.Enqueue(operators.Pop());
                    }
                    operators.Push(token);
                }
            }


            while(operators.Count > 0)
            {
                output.Enqueue(operators.Pop());
            }

            return output.ToArray();
        }
        
        public List<string> GetVariableNames()
        {
            return variables.Keys.ToList();
        }

        /// <summary>
        /// Updates dictionary to store a variable.
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="variableValue"></param>
        public void SetVariable(string cellName, Spreadsheet.SpreadsheetCell cell)
        {
            variables[cellName] = cell;
        }

        /// <summary>
        /// Evaluates expression stored in tree.
        /// </summary>
        /// <returns></returns>

        public double Evaluate()
        {
            if (root != null)
            {
                return Evaluate(root);
            }
            else
            {
                throw new Exception("Root Is Null");
            }
        }

        /// <summary>
        /// Helper function for public Evaluate that recursively calculates expression inside tree.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        /// <exception cref="DivideByZeroException"></exception>
        /// <exception cref="Exception"></exception>
        private static double Evaluate(Node node)
        {
           if(node != null)
            {
                return node.Evaluate();
            }
            else { return -1; }
        }
    }
}