using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.SqlServer.Dts.Runtime;

namespace FluentSsis
{
    public class Factory
    {
        #region Singleton Pattern
        private static readonly Lazy<Factory> FactoryInstance = new Lazy<Factory>(() => new Factory());
        public static Factory Instance => FactoryInstance.Value;
        #endregion

        private readonly Application _application = new Application();

        public void SaveToFile(Package package, string fileName)
        {
            _application.SaveToXml(fileName, package, new EventLogger());
        }

        public Package CreatePackage(string name = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return new Package();
            }
            else
            {
                return new Package { Name = name };
            }
        }
    }
}
