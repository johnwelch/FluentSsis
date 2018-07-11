using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentSsis.Tests.DataFlow
{
    using FluentSsis.DataFlow;
    using FluentSsis.Model;
    using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
    using Microsoft.SqlServer.Dts.Runtime;
    using NUnit.Framework;

    [TestFixture]
    public class ComponentExtensions
    {
        private MainPipe _pipeline;
        private Component _component;

        [SetUp]
        public void Setup()
        {
            var package = new Package();
            var task = package.Executables.Add("Microsoft.Pipeline") as TaskHost;
            _pipeline = task.InnerObject as MainPipe;
            _component = new Component(new Dataflow(package.Connections, package.Variables, _pipeline), "Microsoft.OleDBSource");
        }

        [TestCase("TestName")]
        public void Named(string name)
        {
            Assert.That(_component.Named(name), Is.InstanceOf<Component>());
            Assert.That(_component.ComponentMetadata.Name, Is.EqualTo(name));
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("   ")]
        public void NamedWithInvalidValue(string name)
        {
            Assert.Throws<ArgumentException>(() => _component.Named(name));
        }
    }
}
