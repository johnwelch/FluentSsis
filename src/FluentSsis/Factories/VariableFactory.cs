namespace FluentSsis.Factories
{
    using System;
    using System.Text.RegularExpressions;
    using FluentSsis.ControlFlow;
    using Microsoft.SqlServer.Dts.Runtime;

    /// <summary>
    /// A factory class for creating SSIS variables.
    /// </summary>
    public class VariableFactory
    {
        public Variable Create(string name, object value)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            }

            var names = name.Split(new[] {"::"}, StringSplitOptions.RemoveEmptyEntries);
            if (names.Length > 2)
            {
                throw new ArgumentException("Could not parse name into namespace and name. To set the namespace, the value should be in the format 'namespace::name'.", nameof(name));
            }

            string variableName;
            string variableNamespace;
            if (names.Length == 2)
            {
                variableNamespace = names[0];
                variableName = names[1];
            }
            else
            {
                variableNamespace = "User";
                variableName = names[0];
            }

            Variable variable = PackageSingleton.Instance.Variables.Add(variableName, false, variableNamespace, value);
            PackageSingleton.Instance.Variables.Remove(variable);
            return variable;
        }
    }
}
