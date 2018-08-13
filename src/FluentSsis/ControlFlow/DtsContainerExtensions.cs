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

        public static TItem New<TItem>(this IDTSSequence container, string moniker)
            where TItem : EventsProvider
        {
            if (string.IsNullOrWhiteSpace(moniker))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(moniker));
            }

            try
            {
                Executable executable = container.Executables.Add(moniker);
                return executable as TItem;
            }
            catch (DtsRuntimeException e)
            {
                throw new InvalidOperationException($"The executable could not be created. This is often caused when the moniker ({moniker}) does not match any installed SSIS executables.", e);
            }
        }

        public static TItem ParentAs<TItem>(this DtsContainer container)
            where TItem : DtsContainer
        {
            return container.Parent as TItem;
        }

        public static Package GetPackage(this DtsContainer container)
        {
            while (container.GetType() != typeof(Package))
            {
                container = container.Parent;
            }

            return (Package)container;
        }
    }
}