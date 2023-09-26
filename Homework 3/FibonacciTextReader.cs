using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Homework_3
{
    public class FibonacciTextReader : TextReader
    {

        public FibonacciTextReader(int max_lines)
        {
            if (max_lines < 1)
            {
                throw new ArgumentException("Enter Positive Integer");
            }
            n = max_lines;
        }


        /// <summary>
        /// Returns the next value in the fibonacci sequence as a string
        /// </summary>
        /// <returns>result or null</returns>
        public override string? ReadLine()
        {
            if(current < n)
            {
                string result = currentNum.ToString();//save previous number in sequence

                if(current == 0)
                {
                    current++; 
                }
                else if(current == 1)
                {
                    current++;//keep track of current position in sequence
                    BigInteger temp = ++currentNum; //save to update prev
                    currentNum += prev;
                    prev = temp;
                    result = currentNum.ToString();
                }
                else
                {
                    current++;//keep track of current position in sequence
                    BigInteger temp = currentNum; //save to update prev
                    currentNum += prev;
                    prev = temp;
                }

                return result;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// //max lines
        /// </summary>
        private readonly int n;

        /// <summary>
        /// private int current; //position in sequence
        /// </summary>

        private int current;

        /// <summary>
        /// //last fibonacci that we used
        /// </summary>
        private BigInteger prev;



        /// <summary>
        /// //current fibonacci in sequence
        /// </summary>
        private BigInteger currentNum;
    }
}
