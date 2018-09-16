namespace FluentSsis.Tests.DataFlow
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NUnit.Framework;

    [SetUpFixture]
    public class Database
    {
        private const string ConnectionString = @"Server=(localdb)\MSSQLLocalDB;";
        private IEnumerable<string> _setupCommands = new List<string>
        {
            @"
CREATE DATABASE TestDb;",
            @"
USE TestDb;

CREATE TABLE TestTable
(
    StringValue VARCHAR(50),
    IntValue    INT,
    DateValue   DATETIME
)",
        };

        private IEnumerable<string> _teardownCommands = new List<string>
        {
            @"
USE master;

DROP DATABASE TestDb;",
        };

        [OneTimeSetUp]
        public void DatabaseSetup()
        {
            RunCommands(_setupCommands);
        }

        private void RunCommands(IEnumerable<string> commandList)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    foreach (var item in commandList)
                    {
                        command.CommandText = item;
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        [OneTimeTearDown]
        public void DatabaseTeardown()
        {
            RunCommands(_teardownCommands);
        }
    }
}
