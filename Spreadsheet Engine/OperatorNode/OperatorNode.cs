using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spreadsheet_Engine.OperatorNodeStuff
{
    /// <summary>
    /// abstract class for an operator node.
    /// </summary>
    abstract class OperatorNode : Node
    {
        Node? left, right;
        Associativity associativity;
        int precedence;
        string? operationtype;

        public string? OperationType
        {
            get { return operationtype; } set { operationtype = value; }
        }
        public int Precedence
        {
            get { return precedence; } set { precedence = value; }
        }
        public enum Associativity { LEFT = 0, RIGHT = 1 };
        public Associativity Assoc
        {
            get { return associativity; } set { associativity = value; }
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

        public OperatorNode()
        {

        }      
    }    
}
