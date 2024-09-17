using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spreadsheet_Engine
{
    public class NumericNode : Node
    {
        double constant;
        public NumericNode(double nConstant)
        {
            constant = nConstant;
        }


        public double Constant
        {
            get { return constant; }
            set { constant = value; }
        }


        public override double Evaluate()
        {
            return constant;
        }
    }
}
