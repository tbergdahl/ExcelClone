using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spreadsheet_Engine
{
    /// <summary>
    /// Class for division operator node.
    /// </summary>
    internal class DivisionOperatorNode : OperatorNodeStuff.OperatorNode
    {
        public DivisionOperatorNode()
        {
            Precedence = 2;
            Assoc = Associativity.LEFT;
            OperationType = "/";
        }


        /// <summary>
        /// Override of Evaluate() to preform division.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="DivideByZeroException"></exception>
        public override double Evaluate()
        {
            if (Left != null && Right != null)
            {
                double left = Left.Evaluate();
                double right = Right.Evaluate();

                if (right != 0.0)
                {
                    return left / right;
                }
                else
                {
                    throw new DivideByZeroException("Cannot Divide By Zero.");
                }
            }
            else
            {
                throw new Exception("Invalid Tree.");
            }
        }
    }
}
