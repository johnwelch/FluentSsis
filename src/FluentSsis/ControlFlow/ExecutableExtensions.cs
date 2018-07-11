using System;

namespace FluentSsis.ControlFlow
{
    using FluentSsis.Model;
    using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
    using Microsoft.SqlServer.Dts.Runtime;

    public static class ExecutableExtensions
    {
        /// <summary>
        /// Converts the inner object of a <see cref="TaskHost"/> to the specified type so that
        /// actions can be performed on it. This extends <see cref="Executable"/> but it must
        /// be castable to a <see cref="TaskHost"/> to function.
        /// </summary>
        /// <typeparam name="T">The type of the inner object.</typeparam>
        /// <param name="executable">The Executable to evaluate.</param>
        /// <returns>The converted inner object.</returns>
        /// <exception cref="InvalidCastException">Thrown if the executable is not a TaskHost, or if the inner object can't be cast to the specified type.</exception>
        public static T ConvertTo<T>(this Executable executable)
            where T : class
        {
            if (executable == null)
            {
                throw new ArgumentNullException(nameof(executable));
            }

            TaskHost taskHost = executable as TaskHost;

            if (taskHost == null)
            {
                throw new InvalidCastException();
            }

            return taskHost.ConvertTo<T>();
        }
    }
}
