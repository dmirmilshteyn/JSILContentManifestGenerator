using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JSILContentManifestGenerator
{
    class Program
    {
        static void Main(string[] args) {
            var result = CommandLine.Parser.Default.ParseArguments<Options>(args);

            int code = result.MapResult(options =>
            {
                string baseDirectory = options.BaseDirectory;
                string manifestPath = Path.Combine(baseDirectory, "Content.manifest.js");

                using (ContentManifestWriter manifestWriter = new ContentManifestWriter(manifestPath, "Content")) {
                    foreach (string path in options.InputPaths) {
                        string fullInputPath = Path.Combine(baseDirectory, path);

                        if (Directory.Exists(fullInputPath)) {
                            // This is a directory, process all files within it
                            foreach (string file in Directory.EnumerateFiles(fullInputPath, "*", SearchOption.AllDirectories)) {
                                AddFile(manifestWriter, baseDirectory, file);
                            }
                        } else if (File.Exists(path)) {
                            AddFile(manifestWriter, baseDirectory, fullInputPath);
                        }
                    }
                }

                return 0;
            },
            errors =>
            {
                foreach (Error error in errors) {
                    Console.Error.WriteLine(error.ToString());
                }
                return 1;
            });
        }

        private static void AddFile(ContentManifestWriter manifestWriter, string baseDirectory, string filePath) {
            var fileInfo = new FileInfo(filePath);

            string adjustedFilePath = filePath.Substring(baseDirectory.Length + 1);

            manifestWriter.Add("File", adjustedFilePath, new Dictionary<string, object> { { "sizeBytes", fileInfo.Length } });
        }
    }
}
