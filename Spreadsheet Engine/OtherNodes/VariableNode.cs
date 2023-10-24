using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spreadsheet_Engine
{
    public class VariableNode : Node
    {
        readonly string varName;
        readonly Dictionary<string, double> variables;

        public VariableNode(string newVarName, Dictionary<string, double> treeDic)
        {
            this.varName = newVarName;
            variables = treeDic;
        }

        public string VarName
        {
            get { return varName; }
        }


        public override double Evaluate()
        {
            if( variables.TryGetValue(varName, out var value) == true)
            {
                return value;
            }
            else
            {
                throw new Exception("Variable Doesn't Exist.\n");
            }
        }
    }

}
