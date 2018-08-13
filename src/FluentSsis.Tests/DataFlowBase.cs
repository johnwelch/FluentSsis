namespace FluentSsis.Tests
{
    using FluentSsis.ControlFlow;
    using FluentSsis.DataFlow;
    using FluentSsis.Emitter;
    using FluentSsis.Factories;
    using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
    using Microsoft.SqlServer.Dts.Runtime;

    internal class DataFlowBase
    {
        public DataFlowBase(string packageName)
        {
            Package = new Package();
            Package.Named(packageName)
                .WithDescription(packageName)
                .Add(New.Connection.FromMoniker("OLEDB")
                    .Named("Source")
                    .WithDescription(packageName)
                    .WithConnectionString("Data Source=localhost;Initial Catalog=AdventureWorks;Provider=SQLNCLI11.1;Integrated Security=SSPI;Auto Translate=False;"));
            var pipe = Package.New<TaskHost>("Microsoft.Pipeline")
                .Named("DF")
                .WithDescription(packageName)
                .ToPipelineContext();
            pipe.New("Microsoft.OLEDBSource")
                    .Named("MyOleDbSource")
                    .WithProperty("SqlCommand", "SELECT * FROM [Production].[Product]")
                    .WithProperty("AccessMode", 2)
                    .WithConnection("Source")
                    .RefreshMetadata()
                .New("Microsoft.OLEDBDestination")
                    .Named("MyOleDbTarget")
                    .WithProperty("OpenRowset", "[Production].[Product]")
                    .WithProperty("AccessMode", 3)
                    .WithConnection("Source")
                    .RefreshMetadata()
                    .WithInput("MyOleDbSource.Output");
        }

        public Package Package { get; }
    }
}
