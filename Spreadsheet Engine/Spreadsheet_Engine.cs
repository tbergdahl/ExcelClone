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
                           
                if (cell.Text?.StartsWith('=') == true)//we have a formula
                {
                    EvaluationTree tree = new EvaluationTree(cell.Text.Substring(1));
                    List<string> variableNames = tree.GetVariableNames();

                    foreach(string name in variableNames)
                    {
                        int colIndex = name[0] - 'A';
                        if(int.TryParse(name.Substring(1), out int rowIndex))
                        {
                            var target = this.GetCell(rowIndex, colIndex + 1);
                            tree.SetVariable(name, double.Parse(target.Value));
                        }
                    }
                    cell.Value = tree.Evaluate().ToString();
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


            for(int i = 1; i < 51; i++)
            {
                cells[i, 2].Text = "This is cell B" + i + ".";
                cells[i, 1].Text = cells[i, 2].Text; 
            }
        }
    }


    



    public class EvaluationTree
    {

        private Dictionary<string, double> variables;
        Node? root;
        private OperatorNodeFactory factory;


        /// <summary>
        /// Constructor that builds the tree based off input expression.
        /// </summary>
        /// <param name="expression"></param>
        public EvaluationTree(string expression)
        {
            variables = new Dictionary<string, double>();// initialize variables dictionary
            factory = new OperatorNodeFactory(); //initialize factory
            Compile(expression);      
        }

       


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
                else if (exp[0] == '+' || exp[0] == '-' || exp[0] == '/' || exp[0] == '*')
                {
                    OperatorNode? op = factory.Create(exp);
                    if (op != null)
                    {
                        op.Right = stack.Pop();
                        op.Left = stack.Pop(); // based off the format of the parsing (3, 3, +), the two preceeding nodes on the stack should
                                               // contain 3, 3. Thus we make +'s children 3.
                        stack.Push(op); // if we have something like 3 + 3 * 8, makes 3 + 3 child of *
                    }
                }
                else // variable
                {
                    stack.Push(new VariableNode(exp, variables));
                    variables[exp] = 0;
                }
            }

            if (stack.Count == 1)
            {
                root = stack.Pop();
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
        public void SetVariable(string variableName, double variableValue)
        {
            variables[variableName] = variableValue;
        }

        /// <summary>
        /// Evaluates expression stored in tree.
        /// </summary>
        /// <returns></returns>

        public double Evaluate()
        {
           return Evaluate(root);
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