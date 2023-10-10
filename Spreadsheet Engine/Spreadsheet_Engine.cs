using System.ComponentModel;
using System.Linq.Expressions;
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


    public abstract class Node
    {

    }



    public class EvaluationTree
    {

        private Dictionary<string, double> variables;
        Node root;


        /// <summary>
        /// Constructor that builds the tree based off input expression.
        /// </summary>
        /// <param name="expression"></param>
        public EvaluationTree(string expression)
        {
            variables = new Dictionary<string, double>();// initialize variables dictionary

            Stack<Node> stack = new Stack<Node>();// Using a stack to store the order of the nodes in the tree (first node is at bottom of tree)
            string[] expressions = Parse(expression);
            
            foreach (string exp in expressions) 
            {
                if(exp.Length == 1)// operator or digit
                {
                    if (char.IsDigit(exp[0]))
                    {
                        stack.Push(new NumericNode(double.Parse(exp)));
                    }
                    else
                    {
                        OperatorNode op = new OperatorNode(exp);
                        op.Right = stack.Pop();
                        op.Left = stack.Pop(); // based off the format of the parsing (3, 3, +), the two preceeding nodes on the stack should
                                                // contain 3, 3. Thus we make +'s children 3.
                        stack.Push(op); // if we have something like 3 + 3 * 8, makes 3 + 3 child of *
                    }
                }
                else// variable
                {
                    stack.Push(new VariableNode(exp));
                }
            }

            if(stack.Count == 1)
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
        static public string[] Parse(string expression)
        {
            List<string> tokens = new List<string>();
            string token = "";
            for(int i = 0; i < expression.Length;  i++) 
            {
                if (expression[i] != ' ')
                {
                    if (char.IsDigit(expression[i]))
                    {
                        tokens.Add(expression[i].ToString());
                    }
                    else if (expression[i] == '+' || expression[i] == '-' || expression[i] == '*' || expression[i] == '/')
                    {
                        tokens.Add(expression[i].ToString());
                    }
                    else if(char.IsLetterOrDigit(expression[i]))// variable name
                    {
                        while(i < expression.Length && char.IsLetterOrDigit(expression[i]))
                        {
                            token += expression[i];
                            i++;
                        }
                        --i; //don't skip character
                        tokens.Add(token);
                        token = "";
                    }
                    else
                    {
                        throw new InvalidOperationException("Invalid Operator.");
                    }

                }
            
            }

            // we have everything parsed, now need to put it in order (operand1), (operand2), operator

            for(int i = 2; i < tokens.Count; i += 2) // need to turn 3 + 3 into 3 3 +
            {
                string temp = tokens[i];
                tokens[i] = tokens[i - 1];
                tokens[i - 1] = temp;
            }
            return tokens.ToArray();
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
        private double Evaluate(Node node)
        {
            if(node == null)
            {
                return 0;
            }

            if(node is OperatorNode)
            {
               OperatorNode n = (OperatorNode)node;
               double left = this.Evaluate(n.Left);
               double right = this.Evaluate(n.Right);

                switch(n.operationType) 
                {
                    case "+": return left + right;
                        
                    case "-": return left - right;
                        
                    case "*": return left * right;
                        
                    case "/":
                        if (right == 0)
                        {
                            throw new DivideByZeroException("Error: Dividing By Zero.");
                        }
                        return left / right;
                    default: throw new InvalidOperationException("Invalid Operator.");
                }

            }
            else if(node is NumericNode)
            {
                NumericNode n = (NumericNode)node;
                return n.Constant;
            }
            else if(node is VariableNode)
            {
                VariableNode n = (VariableNode)node;
                if(variables.TryGetValue(n.VarName, out var value))// if value exists
                {
                    return value;
                }
                else
                {
                    throw new Exception("Variable Does Not Exist.");
                }
            }
            return 0.0;
        }





        private class NumericNode : Node
        {
            double constant;
            public NumericNode(double nConstant)
            {
                constant = nConstant;
            }


            public double Constant
            {
                get { return constant; }
                set { constant = value; }
            }

        }

        private class VariableNode : Node
        {
            string varName;

            public VariableNode(string newVarName)
            {
                this.varName = newVarName;
            }

            public string VarName
            {
                get { return varName; }
            }
        }

        private class OperatorNode : Node
        {
            public string operationType;
            Node? left, right;

            public OperatorNode(string newOperationType)
            {
                operationType = newOperationType;
            }

            public Node? Left
            {
                get { return left; }
                set { left = value; }
            }

            public Node? Right
            {
                get { return right; }
                set { right = value; }
            }
        }

    }
}