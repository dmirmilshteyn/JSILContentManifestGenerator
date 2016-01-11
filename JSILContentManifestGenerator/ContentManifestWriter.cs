// Parts obtained from JSIL (https://github.com/sq/JSIL)

// The MIT License(MIT)

// Copyright(c) 2011-2015 Katelyn Gadd
// Some sponsored contributions(c) Mozilla Corporation

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace JSILContentManifestGenerator
{
    public class ContentManifestWriter : IDisposable
    {
        public readonly string OutputPath;
        public readonly string Name;

        private readonly JavaScriptSerializer Serializer;
        private readonly StreamWriter Output;
        private bool Disposed = false;

        public ContentManifestWriter(string outputPath, string name) {
            OutputPath = outputPath;
            Name = name;

            Serializer = new JavaScriptSerializer();
            Output = new StreamWriter(outputPath, false, new UTF8Encoding(false));

            WriteHeader();
        }

        private void WriteHeader() {
            Output.WriteLine("if (typeof (contentManifest) !== \"object\") { contentManifest = {}; };");
            Output.WriteLine("contentManifest[{0}] = [", EscapeString(Name, forJson: true));
        }

        private void WriteFooter() {
            Output.WriteLine("];");
        }

        public void Add(string type, string path, object properties) {
            path = path.Replace("\\", "/");

            Output.WriteLine(
                "    [{0}, {1}, {2}],",
                EscapeString(type, forJson: true),
                EscapeString(path, forJson: true),
                Serializer.Serialize(properties)
            );
        }

        public void Dispose() {
            if (Disposed)
                return;

            Disposed = true;
            WriteFooter();
            Output.Dispose();
        }

        public static string EscapeCharacter(char character, bool forJson) {
            switch (character) {
                case '\'':
                    return @"\'";
                case '\\':
                    return @"\\";
                case '"':
                    return "\\\"";
                case '\t':
                    return @"\t";
                case '\r':
                    return @"\r";
                case '\n':
                    return @"\n";
                default: {
                        if (forJson || (character > 255))
                            return String.Format(@"\u{0:x4}", (int)character);
                        else
                            return String.Format(@"\x{0:x2}", (int)character);
                    }
            }
        }

        public string EscapeString(string text, char quoteCharacter = '\"', bool forJson = false) {
            if (text == null)
                return "null";

            var sb = new StringBuilder();

            sb.Clear();
            sb.Append(quoteCharacter);

            foreach (var ch in text) {
                if (ch == quoteCharacter)
                    sb.Append(EscapeCharacter(ch, forJson));
                else if (ch == '\\')
                    sb.Append(@"\\");
                else if ((ch < ' ') || (ch > 127))
                    sb.Append(EscapeCharacter(ch, forJson));
                else
                    sb.Append(ch);
            }

            sb.Append(quoteCharacter);

            return sb.ToString();
        }
    }
}
