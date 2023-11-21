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
        readonly Dictionary<string, Spreadsheet.SpreadsheetCell> variables;

        public VariableNode(string newVarName, Dictionary<string, Spreadsheet.SpreadsheetCell> treeDic)
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
            if(variables.TryGetValue(varName, out var cell) == true)
            {
                if (cell.Value == "!(bad reference)")
                {
                    return 0;
                }
                if (cell.Value != null)
                {
                    return double.Parse(cell.Value);
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
    }

}
