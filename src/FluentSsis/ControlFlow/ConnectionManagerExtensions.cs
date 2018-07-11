namespace FluentSsis.ControlFlow
{
    using System;
    using Microsoft.SqlServer.Dts.Runtime;

    /// <summary>
    /// Extends the <see cref="ConnectionManager"/> class.
    /// </summary>
    public static class ConnectionManagerExtensions
    {
        /// <summary>
        /// Sets the connection string on a <see cref="ConnectionManager"/>.
        /// </summary>
        /// <param name="connectionManager">The connection manager to set the connection string on.</param>
        /// <param name="connectionString">Tne connection string to use.</param>
        /// <returns>The connection manager that was updated.</returns>
        public static ConnectionManager WithConnectionString(this ConnectionManager connectionManager, string connectionString)
        {
            if (connectionManager == null)
            {
                throw new ArgumentNullException(nameof(connectionManager));
            }

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(connectionString));
            }

            connectionManager.ConnectionString = connectionString;
            return connectionManager;
        }
    }
}
