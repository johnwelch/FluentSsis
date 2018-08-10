namespace FluentSsis.Tests.ControlFlow
{
    using FluentSsis.ControlFlow;
    using FluentSsis.Emitter;
    using FluentSsis.Factories;
    using Microsoft.SqlServer.Dts.Runtime;
    using NUnit.Framework;
    using System;
    using System.IO;

    [TestFixture]
    public class ControlFlowIntegrationTests
    {
        private readonly string PackageFile = Path.ChangeExtension(Path.GetTempFileName(), ".dtsx");
        private const string TestString = "ControlFlowTests";

        private Package _pkg;

        [SetUp]
        public void Setup()
        {
            // TODO: Add Project support
            _pkg = new Package()
                .Named(TestString)
                .WithDescription(TestString)
                .Add(New.Connection.FromMoniker("OLEDB")
                    .Named("Source")
                    .WithDescription(TestString)
                    .WithConnectionString("Data Source=localhost;Initial Catalog=AdvancedSsisScripting;Provider=SQLNCLI11.1;Integrated Security=SSPI;Auto Translate=False;"))
                .Add(New.Variable("TruncateSql", "SELECT * FROM sys.tables"))
                .Add(New.Executable.New<TaskHost>("Microsoft.ExecuteSQLTask")
                    .Named("Truncate Table")
                    .WithDescription(TestString)
                    .WithProperty("SqlStatementSourceType", 3)
                    .WithProperty("SqlStatementSource", "User::TruncateSql")
                    .WithProperty("Connection", "Source"))
                .Add(New.Executable.New<Sequence>("STOCK:SEQUENCE")
                    .Named("Stuff")
                    .Add(New.Executable.New<TaskHost>("Microsoft.ExecuteSqlTask")
                        .Named("Test")
                        .Add(New.Variable("Test2", 5)))
                    .Add(New.Variable("Test3", 5)))
                .Add(New.Constraint.Success("Truncate Table", "Stuff"));
        }

        [Test]
        public void ValidateSettings()
        {
            Assert.That(_pkg.Name, Is.EqualTo(TestString));
            Assert.That(_pkg.Description, Is.EqualTo(TestString));
            Assert.That(_pkg.Connections.Contains("Source"));

            Assert.That(_pkg.Variables.Contains("TruncateSql"), "Variable not found");

            Assert.That(_pkg.Executables.Count, Is.GreaterThan(0));

            var task = _pkg.Executables["Truncate Table"] as TaskHost;
            Assert.That(task, Is.Not.Null);
            var sequence = _pkg.Executables["Stuff"] as Sequence;
            var subTask = sequence.Executables["Test"] as TaskHost;
            Assert.That(subTask.Variables.Contains("Test2"), "Variable not found");
            Assert.That(sequence.Variables.Contains("Test3"), "Variable not found");

            Assert.That(_pkg.PrecedenceConstraints.TryGetValue("Truncate Table", "Stuff", out PrecedenceConstraint constraint), Is.True);
            Assert.That(constraint.Value, Is.EqualTo(DTSExecResult.Success));

            //Assert.That(_pkg.Connections.Contains("Target"));
            //Assert.That(task.Properties["Connection"].GetValue(task), Is.EqualTo(_pkg.Connections["Target"].Name));
        }

        [Test]
        [Ignore("Don't want to execute this currently")]
        public void ExecutePackage()
        {
            Assert.That(_pkg.Execute(), Is.EqualTo(DTSExecResult.Success));
        }

        [Test]
        public void SavePackage()
        {
            Assert.DoesNotThrow(() => _pkg.Save($"{PackageFile}"));
        }
    }
}
