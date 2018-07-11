namespace FluentSsis.Tests.Factories
{
    using System;
    using FluentSsis.Factories;
    using Microsoft.SqlServer.Dts.Runtime;
    using NSubstitute.ExceptionExtensions;
    using NUnit.Framework;

    [TestFixture]
    public class ExecutableFactoryTests
    {
        private ExecutableFactory _factory;

        [SetUp]
        public void Setup()
        {
            _factory = new ExecutableFactory();
        }

        [TestCase("")]
        [TestCase(null)]
        public void NewWithBadMoniker(string moniker)
        {
            Assert.Throws<ArgumentException>(() => _factory.New<EventsProvider>(moniker));
        }

        [Test]
        public void NewWithInvalidMoniker()
        {
            const string iDoNotExist = "I Do Not Exist";
            Assert.That(
                Assert.Throws<InvalidOperationException>(() => _factory.New<EventsProvider>(iDoNotExist)).Message,
                Is.EqualTo($"The executable could not be created. This is often caused when the moniker ({iDoNotExist}) does not match any installed SSIS executables."));
        }

        [TestCase("Microsoft.ExecuteSQLTask", typeof(TaskHost))]
        [TestCase("Microsoft.Pipeline", typeof(TaskHost))]
        [TestCase("STOCK:SEQUENCE", typeof(Sequence))]
        public void New(string moniker, Type expectedReturnType)
        {
            Assert.That(_factory.New<EventsProvider>(moniker), Is.InstanceOf(expectedReturnType));
        }

    }
}
