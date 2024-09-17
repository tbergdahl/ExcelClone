using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Spreadsheet_Engine.OperatorNodeStuff
{
    /// <summary>
    /// Factory class for Operator Node and its subclasses.
    /// </summary>
    internal class OperatorNodeFactory
    {

        private Dictionary<string, Type> operators = new Dictionary<string, Type>();


        public OperatorNodeFactory()
        {
            TraverseAvailableOperators((op, type) => operators.Add(op, type));
        }


        /// <summary>
        /// returns new operator node based off the inout character.
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        public OperatorNode? Create(string op)
        {
            if(operators.ContainsKey(op))
            {
                object operatorNodeObject = System.Activator.CreateInstance(operators[op]);
                if(operatorNodeObject != null)
                {
                    return (OperatorNode)operatorNodeObject;
                }
            }
            throw new Exception("Unhandled Operator.");
        }

        /// <summary>
        /// Returns operator precedence based off input character.
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        public int Precedence(string op)
        {
            if (operators.ContainsKey(op))
            {
                object operatorNodeObject = System.Activator.CreateInstance(operators[op]);
                if (operatorNodeObject is OperatorNode)
                {
                    OperatorNode operatorNode = (OperatorNode)operatorNodeObject;
                    return operatorNode.Precedence;
                }
            }
            throw new Exception("Operator Not Supported.");
        }


        private delegate void OnOperator(string op, Type type);
            
        private void TraverseAvailableOperators(OnOperator onOperator)
        {
            Type operatorNodeType = typeof(OperatorNode);
            foreach(var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                IEnumerable<Type> operatorTypes = assembly.GetTypes().Where(type=> type.IsSubclassOf(operatorNodeType));

                foreach (var type in operatorTypes)
                {
                    PropertyInfo operatorField = type.GetProperty("OperationType");

                    if(operatorField != null)
                    {
                        object value = operatorField.GetValue(Activator.CreateInstance(type));
                        if (value is string)
                        {
                            string operatorSymbol = (string)value;
                            onOperator(operatorSymbol, type);
                        }
                    }
                }
            }

            
        }


        public bool IsOperator(string op)
        {
            if(operators.ContainsKey(op)) 
            {
                return true;
            }
            return false;
        }

    }
}
