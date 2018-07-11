namespace FluentSsis.ControlFlow
{
    using System;
    using Microsoft.SqlServer.Dts.Runtime;

    internal sealed class PackageSingleton
    {
        private static readonly Lazy<Package> Lazy =
            new Lazy<Package>(() => new Package());

        public static Package Instance => Lazy.Value;
    }
}
