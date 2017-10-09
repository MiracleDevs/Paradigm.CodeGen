using System;
using System.IO;
using Paradigm.CodeGen.Logging;
using Paradigm.CodeGen.Output.Templating;
using Paradigm.Core.Extensions;
using RazorLight;
using RazorLight.Extensions;

namespace Paradigm.CodeGen.Output.Razor
{
    public class TemplateService : ITemplateService
    {
        private IRazorLightEngine TemplateEngine { get; }

        public TemplateService()
        {
            var location = typeof(TemplateService).GetAssembly().Location;
            var path = Path.GetDirectoryName(location);
            this.TemplateEngine = EngineFactory.CreatePhysical(path);
        }

        public string Execute<T>(ITemplate template, T model, ILoggingService loggingService)
        {
            try
            {
                return this.TemplateEngine.ParseString(template.Body, model, typeof(T));
            }
            catch(TemplateParsingException ex)
            {
                this.ProcessException(loggingService, ex);

                foreach (var parseError in ex.ParserErrors)
                {
                    loggingService.Error($"     - {parseError.Message}");
                }

                return null;
            }
            catch (Exception ex)
            {
                this.ProcessException(loggingService, ex);
                return null;
            }          
        }

        private void ProcessException(ILoggingService loggingService, Exception ex)
        {
            do
            {
                loggingService.Error(ex.Message);
                ex = ex.InnerException;

            } while (ex != null);
        }
    }
}