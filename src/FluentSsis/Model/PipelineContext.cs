namespace FluentSsis.Model
{
    using FluentSsis.Utility;
    using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
    using Microsoft.SqlServer.Dts.Runtime;

    public class PipelineContext
    {
        public PipelineContext(Connections connections, Variables variables, MainPipe pipeline)
        {
            Connections = connections;
            Variables = variables;
            MainPipe = pipeline;

            MainPipe.Events = DtsConvert.GetExtendedInterface(new PipelineEvents());
        }

        public MainPipe MainPipe { get; }

        public Connections Connections { get; }

        public Variables Variables { get; }
    }
}
