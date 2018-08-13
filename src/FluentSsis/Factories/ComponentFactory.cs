namespace FluentSsis.Factories
{
    using FluentSsis.DataFlow;
    using FluentSsis.Model;
    using Microsoft.SqlServer.Dts.Pipeline.Wrapper;

    public class ComponentFactory
    {
        //public IDTSComponentMetaData100 FromMoniker(MainPipe mainPipe, string moniker)
        //{
        //    IDTSComponentMetaData100 metaData = mainPipe.ComponentMetaDataCollection.New();

        //    // TODO: May need to deal with .NET types here
        //    metaData.ComponentClassID = moniker;

        //    // TODO: So the design time methods are split between
        //    // IDTSComponentMetaData100, 130, and CManagedComponentWrapper
        //    // which blows chunks. Investigate adding a shim that combines these?
        //    // Would want to hide the methods on IDTSComponent* that shouldn't be visible,
        //    // but keep it compatible
        //    CManagedComponentWrapper wrapper = metaData.Instantiate();
        //    wrapper.ProvideComponentProperties();

        //    return metaData;
        //}

        public Component FromMoniker(PipelineContext dataflow, string moniker)
        {
            var component = new Component(dataflow, moniker);
            return component;
        }
    }
}