using Microsoft.Extensions.Configuration;
using Shared.Configuration.Exceptions;

namespace Shared.Configuration.Extensions {
    public static class XmlLoaderExtension {
        public static void AddXmlFilesFromDirectory(this IConfigurationBuilder configurationBuilder, string path) {
            if (path == null) throw new NullPathException();
            foreach (string file in Directory.EnumerateFiles(path, "*.xml")) {
                configurationBuilder.AddXmlFile(file);
            }
        }
    }
}