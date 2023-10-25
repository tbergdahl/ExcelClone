using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spreadsheet_Engine
{
    /// <summary>
    /// Class for Multiplcation operator node.
    /// </summary>
    internal class MultiplicationOperatorNode : OperatorNodeStuff.OperatorNode
    {
        public MultiplicationOperatorNode(string s) : base(s, Associativity.LEFT) 
        {
            precedence = 1;
        }

        /// <summary>
        /// Override of Evaluate() that preforms multiplication.
        /// </summary>
        /// <returns></returns>
        public override double Evaluate()
        {
            if (Left != null && Right != null)
            {
                return Left.Evaluate() * Right.Evaluate();
            }
            else
            {
                throw new Exception("Invalid Tree.");
            }
        }
    }
}
