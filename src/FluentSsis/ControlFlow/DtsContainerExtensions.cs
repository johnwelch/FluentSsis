namespace FluentSsis.ControlFlow
{
    using System;
    using Microsoft.SqlServer.Dts.Runtime;

    /// <summary>
    /// Extends objects in the SSIS Control Flow.
    /// </summary>
    public static class DtsContainerExtensions
    {
        public static TContainer Add<TContainer>(this TContainer container, Executable item)
            where TContainer : IDTSSequence
        {
            container.Executables.Join(item);
            return container;
        }

        public static TContainer Add<TContainer>(this TContainer container, Variable item)
            where TContainer : DtsContainer
        {
            container.Variables.Join(item);
            return container;
        }

        public static TContainer AddOrUpdate<TContainer>(this TContainer container, Variable item)
            where TContainer : DtsContainer
        {
            if (container.Variables.Contains(item.Name))
            {
                container.Variables[item.Name].Value = item.Value;
            }
            else
            {
                container.Variables.Join(item);
            }

            return container;
        }

        public static TContainer Add<TContainer>(this TContainer container, Action<IDTSSequence> item)
            where TContainer : IDTSSequence
        {
            item(container);
            return container;
        }
    }
}
