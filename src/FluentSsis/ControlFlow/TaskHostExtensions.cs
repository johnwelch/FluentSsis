namespace FluentSsis.ControlFlow
{
    using System;
    using FluentSsis.Model;
    using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
    using Microsoft.SqlServer.Dts.Runtime;

    public static class TaskHostExtensions
    {
        /// <summary>
        /// Converts the inner object of a <see cref="TaskHost"/> to the specified type so that
        /// actions can be performed on it.
        /// </summary>
        /// <typeparam name="T">The type of the inner object.</typeparam>
        /// <param name="taskHost">The TaskHost to evaluate.</param>
        /// <param name="action">The action to take on the inner object.</param>
        /// <returns>The <see cref="taskHost"/> that was passed in.</returns>
        /// <exception cref="InvalidCastException">Thrown if the inner object can't be cast to the specified type.</exception>
        public static TaskHost As<T>(this TaskHost taskHost, Action<T> action)
            where T : class
        {
            if (taskHost == null)
            {
                throw new ArgumentNullException(nameof(taskHost));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var innerObject = taskHost.ConvertTo<T>();

            action(innerObject);

            return taskHost;
        }

        /// <summary>
        /// Converts the inner object of a <see cref="TaskHost"/> to the specified type so that
        /// actions can be performed on it.
        /// </summary>
        /// <typeparam name="T">The type of the inner object.</typeparam>
        /// <param name="taskHost">The TaskHost to evaluate.</param>
        /// <returns>The converted inner object.</returns>
        /// <exception cref="InvalidCastException">Thrown if the inner object can't be cast to the specified type.</exception>
        public static T ConvertTo<T>(this TaskHost taskHost)
            where T : class
        {
            if (taskHost == null)
            {
                throw new ArgumentNullException(nameof(taskHost));
            }

            T innerObject = taskHost.InnerObject as T;
            if (innerObject == null)
            {
                throw new InvalidCastException();
            }

            return innerObject;
        }

        public static TaskHost AsDataflow(this TaskHost executable, Action<Dataflow> action)
        {
            if (executable == null)
            {
                throw new ArgumentNullException(nameof(executable));
            }

            var pipeline = executable.InnerObject as MainPipe;

            if (pipeline == null)
            {
                throw new InvalidCastException();
            }

            // TODO: Not working, because the parent of the executable is NULL
            // It's been removed from the Signleton, but not added to the real package
            action(new Dataflow(GetPackage(executable).Connections, executable.Variables, pipeline));

            return executable;
        }

        private static Package GetPackage(DtsContainer executable)
        {
            while (executable.GetType() != typeof(Package))
            {
                executable = executable.Parent;
            }

            return (Package)executable;
        }
    }
}
