namespace FluentSsis.ControlFlow
{
    using Microsoft.SqlServer.Dts.Runtime;

    /// <summary>
    /// Extends the <see cref="Package"/> class.
    /// </summary>
    public static class PackageExtensions
    {
        /// <summary>
        /// Adds a new connection manager to an SSIS package.
        /// </summary>
        /// <param name="package">The package to add the connection manager to.</param>
        /// <param name="connection">The connection amanager to add.</param>
        /// <returns>The package.</returns>
        public static Package Add(this Package package, ConnectionManager connection)
        {
            package.Connections.Join(connection);
            return package;
        }

        // Logs
        // Parameters
    }
}
