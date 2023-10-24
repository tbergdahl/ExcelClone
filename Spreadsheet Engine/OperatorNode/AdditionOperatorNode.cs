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
        public AdditionOperatorNode(string s) : base(s, Associativity.LEFT) 
        {
            precedence = 0;
            associativity = 0;
        }


        /// <summary>
        /// override of abstract Evaluate() for addition.
        /// </summary>
        /// <returns></returns>
        public override double Evaluate()
        {
            return Left.Evaluate() + Right.Evaluate();
        }

    }
}
