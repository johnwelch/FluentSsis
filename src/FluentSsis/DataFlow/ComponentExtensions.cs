namespace FluentSsis.DataFlow
{
    using System;
    using FluentSsis.Model;
    using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
    using Microsoft.SqlServer.Dts.Runtime;
    using Microsoft.SqlServer.Dts.Runtime.Wrapper;

    public static class ComponentExtensions
    {
        public static Component Named(this Component component, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(nameof(name));
            }

            component.ComponentMetadata.Name = name;
            return component;
        }

        public static Component WithDescription(this Component component, string description)
        {
            component.ComponentMetadata.Description = description;
            return component;
        }

        public static Component WithProperty(this Component component, string propertyName, object value)
        {
            // TODO: Give some thought to error handling patterns
            try
            {
                component.ManagedComponentWrapper.SetComponentProperty(propertyName, value);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return component;
        }

        public static Component New(this Component component, string moniker)
        {
            return component.Parent.New(moniker);
        }

        public static Component WithConnection(this Component component, string name)
        {
            var connection = component.Connections[name];
            component.ComponentMetadata.RuntimeConnectionCollection[0].ConnectionManager = DtsConvert.GetExtendedInterface(connection);
            component.ComponentMetadata.RuntimeConnectionCollection[0].ConnectionManagerID = connection.ID;
            return component;
        }

        public static Component WithInput(this Component component, string outputName)
        {
            return WithInput(component, outputName, component.ComponentMetadata.InputCollection[0].Name);
        }

        public static Component WithInput(this Component component, string outputName, string inputName)
        {
            IDTSInput100 input = component.ComponentMetadata.InputCollection[inputName];

            // TODO: Have to decide how to handle name resolution
            //            IDTSOutput100 output = component.Pipeline.GetObjectByID()
            //component.Pipeline.ComponentMetaDataCollection.;


            var path = component.Parent.MainPipe.PathCollection.New();

            // path.AttachPathAndPropagateNotifications(output, input);
            return component;
        }

        public static Component RefreshMetadata(this Component component)
        {
            component.ManagedComponentWrapper.AcquireConnections(null);
            component.ManagedComponentWrapper.ReinitializeMetaData();
            component.ManagedComponentWrapper.ReleaseConnections();
            return component;
        }
    }
}
