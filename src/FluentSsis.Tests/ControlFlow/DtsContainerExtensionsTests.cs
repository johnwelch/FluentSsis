using System;
using System.Runtime.InteropServices;
using FluentSsis.ControlFlow;
using Microsoft.SqlServer.Dts.Runtime;
using NSubstitute;
using NUnit.Framework;

namespace FluentSsis.Tests.ControlFlow
{
    using FluentSsis.Factories;

    [TestFixture]
    public class DtsContainerExtensionsTests
    {
        [Test]
        public void AddOrUpdateTest()
        {
            var name = "Test";

            var pkg = new Package()
                .AddOrUpdate(New.Variable(name, "123456789"));
            int beforeCount = pkg.Variables.Count;

            Assert.That(pkg.Variables.Contains(name), Is.True);
            Assert.That(pkg.Variables[name].Value, Is.EqualTo("123456789"));

            pkg.AddOrUpdate(New.Variable(name, "987654321"));

            Assert.That(pkg.Variables.Contains(name), Is.True);
            Assert.That(pkg.Variables.Count, Is.EqualTo(beforeCount));
            Assert.That(pkg.Variables[name].Value, Is.EqualTo("987654321"));

            // Change type
            pkg.AddOrUpdate(New.Variable(name, 123));

            Assert.That(pkg.Variables.Contains(name), Is.True);
            Assert.That(pkg.Variables.Count, Is.EqualTo(beforeCount));
            Assert.That(pkg.Variables[name].Value, Is.EqualTo(123));

        }

    }
}
