﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spreadsheet_Engine
{
    /// <summary>
    /// Class for a subtraction opeartor node.
    /// </summary>
    internal class SubtractionOperatorNode : OperatorNodeStuff.OperatorNode
    {
        
        public SubtractionOperatorNode()
        {
            Precedence = 1;
            Assoc = Associativity.LEFT;
            OperationType = "-";
        }

        /// <summary>
        /// Override for evaluate that preforms subtraction.
        /// </summary>
        /// <returns></returns>
        public override double Evaluate()
        {
            if (Left != null && Right != null)
            {
                return Left.Evaluate() - Right.Evaluate();
            }
            else
            {
                throw new Exception("Invalid Tree.");
            }
        }
    }
}
