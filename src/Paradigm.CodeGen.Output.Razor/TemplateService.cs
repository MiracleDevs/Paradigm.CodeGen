using Paradigm.CodeGen.Logging;
using Paradigm.CodeGen.Output.Templating;
using RazorLight;
using RazorLight.Compilation;
using RazorLight.Razor;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Paradigm.CodeGen.Output.Razor
{
    public class TemplateService : ITemplateService
    {
        private IRazorLightEngine TemplateEngine { get; }

        private RazorLightProject Project { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateService"/> class.
        /// </summary>
        public TemplateService()
        {
            this.Project = new TemplateCacheRazorLightProject();
            this.TemplateEngine = new RazorLightEngineBuilder()
                .UseProject(this.Project)
                .Build();
        }

        public async Task<string> ExecuteAsync<T>(ITemplate template, T model, ILoggingService loggingService)
        {
            try
            {        
                return await this.TemplateEngine.CompileRenderAsync(template.FileName, model).ConfigureAwait(false);
            }
            catch (TemplateCompilationException ex)
            {
                ProcessException(template, model, loggingService, ex);

                foreach (var parseError in ex.CompilationErrors)
                {
                    loggingService.Error($"     - {parseError}");
                }

                return null;
            }
            catch (Exception ex)
            {
                ProcessException(template, model, loggingService, ex);
                return null;
            }
        }

        private static void ProcessException<T>(ITemplate template, T model, ILoggingService loggingService, Exception ex)
        {

            var builder = new StringBuilder();

            builder.AppendLine();
            builder.AppendLine("-----------------------------------------------------");
            builder.AppendLine(ex.Message);
            builder.AppendLine("-----------------------------------------------------");
            builder.AppendFormat("      - Model Type: {0}", typeof(T));
            builder.AppendLine();
            builder.AppendFormat("      - Model: {0}", model);
            builder.AppendLine();
            builder.AppendFormat("      - Template: {0}", template.FileName);
            builder.AppendLine();
            builder.AppendFormat("      - Lines: {0}{1}", Environment.NewLine, GetErrorLines(template, ex));
            builder.AppendLine();
            builder.AppendFormat("      - Stack Trace: {0}", ex.StackTrace);
            builder.AppendLine();
            builder.AppendLine("-----------------------------------------------------");

            do
            {
                builder.AppendLine(ex.Message);
                ex = ex.InnerException;

            } while (ex != null);

            builder.AppendLine("-----------------------------------------------------");
            loggingService.Error(builder.ToString());
        }

        private static string GetErrorLines(ITemplate template, Exception ex)
        {
            var stackTrace = ex.StackTrace;
            var index = 0;
            var builder = new StringBuilder();
            const string lookUpString = ".tpl:line";

            while ((index = stackTrace.IndexOf(lookUpString, index, StringComparison.InvariantCulture)) >= 0)
            {
                try
                {
                    index += lookUpString.Length;
                    var endOfLine = stackTrace.IndexOf(Environment.NewLine, index, StringComparison.InvariantCulture);
                    var lineNumber = int.Parse(stackTrace.Substring(index, endOfLine - index));
                    var line = template.GetLine(lineNumber);

                    builder.AppendFormat("          - Line [{0}]: {1}", line.lineNumber, line.line);
                    builder.AppendLine();
                }
                catch
                {
                    // do nothing.
                }
            }

            return builder.ToString();
        }
    }

    public class TemplateParsingException : Exception
    {
    }
}