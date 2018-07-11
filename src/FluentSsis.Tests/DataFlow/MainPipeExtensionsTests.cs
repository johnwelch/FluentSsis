namespace FluentSsis.Tests.DataFlow
{
    using System;
    using FluentSsis.ControlFlow;
    using FluentSsis.DataFlow;
    using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
    using Microsoft.SqlServer.Dts.Runtime;
    using NUnit.Framework;

    [TestFixture]
    public class MainPipeExtensionsTests
    {
        private Package _pkg;

        [SetUp]
        public void Setup()
        {
            _pkg = new DataFlowBase("DataFlowTest").Package;
        }

        [TestCase(@"MyOleDbSource", "MyOleDbSource", @"")]
        [TestCase(@"Package\DF\MyOleDbTarget.Outputs[OLE DB Destination Error Output].Columns[ErrorCode]", "MyOleDbTarget", @"Outputs[OLE DB Destination Error Output].Columns[ErrorCode]")]
        [TestCase(@"Outputs[OLE DB Destination Error Output].Columns[ErrorCode]", "Outputs[OLE DB Destination Error Output]", @"Columns[ErrorCode]")]
        [TestCase(@"Columns[ErrorCode]", "Columns[ErrorCode]", @"")]
        public void ResolveIdentificationString(string identifier, string expectedNextValue, string expectedRemaining)
        {
            var result = MainPipeExtensions.ParseNextIdentifer(identifier);
            Assert.That(result.extractedValue, Is.EqualTo(expectedNextValue));
            Assert.That(result.remainingIdentifier, Is.EqualTo(expectedRemaining));
        }

        [TestCase("MyOleDbSource", "MyOleDbSource")]
        [TestCase(@"Package\DF\MyOleDbTarget", "MyOleDbTarget")]
        [TestCase(@"Package\DF\MyOleDbTarget.Outputs[OLE DB Destination Error Output].Columns[ErrorCode]", "MyOleDbTarget")]
        public void FindObjectComponentTest(string identifier, string expectedName)
        {
            var df = _pkg.Executables["DF"].ConvertTo<MainPipe>();
            var result = df.FindObject<IDTSComponentMetaData130>(identifier);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(expectedName));
        }

        [TestCase(@"MyOleDbTarget.Inputs[OLE DB Destination Input]", "OLE DB Destination Input")]
        [TestCase(@"Package\DF\MyOleDbTarget.Inputs[OLE DB Destination Input]", "OLE DB Destination Input")]
        public void FindObjectInputTest(string identifier, string expectedName)
        {
            var df = _pkg.Executables["DF"].ConvertTo<MainPipe>();
            var result = df.FindObject<IDTSInput100>(identifier);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(expectedName));
        }

        [TestCase(@"MyOleDbTarget.Inputs[OLE DB Destination Input].Columns[name]", "OLE DB Destination Input")]
        [TestCase(@"Package\DF\MyOleDbTarget.Inputs[OLE DB Destination Input].Columns[name]", "OLE DB Destination Input")]
        public void FindObjectInputColumnTest(string identifier, string expectedName)
        {
            var df = _pkg.Executables["DF"].ConvertTo<MainPipe>();
            var result = df.FindObject<IDTSInputColumn130>(identifier);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(expectedName));
        }

        [TestCase(@"MyOleDbSource.Outputs[OLE DB Source Output].Columns[name]", "name")]
        [TestCase(@"Package\DF\MyOleDbSource.Outputs[OLE DB Source Output].Columns[name]", "name")]
        public void FindObjectOutputColumnTest(string identifier, string expectedName)
        {
            var df = _pkg.Executables["DF"].ConvertTo<MainPipe>();
            var result = df.FindObject<IDTSOutputColumn130>(identifier);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(expectedName));
        }

        [TestCase(@"MyOleDbSource.Outputs[OLE DB Source Output]", "OLE DB Source Output")]
        [TestCase(@"Package\DF\MyOleDbSource.Outputs[OLE DB Source Output]", "OLE DB Source Output")]
        public void FindObjectOutputTest(string identifier, string expectedName)
        {
            var df = _pkg.Executables["DF"].ConvertTo<MainPipe>();
            var result = df.FindObject<IDTSOutput100>(identifier);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(expectedName));
        }

        [TestCase(@"MyOleDbSource.Connections[OleDbConnection]", "OleDbConnection")]
        [TestCase(@"Package\DF\MyOleDbSource.Connections[OleDbConnection]", "OleDbConnection")]
        public void FindObjectConnectionTest(string identifier, string expectedName)
        {
            var df = _pkg.Executables["DF"].ConvertTo<MainPipe>();
            var result = df.FindObject<IDTSRuntimeConnection100>(identifier);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(expectedName));
        }
    }
}
