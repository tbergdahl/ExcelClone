using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spreadsheet_Engine
{

    /// <summary>
    /// class for addition nodes.
    /// </summary>
    internal class AdditionOperatorNode: OperatorNodeStuff.OperatorNode
    {
        public AdditionOperatorNode() 
        {
            Precedence = 1;
            Assoc = Associativity.LEFT;
            OperationType = "+";
        }


        /// <summary>
        /// override of abstract Evaluate() for addition.
        /// </summary>
        /// <returns></returns>
        public override double Evaluate()
        {
            if (Left != null && Right != null)
            {
                return Left.Evaluate() + Right.Evaluate();
            }
            else
            {
                throw new Exception("Invalid Tree.");
            }
        }

    }
}
