using System;
using System.IO;
using RazorLight.Razor;

namespace Paradigm.CodeGen.Output.Razor
{
    public sealed class TemplateCacheRazorLightProjectItem : RazorLightProjectItem
    {
        private Stream Content { get; }

        public override string Key { get; }

        public override bool Exists => true;

        public TemplateCacheRazorLightProjectItem(string content, string key)
        {
            this.Content = GenerateStreamFromString(content) ?? throw new ArgumentNullException(nameof(content));
            this.Key = key ?? throw new ArgumentNullException(nameof(key));
        }

        public override Stream Read()
        {
            return this.Content;
        }

        private static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            writer.Write(s);

            writer.Flush();
            stream.Position = 0;

            return stream;
        }
    }
}