namespace FluentSsis.Factories
{
    using System;
    using FluentSsis.ControlFlow;
    using Microsoft.SqlServer.Dts.Runtime;

    /// <summary>
    /// A factory class for creating new SSIS control flow objects.
    /// </summary>
    public class ExecutableFactory
    {
        /// <summary>
        /// Creates a new executable of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the executable being added.</typeparam>
        /// <param name="moniker">The creation name for the executable being added.</param>
        /// <returns>The newly created item.</returns>
        public T New<T>(string moniker)
            where T : EventsProvider
        {
            Executable executable = New<T>(PackageSingleton.Instance, moniker);
            PackageSingleton.Instance.Executables.Remove(executable);
            return executable as T;
        }

        public T New<T>(IDTSSequence container, string moniker)
            where T : EventsProvider
        {
            if (string.IsNullOrWhiteSpace(moniker))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(moniker));
            }

            Executable executable;
            try
            {
                executable = container.Executables.Add(moniker);
            }
            catch (DtsRuntimeException e)
            {
                throw new InvalidOperationException($"The executable could not be created. This is often caused when the moniker ({moniker}) does not match any installed SSIS executables.", e);
            }

            return executable as T;
        }

        public TaskHost ExecuteSql()
        {
            return New<TaskHost>("Microsoft.ExecuteSQLTask");
        }

        public TaskHost DataFlow(IDTSSequence container)
        {
            return New<TaskHost>(container, "Microsoft.Pipeline");
        }
    }
}
