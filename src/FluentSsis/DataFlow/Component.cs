namespace FluentSsis.DataFlow
{
    using System;
    using FluentSsis.Model;
    using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
    using Microsoft.SqlServer.Dts.Runtime;

    public class Component// : IDTSObject100
    {
        public Component(Dataflow pipeline, string moniker)
        {
            if (pipeline == null)
            {
                throw new ArgumentNullException(nameof(pipeline));
            }

            if (string.IsNullOrWhiteSpace(moniker))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(moniker));
            }



            Pipeline = pipeline.Pipeline;
            ComponentMetadata = (IDTSComponentMetaData130)Pipeline.ComponentMetaDataCollection.New();
            ComponentMetadata.ComponentClassID = moniker;
            ManagedComponentWrapper = ComponentMetadata.Instantiate();
            ManagedComponentWrapper.ProvideComponentProperties();
            Connections = pipeline.Connections;
        }

        public CManagedComponentWrapper ManagedComponentWrapper { get; }

        public IDTSComponentMetaData130 ComponentMetadata { get; }

        public MainPipe Pipeline { get; }

        public Connections Connections { get; }

        //public int ID
        //{
        //    get => ComponentMetadata.ID;
        //    set => ComponentMetadata.ID = value;
        //}

        //public string Description
        //{
        //    get => ComponentMetadata.Description;
        //    set => ComponentMetadata.Description = value;
        //}

        //public string Name
        //{
        //    get => ComponentMetadata.Name;
        //    set => ComponentMetadata.Name = value;
        //}

        //public DTSObjectType ObjectType => ComponentMetadata.ObjectType;

        //public string IdentificationString => ComponentMetadata.IdentificationString;
    }
}
