using Paradigm.CodeGen.Logging;

namespace Paradigm.CodeGen.Output.Templating
{
    public interface ITemplateService
    {
        string Execute<T>(ITemplate template, T model, ILoggingService loggingService);
    }
}