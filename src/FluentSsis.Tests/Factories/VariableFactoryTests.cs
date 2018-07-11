namespace FluentSsis.Tests.Factories
{
    using System;
    using FluentSsis.Factories;
    using Microsoft.SqlServer.Dts.Runtime;
    using NUnit.Framework;

    [TestFixture]
    public class VariableFactoryTests
    {
        private VariableFactory _factory;

        [SetUp]
        public void Setup()
        {
            _factory = new VariableFactory();
        }

        [TestCase("Name", "", "Name", "User")]
        [TestCase("User::Name", "", "Name", "User")]
        [TestCase("My::Name", "", "Name", "My")]
        public void CreateTest(string name, object value, string expectedName, string expectedNamespace)
        {
            Variable variable = null;
            Assert.DoesNotThrow(() => variable = _factory.Create(name, value));
            Assert.That(variable, Is.Not.Null);
            Assert.That(variable.Name, Is.EqualTo(expectedName));
            Assert.That(variable.Namespace, Is.EqualTo(expectedNamespace));
            Assert.That(variable.Value, Is.EqualTo(value));
        }

        // [TestCase("My&Name")]  // This case throws a DtsRuntimeException, not sure I want to reimplement those rules here.
        [TestCase("")]
        [TestCase(null)]
        [TestCase("  ")]
        [TestCase("My::User::Name")]
        public void CreateWithInvalidNameTest(string name)
        {
            Assert.Throws<ArgumentException>(() => _factory.Create(name, null));
        }
    }
}
