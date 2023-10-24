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
        public DivisionOperatorNode(string s) : base(s, Associativity.LEFT) 
        {
            precedence = 1;
        }


        /// <summary>
        /// Override of Evaluate() to preform division.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="DivideByZeroException"></exception>
        public override double Evaluate()
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
    }
}
