namespace FluentSsis.DataFlow
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using FluentSsis.Model;
    using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
    using Microsoft.SqlServer.Dts.Runtime;
    using Microsoft.SqlServer.Management.Common;
    using Microsoft.SqlServer.Management.Smo;

    /// <summary>
    /// Extends objects in the SSIS data flow.
    /// </summary>
    public static class MainPipeExtensions
    {
        //public static MainPipe Add(this MainPipe container, Action<MainPipe> item)
        //{
        //    // Because of SSIS's object model inconsistencies, we don't
        //    // need to actually do anything with component here, as the
        //    // factory method has already added it to the ComponentMetaDataCollection.
        //    item(container);
        //    return container;
        //}

        public static T FindObject<T>(this MainPipe container, string reference)
            where T : IDTSObject100
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            if (string.IsNullOrWhiteSpace(reference))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(reference));
            }

            // Find the component
            var result = ParseNextIdentifer(reference);
            IDTSComponentMetaData130 component = (IDTSComponentMetaData130)container.ComponentMetaDataCollection[result.extractedValue];
            if (typeof(T) == typeof(IDTSComponentMetaData130))
            {
                return (T)component;
            }

            // Get part of component
            result = ParseNextIdentifer(result.remainingIdentifier);
            if (result.extractedValue.StartsWith("INPUTS", StringComparison.OrdinalIgnoreCase))
            {
                IDTSInput100 input = component.InputCollection[GetValueBetweenBrackets(result.extractedValue)];
                if (typeof(T) == typeof(IDTSInput100))
                {
                    return (T)input;
                }

                // Find the column
                result = ParseNextIdentifer(result.remainingIdentifier);
                if (result.extractedValue.StartsWith("COLUMNS", StringComparison.OrdinalIgnoreCase))
                {
                    var columnName = GetValueBetweenBrackets(result.extractedValue);
                    try
                    {
                        IDTSInputColumn130 inputColumn = (IDTSInputColumn130)input.InputColumnCollection[columnName];
                        if (typeof(T) == typeof(IDTSInputColumn130))
                        {
                            return (T)inputColumn;
                        }
                    }
                    catch (COMException comEx)
                    {
                        if (comEx.HResult == unchecked((int)0xC0010009))
                        {
                            throw new InvalidOperationException($"{columnName} was not found in {input.Name}.");
                        }

                        throw;
                    }
                }
            }
            else if (result.extractedValue.StartsWith("OUTPUTS", StringComparison.OrdinalIgnoreCase))
            {
                IDTSOutput100 output = component.OutputCollection[GetValueBetweenBrackets(result.extractedValue)];
                if (typeof(T) == typeof(IDTSOutput100))
                {
                    return (T)output;
                }

                // Find the column
                result = ParseNextIdentifer(result.remainingIdentifier);
                if (result.extractedValue.StartsWith("COLUMNS", StringComparison.OrdinalIgnoreCase))
                {
                    IDTSOutputColumn130 outputColumn = (IDTSOutputColumn130)output.OutputColumnCollection[GetValueBetweenBrackets(result.extractedValue)];
                    if (typeof(T) == typeof(IDTSOutputColumn130))
                    {
                        return (T)outputColumn;
                    }
                }

                throw new ArgumentException($"Could not resolve the reference '{reference}'.", nameof(reference));
            }
            else if (result.extractedValue.StartsWith("CONNECTIONS", StringComparison.OrdinalIgnoreCase))
            {
                IDTSRuntimeConnection100 connection = component.RuntimeConnectionCollection[GetValueBetweenBrackets(result.extractedValue)];
                if (typeof(T) == typeof(IDTSRuntimeConnection100))
                {
                    return (T)connection;
                }
            }







            return default(T);
        }

        private static string GetValueBetweenBrackets(string valueToParse)
        {
            int startPosition = valueToParse.IndexOf('[') + 1;
            int endPosition = valueToParse.IndexOf(']');
            return valueToParse.Substring(startPosition, endPosition - startPosition);
        }

        /// <summary>
        /// Gets the next piece of an identifier for a data flow object.
        /// </summary>
        /// <param name="identifer">The identifer to parse.</param>
        /// <returns>A tuple containing the extracted value, and the remaining portion of the identifier.</returns>
        public static (string extractedValue, string remainingIdentifier) ParseNextIdentifer(string identifer)
        {
            // Package\DF\MyOleDbSource.Outputs[OLE DB Source Error Output].Columns[ErrorCode]

            // Look for a \ as final seperator before Component Name
            // Look for a . as seperator
            // Names can't include .

            char[] buffer = new char[identifer.Length];
            int bufferIndex = 0;

            for (int i = 0; i < identifer.Length; i++)
            {
                switch (identifer[i])
                {
                    case '\\': // Found an executable seperator, reset the buffer
                        bufferIndex = 0;
                        break;
                    case '.': // Found a dataflow seperator
                        if (bufferIndex == 0)
                        {
                            // We just reset the buffer, shouldn't see this
                            Debug.Assert(false, "Found a period after a reset");
                        }

                        return (new string(buffer, 0, bufferIndex), identifer.Substring(i + 1));
                    default:
                        buffer[bufferIndex] = identifer[i];
                        bufferIndex++;
                        break;
                }
            }

            return (new string(buffer, 0, bufferIndex), string.Empty);

        }

        //public static T ParseIdentification<T>(string identifer)
        //{
        //    // Package\DF\MyOleDbSource.Outputs[OLE DB Source Error Output].Columns[ErrorCode]

        //    // - Component
        //    //   - Properties

        //    //   - Inputs
        //    //   - Outputs
        //    //   - Connections
        //    var navTree = new NavigationPath
        //    {
        //        Delimiter = "\\",
        //        Name = "ControlFlow",
        //        Children = new List<NavigationPath>()
        //        {
        //            new NavigationPath() {}
        //        }
        //    };

        //    // Look for a \ as final seperator before Component Name
        //    // Look for a . as seperator
        //    // Names can't include .

        //    char[] buffer = new char[identifer.Length];
        //    int bufferIndex = 0;

        //    for (int i = 0; i < identifer.Length; i++)
        //    {
        //        switch (identifer[i])
        //        {
        //            case '\\': // Found an executable seperator, reset the buffer
        //                bufferIndex = 0;
        //                break;
        //            case '.': // Found a dataflow seperator
        //                if (bufferIndex == 0)
        //                {
        //                    // We just reset the buffer, shouldn't see this
        //                    Debug.Assert(false, "Found a period after a reset");
        //                }

        //                return (new string(buffer, 0, bufferIndex), identifer.Substring(i + 1));
        //            default:
        //                buffer[bufferIndex] = identifer[i];
        //                bufferIndex++;
        //                break;
        //        }
        //    }

        //    return (new string(buffer, 0, bufferIndex), string.Empty);

        //}

        private class NavigationPath
        {
            public string Name { get; set; }

            public Type ObjectType { get; set; }

            public string Match { get; set; }

            public string Delimiter { get; set; }

            public IList<NavigationPath> Children { get; set; }
        }
    }
}
