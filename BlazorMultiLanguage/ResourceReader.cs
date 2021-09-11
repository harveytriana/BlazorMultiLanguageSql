// --------------------------------
// blazorspread.net
// --------------------------------
using System.IO;
using System.Linq;
using System.Reflection;

namespace BlazorMultiLanguage
{
    public static class ResourceReader
    {
        public static string Read(string name)
        {
            // Determine path
            var assembly = Assembly.GetExecutingAssembly();
            string resourcePath = name;
            // Format: "{Namespace}.{Folder}.{filename}.{Extension}"
            resourcePath = assembly.GetManifestResourceNames()
                                   .Single(_ => _.EndsWith(name));

            using var stream = assembly.GetManifestResourceStream(resourcePath);
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
