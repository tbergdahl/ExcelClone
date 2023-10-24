using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spreadsheet_Engine.OperatorNodeStuff
{
    /// <summary>
    /// Factory class for Operator Node and its subclasses.
    /// </summary>
    internal class OperatorNodeFactory
    {
        /// <summary>
        /// returns new operator node based off the inout character.
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        public static OperatorNode? Create(char op)
        {
            switch(op)
            {
                case '+': return new AdditionOperatorNode("+");
                case '-': return new SubtractionOperatorNode("-");
                case '*': return new MultiplicationOperatorNode("*");
                case '/': return new DivisionOperatorNode("/");
                default: return null;
            }
        }

        /// <summary>
        /// Returns operator precedence based off input character.
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        public static int Precedence(char op)
        {
            switch (op)
            {
                case '+': return 1;
                case '-': return 1;
                case '*': return 2;
                case '/': return 2;
                default: return 0;
            }
        }


    }
}
