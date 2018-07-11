namespace FluentSsis.Factories
{
    using FluentSsis.ControlFlow;
    using Microsoft.SqlServer.Dts.Runtime;

    /// <summary>
    /// A factory class for creating new SSIS connection managers.
    /// </summary>
    public class ConnectionFactory
    {
        /// <summary>
        /// Creates a connection manager.
        /// </summary>
        /// <param name="moniker">The creation name for the connection manager.</param>
        /// <returns>The newly created connection manager.</returns>
        public ConnectionManager FromMoniker(string moniker)
        {
            var connection = PackageSingleton.Instance.Connections.Add(moniker);
            PackageSingleton.Instance.Connections.Remove(connection);
            return connection;
        }

        /// <summary>
        /// Creates an OLEDB connection manager.
        /// </summary>
        /// <returns>The newly created connection manager.</returns>
        public ConnectionManager OleDb()
        {
            return FromMoniker("OLEDB");
        }
    }
}
