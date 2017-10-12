using System;
using System.Threading.Tasks;
using Paradigm.CodeGen.Logging;
using Paradigm.CodeGen.Output.Templating;
using RazorLight;
using RazorLight.Razor;

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
            this.TemplateEngine = new EngineFactory().Create(this.Project);
        }

        public async Task<string> ExecuteAsync<T>(ITemplate template, T model, ILoggingService loggingService)
        {
            try
            {
                return await this.TemplateEngine.CompileRenderAsync(template.FileName, model).ConfigureAwait(false);
            }
            catch (TemplateCompilationException ex)
            {
                ProcessException(loggingService, ex);

                foreach (var parseError in ex.CompilationErrors)
                {
                    loggingService.Error($"     - {parseError}");
                }

                return null;
            }
            catch (Exception ex)
            {
                ProcessException(loggingService, ex);
                return null;
            }
        }

        private static void ProcessException(ILoggingService loggingService, Exception ex)
        {
            do
            {
                loggingService.Error(ex.Message);
                ex = ex.InnerException;

            } while (ex != null);
        }
    }

    public class TemplateParsingException : Exception
    {
    }
}