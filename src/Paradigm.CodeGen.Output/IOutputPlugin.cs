using Paradigm.CodeGen.Output.Models.Configuration;
using Paradigm.Core.DependencyInjection.Interfaces;

namespace Paradigm.CodeGen.Output
{
    public interface IOutputPlugin
    {
        void RegisterPlugin(OutputConfiguration configuration, IDependencyBuilder builder);
    }
}