namespace FluentSsis.Factories
{
    using System;
    using Microsoft.SqlServer.Dts.Runtime;

    public static class New
    {
        private static readonly Lazy<ComponentFactory> Components = new Lazy<ComponentFactory>(() => new ComponentFactory());

        private static readonly Lazy<ConnectionFactory> Connections =
                    new Lazy<ConnectionFactory>(() => new ConnectionFactory());

        private static readonly Lazy<ConstraintFactory> Constraints = new Lazy<ConstraintFactory>(() => new ConstraintFactory());

        private static readonly Lazy<DestinationFactory> Destinations = new Lazy<DestinationFactory>(() => new DestinationFactory());

        private static readonly Lazy<ExecutableFactory> Executables =
            new Lazy<ExecutableFactory>(() => new ExecutableFactory());

        private static readonly Lazy<SourceFactory> Sources = new Lazy<SourceFactory>(() => new SourceFactory());

        private static readonly Lazy<VariableFactory> Variables =
                                            new Lazy<VariableFactory>(() => new VariableFactory());

        public static ComponentFactory Component => Components.Value;

        public static ConnectionFactory Connection => Connections.Value;

        public static ConstraintFactory Constraint => Constraints.Value;

        public static DestinationFactory Destination => Destinations.Value;

        public static ExecutableFactory Executable => Executables.Value;

        public static SourceFactory Source => Sources.Value;

        public static Variable Variable(string name, object value) => Variables.Value.Create(name, value);
    }
}