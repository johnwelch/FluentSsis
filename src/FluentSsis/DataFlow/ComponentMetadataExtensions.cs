namespace FluentSsis.DataFlow
{
    using Microsoft.SqlServer.Dts.Pipeline.Wrapper;

    public static class ComponentMetadataExtensions
    {
        public static CManagedComponentWrapper Wrapper(this IDTSComponentMetaData100 component)
        {
            // TODO: Profile the performance on this
            return component.Instantiate();
        }
    }
}
