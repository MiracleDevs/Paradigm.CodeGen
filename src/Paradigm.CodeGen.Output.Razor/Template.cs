using System;
using System.IO;
using System.Text;
using Paradigm.CodeGen.Output.Templating;

namespace Paradigm.CodeGen.Output.Razor
{
    public class Template : ITemplate
    {
        #region Properties

        public string Body { get; private set; }

        public string FileName { get; private set; }

        private int BodyStartLine { get; set; }

        #endregion

        #region Public Methods

        public void Open(string fileName)
        {
            this.FileName = fileName;
            this.Body = File.ReadAllText(fileName);

            this.ProcessBody();
        }

        public (int lineNumber, string line) GetLine(int lineNumber)
        {
            return (lineNumber < this.BodyStartLine ? lineNumber :  lineNumber - this.BodyStartLine, this.GetTemplateLine(lineNumber));
        }

        #endregion

        #region Private Methods

        private void ProcessBody()
        {
            if (this.Body == null)
                return;

            int indexOfInclude;
            var directory = Path.GetDirectoryName(this.FileName);

            if (directory == null)
                return;

            while ((indexOfInclude = this.Body.IndexOf("@include", StringComparison.OrdinalIgnoreCase)) >= 0)
            {
                var firstQuote = this.Body.IndexOf("\"", indexOfInclude, StringComparison.OrdinalIgnoreCase);

                if (firstQuote < 0 || firstQuote >= this.Body.Length)
                    throw new Exception("Missing opening quote on include sentence.");

                var lastQuote = this.Body.IndexOf("\"", firstQuote + 1, StringComparison.OrdinalIgnoreCase);

                if (lastQuote < 0)
                    throw new Exception("Missing ending quote on include sentence.");

                var path = Path.GetFullPath($"{directory}/{this.Body.Substring(firstQuote + 1, lastQuote - firstQuote - 1)}");

                var builder = new StringBuilder(this.Body);
                var include = this.GetInclude(path);
                this.BodyStartLine += this.CountLines(include);

                builder.Remove(indexOfInclude, lastQuote - indexOfInclude + 1);
                builder.Insert(indexOfInclude, include);

                this.Body = builder.ToString();
            }
        }

        private string GetInclude(string fileName)
        {
            var builder = new StringBuilder();
            var lines = File.ReadAllLines(fileName);
            var newLineLength = Environment.NewLine.Length;

            foreach(var line in lines)
            {
                if (line.StartsWith("using"))
                { 
                    builder.Append("@");
                    builder.Append(line);
                    builder.Remove(builder.Length - 1, 1);
                }
                else if (line.Replace(" ", "").StartsWith("publicstaticclass"))
                {
                    builder.Append("@functions");                  
                }
                else if (!string.IsNullOrWhiteSpace(line))
                {
                    builder.Append(line);
                }
                else
                {
                    continue;
                }

                builder.AppendLine();
            }          

            return builder.Remove(builder.Length - newLineLength, newLineLength).ToString();
        }

        private int CountLines(string text)
        {
            const char cr = '\r';
            const char lf = '\n';
            const char NULL = (char)0;
            var currentLineNumber = 0;
            var previousCharacter = '\0';

            for (var c = 0; c < text.Length - 1; c++)
            {
                var currentCharacter = text[c];

                if (currentCharacter == NULL || (currentCharacter == lf && previousCharacter == cr))
                {
                    continue;
                }

                if (currentCharacter == cr || (currentCharacter == lf && previousCharacter != cr))
                {
                    currentLineNumber++;
                }

                previousCharacter = currentCharacter;
            }

            return currentLineNumber;
        }

        private string GetTemplateLine(int lineNumber)
        {
            const char cr = '\r';
            const char lf = '\n';
            const char NULL = (char)0;

            var currentLineNumber = 0;
            var previousCharacter = '\0';
            var line = "";


            for (var c = 0; c < this.Body.Length - 1; c++)
            {
                var currentCharacter = this.Body[c];

                if (currentCharacter == NULL || (currentCharacter == lf && previousCharacter == cr))
                {
                    continue;
                }

                if (currentCharacter == cr || (currentCharacter == lf && previousCharacter != cr))
                {
                    currentLineNumber++;
                    if (currentLineNumber > lineNumber + 1)
                    {
                        return line;
                    }
                }

                if (currentLineNumber == lineNumber - 1 && currentCharacter != lf && currentCharacter != cr && currentCharacter != '\t')
                {
                    line += currentCharacter;
                }

                previousCharacter = currentCharacter;
            }

            return string.Empty;
        }

        #endregion
    }
}