using Spreadsheet_Engine.OperatorNodeStuff;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spreadsheet_Engine
{
    public class EvaluationTree
    {


            private Dictionary<string, Spreadsheet.SpreadsheetCell> variables;
            Node? root;
            private readonly OperatorNodeFactory factory;
            private readonly GetCellDelegate? GetCellDelegate;
            public event PropertyChangedEventHandler VariableChanged = delegate { };
            Spreadsheet.SpreadsheetCell parentCell;


            /// <summary>
            /// Constructor that builds the tree based off input expression.
            /// </summary>
            /// <param name="expression"></param>
            public EvaluationTree(GetCellDelegate del, Spreadsheet.SpreadsheetCell parent, string expression = "0 + 0")
            {
                variables = new Dictionary<string, Spreadsheet.SpreadsheetCell>();// initialize variables dictionary
                factory = new OperatorNodeFactory(); //initialize factory
                GetCellDelegate = del; // pass the delegate through the spreadsheet cell so it can initialize tree with it
                parentCell = parent;
                Compile(expression);
            }

            /// <summary>
            /// Function that subscribes the cells held in the variable list to each Cell's value changed event.
            /// </summary>
            private void SubscribeToReferencedCells()
            {
                foreach (KeyValuePair<string, Spreadsheet.SpreadsheetCell> variable in variables)
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
                if (sender is Spreadsheet.SpreadsheetCell cell)
                {
                    foreach (KeyValuePair<string, Spreadsheet.SpreadsheetCell> variable in variables) // go through variables to find match
                    {
                        if (cell == variable.Value)
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
                bool throwException = true;

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

                                    if (varCell != null)
                                    {
                                        if (varCell != parentCell)
                                        {
                                            if (varCell.Value == null || varCell.Value == "!(bad reference)")
                                            {
                                                parentCell.Text = "!(bad reference)";
                                                parentCell.SendNotification();
                                            }
                                            variables[exp] = varCell;
                                            stack.Push(new VariableNode(exp, variables));
                                        }
                                        else
                                        {
                                            parentCell.Text = "!(self reference)";
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        parentCell.Text = "!(bad reference)";
                                        throwException = false;
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
                else if(throwException)
                {
                    parentCell.Text = "!(bad reference)";
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
                for (int i = 0; i < expression.Length; i++)// parenthesis counting to ensure there are matching open and close parenthesis
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
                if (left_parenthesis != right_parenthesis)
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
                        while (operators.Count > 0 && operators.Peek()[0] != '(')
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
                        while (operators.Count > 0 && operators.Peek()[0] != '(' && factory.Precedence(token) <= factory.Precedence(operators.Peek()))
                        {
                            output.Enqueue(operators.Pop());
                        }
                        operators.Push(token);
                    }
                }


                while (operators.Count > 0)
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
                    return -1;
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
                if (node != null)
                {
                    return node.Evaluate();
                }
                else { return -1; }
            }
        }
    }
