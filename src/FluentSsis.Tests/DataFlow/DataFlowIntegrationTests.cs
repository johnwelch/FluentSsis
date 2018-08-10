namespace FluentSsis.Tests.DataFlow
{
    using FluentSsis.ControlFlow;
    using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
    using Microsoft.SqlServer.Dts.Runtime;
    using NUnit.Framework;
    using System.IO;
    using System.Linq;

    [TestFixture]
    public class DataFlowIntegrationTests
    {
        private readonly string PackageFile = Path.ChangeExtension(Path.GetTempFileName(), ".dtsx");
        private const string TestString = "DataFlowTests";

        private Package _pkg;

        [SetUp]
        public void Setup()
        {
           _pkg = new DataFlowBase(TestString).Package;
        }

        //[Test]
        //public void TestSomeSsisStuff()
        //{
        //    var task = _pkg.GetObjectFromPackagePath(@"\Package\DF\MyOleDbSource", out DtsProperty property);
        //    Assert.That(task, Is.TypeOf<TaskHost>());
        //    Assert.That(property, Is.Not.Null);
        //    Assert.That(property.GetValue(task), Is.EqualTo(string.Empty));
        //}

        [Test]
        public void ValidateSettings()
        {
            Assert.That(_pkg.Executables.Count, Is.EqualTo(1));

            var dataFlow = _pkg.Executables["DF"].ConvertTo<MainPipe>();
            Assert.That(dataFlow, Is.Not.Null);
            Assert.That(dataFlow.ComponentMetaDataCollection["MyOleDbSource"], Is.Not.Null);
            Assert.That(dataFlow.ComponentMetaDataCollection["MyOleDbTarget"], Is.Not.Null);
            Assert.That(dataFlow.PathCollection.OfType<IDTSPath100>().Any(path=>path.StartPoint.Name == "OLE DB Source Output" && path.EndPoint.Name == "OLE DB Destination Input"));
        }

        [Test]
        public void SavePackage()
        {
            Assert.DoesNotThrow(() => _pkg.Save($"{PackageFile}"));
        }
    }
}
