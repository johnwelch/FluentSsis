namespace FluentSsis.Tests.DataFlow
{
    using System.Linq;
    using FluentSsis.ControlFlow;
    using FluentSsis.DataFlow;
    using FluentSsis.Emitter;
    using FluentSsis.Factories;
    using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
    using Microsoft.SqlServer.Dts.Runtime;
    using NUnit.Framework;

    [TestFixture]
    public class DataFlowIntegrationTests
    {
        private const string PackageDirectory = @"C:\Temp\TestPackageBuilder\TestPackageBuilder\";
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
            //Assert.That(dataFlow.PathCollection.OfType<IDTSPath100>().Any(path=>path.StartPoint.Name == "OLE DB Source Output" && path.EndPoint.Name == "OLE DB Destination Input"));
        }

        [Test]
        public void SavePackage()
        {
            Assert.DoesNotThrow(() => _pkg.Save($"{PackageDirectory}{_pkg.Name}.dtsx"));
        }
    }
}
