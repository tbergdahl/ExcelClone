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
        public string operationType;
        protected int precedence;
        public enum Associativity { LEFT = 0, RIGHT = 1 };
        public Associativity associativity;
        Node? left, right;

            public OperatorNode(string newOperationType, Associativity assoc)
            {
                operationType = newOperationType;
                associativity = assoc;
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
