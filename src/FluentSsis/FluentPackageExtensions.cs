namespace FluentSsis
{
    using System;
    using System.IO;
    using Microsoft.SqlServer.Dts.Runtime;

    public static class FluentPackageExtensions
    {
        public static void Save(this Package package, string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            if (!Directory.Exists(Path.GetDirectoryName(fileName)))
            {
                throw new DirectoryNotFoundException();
            }

            Factory.Instance.SaveToFile(package, fileName);
        }
    }
}
