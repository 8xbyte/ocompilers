using Microsoft.Extensions.Configuration;
using Shared.Configuration.Exceptions;

namespace Shared.Configuration.Extensions {
    public static class JsonLoaderExtension {
        public static void AddJsonFilesFromDirectory(this IConfigurationBuilder configurationBuilder, string path) {
            if (path == null) throw new NullPathException();
            foreach (string file in Directory.EnumerateFiles(path, "*.json")) {
                configurationBuilder.AddJsonFile(file);
            }
        }
    }
}