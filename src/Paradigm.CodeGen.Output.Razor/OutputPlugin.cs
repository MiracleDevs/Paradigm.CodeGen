using Paradigm.CodeGen.Output.Models.Configuration;
using Paradigm.CodeGen.Output.Templating;
using Paradigm.Core.DependencyInjection.Interfaces;

namespace Paradigm.CodeGen.Output.Razor
{
    public class OutputPlugin: IOutputPlugin
    {
        public void RegisterPlugin(OutputConfiguration configuration, IDependencyBuilder builder)
        {
            builder.Register<ITemplateService, TemplateService>();                
            builder.Register<ITemplate, Template>();
            builder.Register<IOutputService, OutputService>();
        }
    }
}