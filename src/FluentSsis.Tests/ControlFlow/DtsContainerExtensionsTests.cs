namespace FluentSsis.Tests.ControlFlow
{
    using FluentSsis.ControlFlow;
    using FluentSsis.Factories;
    using Microsoft.SqlServer.Dts.Runtime;
    using NUnit.Framework;

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

        [Test]
        public void ParentAs()
        {
            Package pkg = new Package();
            Sequence seq = pkg.New<Sequence>("STOCK:SEQUENCE");
            TaskHost task = seq.New<TaskHost>("Microsoft.Pipeline");

            Assert.That(task.ParentAs<Sequence>(), Is.TypeOf<Sequence>());
            Assert.That(seq.ParentAs<Package>(), Is.TypeOf<Package>());
        }

    }
}
