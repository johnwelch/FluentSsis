using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentSsis.Model
{
    using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
    using Microsoft.SqlServer.Dts.Runtime;

    public class Dataflow
    {
        public Dataflow(Connections connections, Variables variables, MainPipe pipeline)
        {
            Connections = connections;
            Variables = variables;
            Pipeline = pipeline;
        }

        public MainPipe Pipeline { get; }

        public Connections Connections { get; }

        public Variables Variables { get; }
    }
}
