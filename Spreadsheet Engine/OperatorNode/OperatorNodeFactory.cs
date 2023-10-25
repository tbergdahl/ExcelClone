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
            return op switch
            {
                '+' => new AdditionOperatorNode("+"),
                '-' => new SubtractionOperatorNode("-"),
                '*' => new MultiplicationOperatorNode("*"),
                '/' => new DivisionOperatorNode("/"),
                _ => null,
            };
        }

        /// <summary>
        /// Returns operator precedence based off input character.
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        public static int Precedence(char op)
        {
            return op switch
            {
                '+' => 1,
                '-' => 1,
                '*' => 2,
                '/' => 2,
                _ => 0,
            };
        }


    }
}
