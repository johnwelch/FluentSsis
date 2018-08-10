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
            Package = new Package()
                .Named(packageName)
                .WithDescription(packageName)
                .Add(New.Connection.FromMoniker("OLEDB")
                    .Named("Source")
                    .WithDescription(packageName)
                    .WithConnectionString("Data Source=localhost;Initial Catalog=AdventureWorks2017;Provider=SQLNCLI11.1;Integrated Security=SSPI;Auto Translate=False;"))
                .Add(container =>
                    {
                        New.Executable.DataFlow(container)
                        .Named("DF")
                        .WithDescription(packageName)
                        .AsDataflow(pipe =>
                        {
                            pipe.Pipeline
                                .Add(New.Component.FromMoniker(pipe, "Microsoft.OLEDBSource")
                                        .Named("MyOleDbSource")
                                        .WithProperty("SqlCommand", "SELECT * FROM sys.tables")
                                        .WithProperty("AccessMode", 2)
                                        .WithConnection("Source")
                            //.RefreshMetadata()
                            //);
                            )
                            .Add(New.Component.FromMoniker(pipe, "Microsoft.OLEDBDestination")
                                .Named("MyOleDbTarget")
                                .WithProperty("SqlCommand", "Do something")
                                .WithProperty("AccessMode", 1)
                               // .WithInput("MyOleDbSOurce.Output")
                            );
                        });
                    }
                );
        }

        public Package Package { get; }
    }
}
