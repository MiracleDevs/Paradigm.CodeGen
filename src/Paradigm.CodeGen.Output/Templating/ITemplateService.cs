using System.Threading.Tasks;
using Paradigm.CodeGen.Logging;

namespace Paradigm.CodeGen.Output.Templating
{
    public interface ITemplateService
    {
        Task<string> ExecuteAsync<T>(ITemplate template, T model, ILoggingService loggingService);
    }
}