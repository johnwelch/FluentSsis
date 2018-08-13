namespace FluentSsis.DataFlow
{
    using FluentSsis.Model;

    public static class DataflowExtensions
    {
        public static Component New(this PipelineContext container, string moniker)
        {
            var component = new Component(container, moniker);
            return component;
        }
    }
}
