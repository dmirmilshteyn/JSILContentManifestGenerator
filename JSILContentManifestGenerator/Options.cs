using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSILContentManifestGenerator
{
    class Options
    {
        [Option('p', "paths", Required = true, HelpText = "Input files or directories to be included in the content manifest.")]
        public IEnumerable<string> InputPaths { get; set; }

        [Option('b', "base", Required = true, HelpText = "The base directory to place the generated manifest and use as a reference for parsing files.")]
        public string BaseDirectory { get; set; }
    }
}
