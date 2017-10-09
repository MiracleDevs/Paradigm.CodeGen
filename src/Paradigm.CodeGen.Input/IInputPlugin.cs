using Paradigm.CodeGen.Input.Models.Configuration;
using Paradigm.Core.DependencyInjection.Interfaces;

namespace Paradigm.CodeGen.Input
{
    public interface IInputPlugin
    {
        void RegisterPlugin(InputConfiguration configuration, IDependencyBuilder builder);
    }
}